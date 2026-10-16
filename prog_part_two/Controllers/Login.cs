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
        public IActionResult Logins(string organiserName, string organiserEmail, string organiserPassword)
        {
            // 1. Basic validation
            if (string.IsNullOrWhiteSpace(organiserName) ||
                string.IsNullOrWhiteSpace(organiserEmail) ||
                !organiserEmail.Trim().EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrEmpty(organiserPassword) ||
                organiserPassword.Length < 8)
            {
                return Unauthorized(new { message = "Invalid Organiser name, email, or password." });
            }

            string connectionString = @"Data source=(localdb)\api_part_1;database=prog_part_ll;Integrated Security=True;TrustServerCertificate=True;";
            string query = "SELECT OrganiserId, OrganiserName, Password FROM Organiser WHERE OrganiserName = @Name AND OrganiserEmail = @Email";

            try
            {
                using SqlConnection connect = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand(query, connect);

                cmd.Parameters.AddWithValue("@Name", organiserName);
                cmd.Parameters.AddWithValue("@Email", organiserEmail);

                connect.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedHash = reader["Password"].ToString();

                    if (BCrypt.Net.BCrypt.Verify(organiserPassword, storedHash))
                    {
                        return Ok(new
                        {
                            message = "Login successful",
                            organiserId = Convert.ToInt32(reader["OrganiserId"]),
                            organiserName = reader["OrganiserName"].ToString()
                        });
                    }
                }

                return Unauthorized(new { message = "Invalid Organiser name, email, or password." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("participant/login")]
        public IActionResult ParticipantLogin(string participantName, string participantEmail, string participantPassword)
        {
            if (string.IsNullOrEmpty(participantEmail) ||
                !participantEmail.ToLower().Contains("gmail.com") ||
                string.IsNullOrEmpty(participantPassword) ||
                participantPassword.Length <= 8)
            {
                return Unauthorized(new { message = "Invalid participant name or email or password." });
            }

            string connectionString = @"Data source=(localdb)\api_part_1;database=prog_part_ll;";
            string query = "SELECT ParticipantId, ParticipantName, ParticipantPasswordhash FROM Participant WHERE ParticipantName = @Name AND ParticipantEmail = @Email";

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", participantName);
                        cmd.Parameters.AddWithValue("@Email", participantEmail);

                        connect.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // FIX: Retrieve columns safely by explicit string identifiers
                                string storedHash = reader.GetString(reader.GetOrdinal("ParticipantPasswordhash"));

                                if (BCrypt.Net.BCrypt.Verify(participantPassword, storedHash))
                                {
                                    return Ok(new
                                    {
                                        message = "Participant login successful.",
                                        participantId = reader.GetInt32(reader.GetOrdinal("ParticipantId")),
                                        participantName = reader.GetString(reader.GetOrdinal("ParticipantName"))
                                    });
                                }
                            }
                        }
                    }
                    return Unauthorized(new { message = "Invalid Participant name or email or password." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpGet("events/categories")]
        public IActionResult GetEventCategories()
        {
            string connectionString = @"Data source=(localdb)\api_part_1;database=prog_part_ll;";
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
