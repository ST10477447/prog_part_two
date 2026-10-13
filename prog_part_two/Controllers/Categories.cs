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
            var Category = category.FirstOrDefault(category => category.Id == Id);
            return Category is not null ? Ok(Category) : NotFound();
        }

        [HttpPut]
        public IActionResult UpdateById(int Id, Category updated)
        {
            var Category = category.FirstOrDefault(category => category.Id == Id);
            if (Category is null) return NotFound();
            Category.CategoryName = updated.CategoryName;
            Category.CategoryDescription = updated.CategoryDescription;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int Id)
        {
            category.RemoveAll(category => category.Id == Id);
            return NoContent();
        }

        [HttpPost]
        public string Category(int Id, string CategoryName, string CategoryDescription)
        {
            string message = "";

            try
            {
                string connection_string = @"Data source=(localdb)\testingPoe;database=poe_prac1;";

                SqlConnection connect = new SqlConnection(connection_string);

                connect.Open();

                string query = @"insert into Events values ('" + Id + "','" + CategoryName + "', '" + CategoryDescription + "')";

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
    
