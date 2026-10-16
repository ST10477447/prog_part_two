using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prog_part_two.Controllers;

namespace Login_Test
{
    public class EnrollmentController_test
    {
        [Fact]
        public void EnrolToEvent_InvalidIds_ReturnsValidationMessage()
        {
            // 1. Arrange (Setting up your variables exactly like your example)
            Participant test_correct = new Participant();

            int test_participantId = 0; // Invalid ID
            int test_eventId = 5;
            string test_date = "2026-10-16";

            // 2. Act
            string results = test_correct.EnrolToEvent(test_participantId, test_eventId, test_date);
            string expected_results = "Invalid input data: ID parameters must be greater than zero.";

            // 3. Assert (Directly comparing strings exactly like your example)
            Assert.Equal(expected_results, results);
        }

        [Fact]
        public void EnrolToEvent_DatabaseMissingRecord_ReturnsErrorMessage()
        {
            // 1. Arrange
            Participant test_correct = new Participant();

            // Valid numbers to pass validation, but ID 99999 triggers your foreign key error
            int test_participantId = 1;
            int test_eventId = 99999;
            string test_date = "2026-10-16";

            // 2. Act
            string results = test_correct.EnrolToEvent(test_participantId, test_eventId, test_date);

            // 3. Assert
            // Since it goes to the catch block, we check that it accurately contains the SQL error message
            Assert.Contains("Enrollment processing failed:", results);
        }
    }
}
    
