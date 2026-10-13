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

    }
}
