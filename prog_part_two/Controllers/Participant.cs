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

        [HttpPost]
        public string store_participantInfo(string ParticipantName, string ParticipantSurname, int ParticipantAge, string ParticipantEmail, int ParticipantNumber, string ParticipantGender)
        {
            string message = "";

            try
            {
                string connection_string = @"Data source=(localdb)\testingPoe;database=poe_prac1;";

                SqlConnection connect = new SqlConnection(connection_string);

                connect.Open();

                string query = @"insert into Participant values ('" + ParticipantName + "', '" + ParticipantSurname + "', '" + ParticipantAge + "' ,'" + ParticipantEmail + "', '" + ParticipantNumber + "', '" + ParticipantGender + "')";

                SqlCommand run_query = new SqlCommand(query, connect);

                run_query.ExecuteNonQuery();

                message = "Participant info saved successfully";

                connect.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }
    }
}
