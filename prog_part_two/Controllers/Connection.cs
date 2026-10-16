using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace prog_part_two.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Connection : ControllerBase
    {
        [HttpPost]
        public string db_connection()
        {
            string message = "";

            try
            {
                string connection_string = @"Data source=(localdb)\api_part_1;database=prog_part_ll;";

                SqlConnection connect = new SqlConnection(connection_string);

                connect.Open();

                message = "Connected successfully";

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
