using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using prog_part_two.Models;

namespace prog_part_two.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Participant : ControllerBase
    {
        private static List<Participants> participant = new()
        {
        };

        [HttpGet]
        private IActionResult GetAll() => Ok(participant);

        [HttpGet]
        public IActionResult GetById(int Id)
        {
            var Participants = participant.FirstOrDefault(participant => participant.Id == Id);
            return Participants is not null ? Ok(Participants) : NotFound();
        }
    }
}
