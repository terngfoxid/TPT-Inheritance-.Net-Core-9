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
                if (!json.TryGetProperty("componentType", out JsonElement typeElement))
                    return BadRequest("Missing 'componentType' field.");

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
                if (!json.TryGetProperty("componentType", out JsonElement typeElement))
                    return BadRequest("Missing 'componentType' field.");

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

        [HttpGet("example/banner_item")]
        public async Task<IActionResult> ExampleBannerItem()
        {
            Banner? banner = await _context.Banners.FirstOrDefaultAsync(b => b.Id == 67);
            banner.SetIdValue();
            return Ok(banner);
        }
    }
}
