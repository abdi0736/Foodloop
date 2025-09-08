using Foodloop.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Foodloop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BodControl : ControllerBase
{
        private static readonly List<Bod> _boder = new()
        {
            new Bod { Id = 1, Navn = "Pizza Palace", Kategori = "Sushimushi", Latitude = 55.6171, Longitude = 12.0796, Status = BodStatus.Aaben },
            new Bod { Id = 2, Navn = "Burger Bar", Kategori = "KØD", Latitude = 55.6180, Longitude = 12.0785, Status = BodStatus.Optaget },
            new Bod { Id = 3, Navn = "Sushi Station", Kategori = "Vegan", Latitude = 55.6165, Longitude = 12.0802, Status = BodStatus.Aaben },
            new Bod { Id = 4, Navn = "Taco Town", Kategori = "Thai food", Latitude = 55.6177, Longitude = 12.0811, Status = BodStatus.Lukket }
        };

        [HttpGet]
        [Route("")]  // Maps to GET /api/BodControl
        public ActionResult<IEnumerable<Bod>> GetBoder()
        {
            return Ok(_boder);
        }

        [HttpGet("{id}")]
        [Route("{id}")]  // Maps to GET /api/BodControl/1
        public ActionResult<Bod> GetBod(int id)
        {
            var bod = _boder.FirstOrDefault(b => b.Id == id);
            if (bod == null) return NotFound();
            return Ok(bod);
        }
    }
