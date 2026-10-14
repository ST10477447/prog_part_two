using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
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
        public string login(string email, string password)
        {
            if (email.ToLower().Contains("gmail.com") && password.Length >= 8)
            {
                return "login successful";
            }
            else
            {
                return "incorrect email or password";
            }
        }

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
            Organisers.Password = updated.Password;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int Id)
        {
            organiser.RemoveAll(organiser => organiser.Id == Id);
            return NoContent();
        }

        [HttpPost]
        public string store_organiserInfo(string OrganiserName, string OrganiserSurname, int OrganiserAge, string OrganiserEmail, int OrganiserNumber, string Password)
        {
            string message = "";

            try
            {
                string connection_string = @"Data source=(localdb)\testingPoe;database=poe_prac1;";

                SqlConnection connect = new SqlConnection(connection_string);

                connect.Open();

                string query = @"insert into Organiser values ('" + OrganiserName + "', '" + OrganiserSurname + "', '" + OrganiserAge + "' ,'" + OrganiserEmail + "', '" + OrganiserNumber + "', '" + Password + "')";

                SqlCommand run_query = new SqlCommand(query, connect);

                run_query.ExecuteNonQuery();

                message = "Organiser info saved successfully";

                connect.Close();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        [HttpPost("store_participant")]
        public IActionResult StoreParticipantInfo(string name, string email, int organiserId)
        {
            string connectionString = @"Data source=(localdb)\testingPoe;database=poe_prac1;";
            string query = "INSERT INTO Participant (ParticipantName, ParticipantEmail, OrganiserId) VALUES (@Name, @Email, @OrganiserId)";

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand runQuery = new SqlCommand(query, connect))
                    {
                        runQuery.Parameters.AddWithValue("@Name", name);
                        runQuery.Parameters.AddWithValue("@Email", email);
                        runQuery.Parameters.AddWithValue("@OrganiserId", organiserId); // Links participant to organiser

                        connect.Open();
                        runQuery.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Participant linked to Organiser successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        
    }
}
