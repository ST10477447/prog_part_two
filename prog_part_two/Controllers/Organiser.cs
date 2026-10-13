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

        [HttpGet("{id}")]
        public IActionResult GetById(int Id)
        {
            var Organisers = organiser.FirstOrDefault(organiser => organiser.Id == Id);
            return Organisers is not null ? Ok(Organisers) : NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateById(int Id, Organisers updated)
        {
            var Organisers = organiser.FirstOrDefault(organiser => organiser.Id == Id);
            if (Organisers is null) return NotFound();
            Organisers.OrganiserName = updated.OrganiserName;
            Organisers.OrganiserSurname = updated.OrganiserSurname;
            Organisers.OrganiserEmail = updated.OrganiserEmail;
            Organisers.OrganiserNumber = updated.OrganiserNumber;
            return NoContent();
        }
    }
}
