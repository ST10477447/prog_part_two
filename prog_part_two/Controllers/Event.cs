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
    }
}
