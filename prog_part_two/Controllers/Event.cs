using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prog_part_two.Models;
using System.Data.SqlClient;

namespace prog_part_two.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Event : ControllerBase
    {
        private static List<Events> events = new()
        {
        };

        [HttpGet]
        public IActionResult GetAll() => Ok(events);

        [HttpGet("{id}")]
        public IActionResult GetById(int Id)
        {
            var Events = events.FirstOrDefault(events => events.Id == Id);
            return Events is not null ? Ok(Events) : NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateById(int Id, Events updated)
        {
            var Events = events.FirstOrDefault(events => events.Id == Id);
            if (Events is null) return NotFound();

            Events.EventName = updated.EventName;
            Events.EventDate = updated.EventDate;
            Events.EventLocation = updated.EventLocation;
            Events.EventDescription = updated.EventDescription;
            Events.OrganiserId = updated.OrganiserId;
            Events.ParticipantId = updated.ParticipantId;

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteById(int Id)
        {
            events.RemoveAll(events => events.Id == Id);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Organiser")]
        public string Events(int EventId, string EventName, string EventDate, string EventLocation, string EventDescription, int OrganiserId, int ParticipantId)
        {
            string message = "";

            try
            {
                string connection_string = @"Data source=(localdb)\testingPoe;database=poe_prac1;";
                using SqlConnection connect = new SqlConnection(connection_string);

                connect.Open();

                string query = @"INSERT INTO Events (EventId, EventName, EventDate, EventLocation, EventDescription, OrganiserId, ParticipantId)
                         VALUES (@EventId, @EventName, @EventDate, @EventLocation, @EventDescription, @OrganiserId, @ParticipantId)";

                SqlCommand run_query = new SqlCommand(query, connect);

                // Use parameters to avoid SQL injection
                run_query.Parameters.AddWithValue("@EventId", EventId);
                run_query.Parameters.AddWithValue("@EventName", EventName);
                run_query.Parameters.AddWithValue("@EventDate", EventDate);
                run_query.Parameters.AddWithValue("@EventLocation", EventLocation);
                run_query.Parameters.AddWithValue("@EventDescription", EventDescription);
                run_query.Parameters.AddWithValue("@OrganiserId", OrganiserId);
                run_query.Parameters.AddWithValue("@ParticipantId", ParticipantId);

                run_query.ExecuteNonQuery();

                message = "Event saved successfully";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        [HttpPost("create_event")]
        public IActionResult CreateEvent(string eventName, string eventDescription, int organiserId)
        {
            string connectionString = @"Data source=(localdb)\testingPoe;database=poe_prac1;";

            // 1. Check if the Organiser exists first
            string checkQuery = "SELECT COUNT(1) FROM Organiser WHERE Id = @OrganiserId";
            string insertQuery = "INSERT INTO Event (EventName, OrganiserId) VALUES (@EventName, @OrganiserId)";

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();

                    // Validate Organiser
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connect))
                    {
                        checkCmd.Parameters.AddWithValue("@OrganiserId", organiserId);
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (exists == 0)
                            return Unauthorized(new { message = "Only registered Organisers can create events." });
                    }

                    // Insert Event
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connect))
                    {
                        insertCmd.Parameters.AddWithValue("@EventName", eventName);
                        insertCmd.Parameters.AddWithValue("@EventDescription", eventDescription);
                        insertCmd.Parameters.AddWithValue("@OrganiserId", organiserId);
                        insertCmd.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Event created successfully by Organiser." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
