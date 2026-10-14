using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace prog_part_two.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {

        [HttpPost("login")]
        public IActionResult Logins(string organiserName, string organiserEmail, int organiserPassword)
        {
            string connectionString = @"Data source=(localdb)\testingPoe;database=poe_prac1;";
            // Query looks up the organiser by name and email
            string query = "SELECT Id, OrganiserName FROM Organiser WHERE OrganiserName = @Name AND OrganiserEmail = @Email";

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", organiserName);
                        cmd.Parameters.AddWithValue("@Email", organiserEmail);
                        cmd.Parameters.AddWithValue("@Password", organiserPassword);

                        connect.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);

                                // Login successful: Return identity data to the client
                                return Ok(new
                                {
                                    message = "Login successful",
                                    organiserId = id,
                                    organiserName = name
                                });
                            }
                        }
                    }
                }
                // If no record matches
                return Unauthorized(new { message = "Invalid Organiser name or email." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("participant/login")]
        public IActionResult ParticipantLogin(string participantName, string participantEmail, int participantPassword)
        {
            string connectionString = @"Data source=(localdb)\testingPoe;database=poe_prac1;";
            string query = "SELECT ParticipantId, ParticipantName FROM Participant WHERE ParticipantName = @Name AND ParticipantEmail = @Email";

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", participantName);
                        cmd.Parameters.AddWithValue("@Email", participantEmail);
                        cmd.Parameters.AddWithValue("@Password", participantPassword);

                        connect.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return Ok(new
                                {
                                    message = "Participant login successful",
                                    participantId = reader.GetInt32(0),
                                    participantName = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
                return Unauthorized(new { message = "Invalid Participant name or email." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("events/categories")]
        public IActionResult GetEventCategories()
        {
            string connectionString = @"Data source=(localdb)\testingPoe;database=poe_prac1;";
            // Adjust table and column names to match your actual schema(change)
            string query = "SELECT EventType, CategoryName, EventDescription FROM Events";

            var eventList = new List<object>();

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        connect.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                eventList.Add(new
                                {
                                    EventType = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                    Category = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? "" : reader.GetString(2)
                                });
                            }
                        }
                    }
                }
                return Ok(eventList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
