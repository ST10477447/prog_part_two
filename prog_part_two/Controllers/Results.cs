using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prog_part_two.Models;
using System.Data.SqlClient;

namespace prog_part_two.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Results : ControllerBase
    {

        // 1. Submit a result (For Organisers)
        [HttpPost("submit_result")]
        public IActionResult SubmitResult(int organiserId, string participantName, string eventName,string categoryName, string scoreOrOutcome)
        {
            // Simple validation rule
            if (string.IsNullOrEmpty(scoreOrOutcome))
            {
                return BadRequest(new { message = "Score or outcome cannot be empty." });
            }

            string connectionString = @"Data source=(localdb)\api_part_1;database=prog_part_ll;";
            string query = "INSERT INTO Results (OrganiserId, ParticipantName, EventName, CategoryName, ScoreOrOutcome) VALUES ( @OrganiserId, @ParticipantName, @EventName ,@CategoryName, @ScoreorOutcome)";

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@OrganiserId", organiserId);
                        cmd.Parameters.AddWithValue("@ParticipantName", categoryName);
                        cmd.Parameters.AddWithValue("@EventName", eventName);
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        cmd.Parameters.AddWithValue("@ScoreOrOutcome", scoreOrOutcome);

                        connect.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Participant result saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // 2. Get results for a specific participant (For Participants to view their score)
        [HttpGet("participant/{participantId}/results")]
        public IActionResult GetParticipantResults(int participantId)
        {
            string connectionString = @"Data source=(localdb)\api_part_1;database=prog_part_2;";
            string query = "SELECT CategoryName, ScoreOrOutcome, DateRecorded FROM Results WHERE ParticipantId = @ParticipantId";

            var resultsList = new List<object>();

            try
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@ParticipantId", participantId);

                        connect.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                resultsList.Add(new
                                {
                                    Category = reader.GetString(0),
                                    Result = reader.GetString(1),
                                    Date = reader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm")
                                });
                            }
                        }
                    }
                }

                if (resultsList.Count == 0) return NotFound(new { message = "No results found for this participant." });

                return Ok(resultsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
