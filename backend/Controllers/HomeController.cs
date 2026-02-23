using backend.DTO;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly CnpmContext _context;

        public HomeController(IConfiguration config, CnpmContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var apiKey = Request.Headers["x-api-key"].FirstOrDefault();

            if (apiKey != _config["API_KEY"])
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "SAI KEY RỒI THẰNG NGU HAHA",
                    data = "Data CON CAC"
                });
            }

            var Hello = "hello";

            return Ok(new
            {
                success = true,
                message = "Thành công",
                data = Hello
            });
        }

        [HttpGet("hottel")]
        public async Task<IActionResult> Hottel()
        {

            var hottels = await _context.Hottels.ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Thành công",
                data = hottels
            });
        }

        [HttpGet("tourist_area")]
        public async Task<IActionResult> tourist_area()
        {
            var tourist_area = await _context.TouristAreas.ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Thành công",
                data = tourist_area
            });
        }

    }
}
