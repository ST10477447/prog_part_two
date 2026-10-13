using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prog_part_two.Models;

namespace prog_part_two.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Organiser : ControllerBase
    {
        private static List<Organisers> organiser = new()
        {
        };

        [HttpGet]
        public IActionResult GetAll() => Ok(organiser);


    }
}
