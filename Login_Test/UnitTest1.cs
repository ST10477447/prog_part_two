using Microsoft.AspNetCore.Mvc;
using prog_part_two.Controllers;
namespace Login_Test
{
    public class UnitTest1
    {
        [Fact]
        public void Login_InvalidInput_ReturnsErrorMessage()
        {
            // 1. Arrange (Setting up your variables exactly like your example)
            Login test_correct = new Login();

            string test_name = "Lesedi";
            string test_email = "lesedi@yahoo.com"; // Invalid because it's not @gmail.com
            string test_password = "short";        // Invalid because it's under 8 characters

            // 2. Act (Calling your controller method)
            IActionResult actionResult = test_correct.Logins(test_name, test_email, test_password);

            // 3. Assert (Extracting the result to compare strings)
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult);

            // Serialize the anonymous object { message = "..." } to a string to match your logic
            string results = Newtonsoft.Json.JsonConvert.SerializeObject(unauthorizedResult.Value);
            string expected_results = "Invalid Organiser name, email, or password.";

            Assert.Contains(expected_results, results);
        }

        [Fact]
        public void ParticipantLogin_InvalidInput_ReturnsErrorMessage()
        {
            // 1. Arrange
            Login test_correct = new Login();

            string test_name = "Lesedi";
            string test_email = "lesedi@outlook.com"; // Invalid because it doesn't contain gmail.com
            string test_password = "123";              // Invalid because length is <= 8

            // 2. Act
            IActionResult actionResult = test_correct.ParticipantLogin(test_name, test_email, test_password);

            // 3. Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult);

            string results = Newtonsoft.Json.JsonConvert.SerializeObject(unauthorizedResult.Value);
            string expected_results = "Invalid participant name or email or password.";

            Assert.Contains(expected_results, results);
        }
    }
}