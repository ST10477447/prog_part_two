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

        [HttpPut]
        public IActionResult UpdateById(int Id, Participants updated)
        {
            var Participants = participant.FirstOrDefault(participant => participant.Id == Id);
            if (Participants is null) return NotFound();
            Participants.ParticipantName = updated.ParticipantName;
            Participants.ParticipantSurname = updated.ParticipantSurname;
            Participants.ParticipantAge = updated.ParticipantAge;
            Participants.ParticipantEmail = updated.ParticipantEmail;
            Participants.ParticipantNumber = updated.ParticipantNumber;
            Participants.ParticipantGender = updated.ParticipantGender;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int Id)
        {
            participant.RemoveAll(participant => participant.Id == Id);
            return NoContent();
        }
    }
}
