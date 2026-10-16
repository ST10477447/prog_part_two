using Microsoft.AspNetCore.Mvc;
using prog_part_two.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_Test
{
    public class ApplicationControllers_test
    {
        [Fact]
        public void store_organiserInfo_test()
        {
            // Arrange (Using your preferred variable layout)
            Organiser test_correct = new Organiser();

            string test_name = "Thabo";
            string test_surname = "Khumalo";
            string test_email = "thabo@yahoo.com"; // Invalid: not @gmail.com
            int test_number = 0721234567;
            string test_password = "123";          // Invalid: <= 8 characters

            // Act
            string results = test_correct.store_organiserInfo(
                test_name, test_surname, test_email, test_number, test_password
            );
            string expected_results = "Invalid organiser email or password.";

            // Assert
            Assert.Equal(expected_results, results);
        }

        [Fact]
        public void StoreParticipantInfo_DatabaseConnectionFails_ReturnsErrorMessage()
        {
            // Arrange
            Organiser test_correct = new Organiser();

            string test_name = "Sipho";
            string test_email = "sipho@gmail.com";
            int test_organiserId = 1;

            // Act
            IActionResult actionResult = test_correct.StoreParticipantInfo(test_name, test_email, test_organiserId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(500, objectResult.StatusCode);

            // Turn the anonymous exception object into a string to inspect the database crash text
            string results = Newtonsoft.Json.JsonConvert.SerializeObject(objectResult.Value);
            Assert.Contains("error", results);
        }


        // ==========================================
        // PARTICIPANT CONTROLLER TESTS
        // ==========================================

        [Fact]
        public void store_participantInfo_test()
        {
            // Arrange (Using your preferred variable layout)
            Participant test_correct = new Participant();

            string test_name = "Lesedi";
            string test_surname = "Molefe";
            int test_age = 22;
            string test_gender = "Female";
            string test_email = "lesedi@yahoo.com"; // Invalid: not @gmail.com
            int test_number = 0831234567;
            string test_password = "123";          // Invalid: <= 8 characters

            // Act
            string results = test_correct.store_participantInfo(
                test_name, test_surname, test_age, test_gender, test_email, test_number, test_password
            );
            string expected_results = "Invalid participant email or password.";

            // Assert
            Assert.Equal(expected_results, results);
        }
    }
}
