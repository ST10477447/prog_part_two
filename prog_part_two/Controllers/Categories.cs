using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prog_part_two.Models;
using System.Data.SqlClient;

namespace prog_part_two.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Categories : ControllerBase
    {
        public static List<Category> category = new()
        {
        };

        [HttpGet]
        public IActionResult GetAll() => Ok(category);

        [HttpGet("{id}")]
        public IActionResult GetById(int Id)
        {
            var Category = category.FirstOrDefault(category => category.CategoryId == Id);
            return Category is not null ? Ok(Category) : NotFound();
        }

        [HttpPut]
        public IActionResult UpdateById(int Id, Category updated)
        {
            var Category = category.FirstOrDefault(category => category.CategoryId == Id);
            if (Category is null) return NotFound();
            Category.CategoryName = updated.CategoryName;
            Category.CategoryDescription = updated.CategoryDescription;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int Id)
        {
            category.RemoveAll(category => category.CategoryId == Id);
            return NoContent();
        }

        [HttpPost]
        public string Category(string CategoryName, string CategoryDescription)
        {
            string message = "";

            try
            {
                string connection_string = @"Data source=(localdb)\api_part_1;database=prog_part_ll;";

                SqlConnection connect = new SqlConnection(connection_string);

                connect.Open();

                string query = @"insert into Category values ('" + CategoryName + "', '" + CategoryDescription + "')";

                SqlCommand run_query = new SqlCommand(query, connect);

                run_query.ExecuteNonQuery();

                message = "Category saved successfully";

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
    
