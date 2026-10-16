using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using prog_part_two.Models;
using BCrypt.Net;

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
       

        [HttpGet]
        public IActionResult GetById(int Id)
        {
            var Participants = participant.FirstOrDefault(participant => participant.ParticipantId == Id);
            return Participants is not null ? Ok(Participants) : NotFound();
        }

        [HttpPut]
        public IActionResult UpdateById(int Id, Participants updated)
        {
            var Participants = participant.FirstOrDefault(participant => participant.ParticipantId == Id);
            if (Participants is null) return NotFound();
            Participants.ParticipantName = updated.ParticipantName;
            Participants.ParticipantSurname = updated.ParticipantSurname;
            Participants.ParticipantAge = updated.ParticipantAge;
            Participants.ParticipantEmail = updated.ParticipantEmail;
            Participants.ParticipantNumber = updated.ParticipantNumber;
            Participants.ParticipantGender = updated.ParticipantGender;
            Participants.Password = updated.Password;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int Id)
        {
            participant.RemoveAll(participant => participant.ParticipantId == Id);
            return NoContent();
        }

        [HttpPost]
        public string store_participantInfo(string ParticipantName, string ParticipantSurname, int ParticipantAge, string ParticipantGender, string ParticipantEmail, int ParticipantNumber, string Password)
        {
            string message = "";

            try
            {
                if (ParticipantEmail != null && !ParticipantEmail.ToLower().Contains("gmail.com") && Password != null && Password.Length <= 8)
                {
                    return "Invalid participant email or password.";
                }

                string connection_string = @"Data source=(localdb)\api_part_1;database=prog_part_ll;";

                SqlConnection connect = new SqlConnection(connection_string);

                connect.Open();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(Password, workFactor: 12);

                string query = @"insert into Participant values ('" + ParticipantName + "', '" + ParticipantSurname + "', '" + ParticipantAge + "' ,'" + ParticipantGender + "', '" + ParticipantEmail + "', '" + ParticipantNumber + "', '" + hashPassword + "')";

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

        [HttpPost("enrol")]
        public string EnrolToEvent(int participantId, int eventId, string enrollmentDate)
        {
            string message = "";
            string connection_string = @"Data source=(localdb)\api_part_1;database=prog_part_ll;";

            // Basic data sanity validation
            if (participantId <= 0 || eventId <= 0)
            {
                return "Invalid input data: ID parameters must be greater than zero.";
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(connection_string))
                {
                    connect.Open();

                    // Check if the link already exists to give a friendly error
                    string checkQuery = "SELECT COUNT(*) FROM EventEnrollments WHERE ParticipantId = @ParticipantId AND EventId = @EventId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connect))
                    {
                        checkCmd.Parameters.AddWithValue("@ParticipantId", participantId);
                        checkCmd.Parameters.AddWithValue("@EventId", eventId);


                        int exists = (int)checkCmd.ExecuteScalar();
                        if (exists > 0)
                        {
                            return "You are already enrolled in this event.";
                        }
                    }

                    // Insert into the bridge table using parameters (Safe from SQL Injection)
                    string insertQuery = "INSERT INTO EventEnrollments (ParticipantId, EventId, EnrollmentDate) VALUES (@ParticipantId, @EventId, @EnrollmentDate)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connect))
                    {
                        insertCmd.Parameters.AddWithValue("@ParticipantId", participantId);
                        insertCmd.Parameters.AddWithValue("@EventId", eventId);
                        insertCmd.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate);


                        insertCmd.ExecuteNonQuery();
                        message = "Enrolled to event successfully!";
                    }
                }
            }
            catch (Exception ex)
            {
                // Catches database constraint errors (e.g. if Event ID doesn't exist)
                message = "Enrollment processing failed: " + ex.Message;
            }

            return message;
        }

    }
}
