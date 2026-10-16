using Microsoft.AspNetCore.Mvc;
using prog_part_two.Controllers;
using prog_part_two.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_Test
{
    public class EventControllerTests
    {
        
        private readonly Event _controller;

        public EventControllerTests()
        {
            // Initialise the controller before each test
            _controller = new Event();
        }

        // --- Tests for GET GetAll ---
        [Fact]
        public void GetAll_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        // --- Tests for GET GetById ---
        [Fact]
        public void GetById_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Act
            var result = _controller.GetById(999); // ID that doesn't exist

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // --- Tests for PUT UpdateById ---
        [Fact]
        public void UpdateById_ReturnsNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var updatedEvent = new Events { EventName = "New Name" };

            // Act
            var result = _controller.UpdateById(999, updatedEvent);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // --- Tests for DELETE DeleteById ---
        [Fact]
        public void DeleteById_ReturnsNoContentResult()
        {
            // Act
            var result = _controller.DeleteById(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // --- Tests for POST CreateEvent ---
        // Note: Because SqlConnection is hardcoded, hitting this will attempt to connect 
        // to your local database. If the database is offline or the ID is missing, it returns a BadRequest.
        [Fact]
        public void CreateEvent_ReturnsBadRequestOrUnauthorized_WhenDatabaseIsUnavailable()
        {
            // Act
            var result = _controller.CreateEvent(
                "Marathon", "Running", "21km", "2026-12-01",
                "Stadium", "Annual race", 1, 2
            );

            // Assert
            // It will either fail on connection (BadRequest) or role validation (Unauthorized)
            Assert.True(result is BadRequestObjectResult || result is UnauthorizedObjectResult);
        }
    }
}
