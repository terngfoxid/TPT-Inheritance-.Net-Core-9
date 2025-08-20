using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inheritance_Test.Data;
using Inheritance_Test.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.Json;
using System.Reflection.Metadata;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;

namespace Inheritance_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController : ControllerBase
    {
        private readonly CustomContext _context;

        public ComponentController(CustomContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }


        [HttpGet("classname")]
        public async Task<IActionResult> ClassName()
        {
            string targetNamespace = "Inheritance_Test.Models";
            Assembly assembly = Assembly.GetExecutingAssembly();
            var classNamesInNamespace = assembly.GetTypes()
                .Where(t => t.IsClass && t.Namespace == targetNamespace)
                .Select(t => new { 
                    Name = t.Name,
                    Namespace = t.Namespace,
                    FullName = t.FullName,
                }).ToList();

            return Ok(classNamesInNamespace);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] JsonElement json)
        {
            try
            {
                if (!json.TryGetProperty("type", out JsonElement typeElement))
                    return BadRequest("Missing 'type' field.");

                string? typeName = typeElement.GetString();
                Type? type = Type.GetType(typeName);

                if (type == null)
                    return BadRequest($"Unknown component type: {typeName}");

                string rawJson = json.GetRawText();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                object? deserialized = System.Text.Json.JsonSerializer.Deserialize(rawJson, type, options);

                if (deserialized == null)
                    return BadRequest("Failed to deserialize the input.");

                if (deserialized is Component component)
                {
                    component.create();
                    return Ok(component);
                }

                // Or handle generic object
                return Ok(new { Message = $"Deserialized to {type.Name}", Data = deserialized });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] JsonElement json)
        {
            try
            {
                if (!json.TryGetProperty("type", out JsonElement typeElement))
                    return BadRequest("Missing 'type' field.");

                string? typeName = typeElement.GetString();
                Type? type = Type.GetType(typeName);

                if (type == null)
                    return BadRequest($"Unknown component type: {typeName}");

                string rawJson = json.GetRawText();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                object? deserialized = System.Text.Json.JsonSerializer.Deserialize(rawJson, type, options);

                if (deserialized == null)
                    return BadRequest("Failed to deserialize the input.");

                
                if (deserialized is Component component)
                {
                    dynamic target = deserialized;
                    component.Id = target.Id;
                    component.update();
                    return Ok(component);
                }

                // Or handle generic object
                return Ok(new { Message = $"Deserialized to {type.Name}", Data = deserialized });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        //get Example Structure
        [HttpGet("example/banner")]
        public async Task<IActionResult> ExampleBanner()
        {
            Banner banner = new Banner();
            return Ok(banner);
        }

        [HttpGet("example/page")]
        public async Task<IActionResult> ExamplePage()
        {
            Page page = new Page();
            return Ok(page);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int id)
        {
            Component? component = await _context.Components.FirstOrDefaultAsync(b => b.Id == id);
            try
            {
                Type? dynamicType = Type.GetType(component.Type);
                if (dynamicType != null)
                {

                    // 2. Get OfType<T>() and apply it to _context.Components
                    MethodInfo? ofTypeMethod = typeof(Queryable)
                        .GetMethods()
                        .FirstOrDefault(m => m.Name == "OfType" && m.IsGenericMethod && m.GetParameters().Length == 1)?
                        .MakeGenericMethod(dynamicType);

                    if (ofTypeMethod == null)
                        return StatusCode(500, "Failed to reflect OfType<T>()");

                    IQueryable baseQuery = _context.Components;
                    IQueryable filteredQuery = (IQueryable)ofTypeMethod.Invoke(null, new object[] { baseQuery })!;

                    // 3. Build expression: x => x.Id == id
                    var parameter = Expression.Parameter(dynamicType, "x");
                    var property = Expression.Property(parameter, "Id");
                    var constant = Expression.Constant(id);
                    var equality = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda(equality, parameter);

                    // 4. Apply Where<T>(source, predicate)
                    MethodInfo whereMethod = typeof(Queryable).GetMethods()
                        .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                        .MakeGenericMethod(dynamicType);

                    var whereCall = whereMethod.Invoke(null, new object[] { filteredQuery, lambda })!;

                    // 5. Reflect and invoke FirstOrDefaultAsync<T>
                    MethodInfo? firstOrDefaultAsync = typeof(EntityFrameworkQueryableExtensions)
                        .GetMethods()
                        .Where(m => m.Name == "FirstOrDefaultAsync" && m.GetParameters().Length == 2)
                        .First()
                        .MakeGenericMethod(dynamicType);

                    var task = (Task)firstOrDefaultAsync.Invoke(null, new object[] { whereCall, CancellationToken.None })!;
                    await task.ConfigureAwait(false);

                    var resultProperty = task.GetType().GetProperty("Result");
                    var result = resultProperty?.GetValue(task);

                    if (result is Component componentResult)
                    {
                        dynamic target = result;
                        target.Id = componentResult.Id;
                        return Ok(target);
                    }
                    return Ok(result);
                }
                return Ok(component);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
