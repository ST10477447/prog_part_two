using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prog_part_two.Models;
using System.Data.SqlClient;
using System.Security.Claims;

namespace prog_part_two.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Event : ControllerBase
    {
        private static List<Events> events = new();

        [HttpGet]
        public IActionResult GetAll() => Ok(events);

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var ev = events.FirstOrDefault(e => e.EventId == id);
            return ev is not null ? Ok(ev) : NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateById(int id, Events updated)
        {
            var ev = events.FirstOrDefault(e => e.EventId == id);
            if (ev is null) return NotFound();

            ev.EventName = updated.EventName;
            ev.EventType = updated.EventType;
            ev.Distance = updated.Distance;
            ev.EventDate = updated.EventDate;
            ev.EventLocation = updated.EventLocation;
            ev.EventDescription = updated.EventDescription;
            ev.OrganiserId = updated.OrganiserId;
            ev.CategoryId = updated.CategoryId;
            

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            events.RemoveAll(e => e.EventId == id);
            return NoContent();
        }

        // Only organisers can create events
        [HttpPost]
        public IActionResult CreateEvent(string EventName, string EventType, string Distance,
              string EventDate, string EventLocation, string EventDescription,
              int OrganiserId, int CategoryId)
        {
            



            try
            {
                string connectionString = @"Data source=(localdb)\api_part_1;database=prog_part_ll;";
                using SqlConnection connect = new SqlConnection(connectionString);

                connect.Open();

                // 1. Verify that the user's role is "Organiser"
                string roleQuery = "SELECT Role FROM Organiser WHERE OrganiserId = @OrganiserId";
                using SqlCommand roleCmd = new SqlCommand(roleQuery, connect);
                roleCmd.Parameters.AddWithValue("@OrganiserId", OrganiserId);

                object roleResult = roleCmd.ExecuteScalar();

                if (roleResult == null || roleResult.ToString() != "Organiser")
                {
                    return Unauthorized("Access denied. Only users with the 'Organiser' role can create an event.");
                }



                string query = @"INSERT INTO Events (EventName, EventType, Distance, EventDate, EventLocation, EventDescription, OrganiserId, CategoryId)
                         VALUES (@EventName, @EventType, @Distance, @EventDate, @EventLocation, @EventDescription, @OrganiserId, @CategoryId)";

                using SqlCommand cmd = new SqlCommand(query, connect);

                cmd.Parameters.AddWithValue("@EventName", EventName);
                cmd.Parameters.AddWithValue("@EventType", EventType);
                cmd.Parameters.AddWithValue("@Distance", Distance);
                cmd.Parameters.AddWithValue("@EventDate", EventDate);
                cmd.Parameters.AddWithValue("@EventLocation", EventLocation);
                cmd.Parameters.AddWithValue("@EventDescription", EventDescription);
                cmd.Parameters.AddWithValue("@OrganiserId", OrganiserId);
                cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
                

                cmd.ExecuteNonQuery();

                return Ok("Event saved successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
    }

