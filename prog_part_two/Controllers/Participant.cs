using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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



    }
}
