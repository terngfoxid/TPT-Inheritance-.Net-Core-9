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

namespace Inheritance_Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController : ControllerBase
    {
        private readonly CustomContext _context;
        private InheritanceTestContext OldContext = new InheritanceTestContext();

        public ComponentController(CustomContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var a = _context.Containers.Where(c => c.Id == 57).First();
            a.Containings = _context.Containings.Where(c => c.ContainerId == 57).ToList();
            foreach (var item in a.Containings)
            {
                item.Component = _context.Components.Where(t => t.Id == item.ComponentId).FirstOrDefault();

                if(item.Component.GetType() == typeof(Banner))
                {
                    ((Banner)item.Component).Subdetails = _context.Subdetails.Where(t => t.BannerId == item.ComponentId).ToList();
                }
            }

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(a, settings);

            return Ok(json);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            Page p = new Page();
            p.Name = "TestPage3";
            p.ContainerType = "Page3";
            p.Pagename = "PageTestPrototypeContainer3";
            Component Pcast = (Component)p;

            Banner banner = new Banner();
            banner.Name = "bannerC1";
            banner.ImageUrl = "urlc1";

            Subdetail sub = new Subdetail();
            sub.SubCode = "TestCode";
            sub.Sublink = "TestLink";
            banner.Subdetails.Add(sub);

            Component bc = (Component)banner;

            Textbox textbox = new Textbox();
            textbox.Name = "textboxc1";
            textbox.Header = "headerc1";
            textbox.Text = "textc1";
            Component tc = (Component)textbox;

            _context.Add(Pcast);
            _context.Add(bc);
            _context.Add(tc);
            _context.SaveChanges();

            Containing containing1 = new Containing();
            containing1.ContainerId = Pcast.Id;
            containing1.ComponentId = bc.Id;
            Containing containing2 = new Containing();
            containing2.ContainerId = Pcast.Id;
            containing2.ComponentId = tc.Id;

            _context.Add(containing1);
            _context.Add(containing2);
            _context.SaveChanges();

            return Ok("This is Create");
        }

    }
}
