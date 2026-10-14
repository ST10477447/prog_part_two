using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prog_part_two.Models;
using prog_part_two.Models;
using System.Data.SqlClient;

namespace prog_part_two.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Results : ControllerBase
    {
        public static List<Result> results = new()
        {
        };


    }
}
