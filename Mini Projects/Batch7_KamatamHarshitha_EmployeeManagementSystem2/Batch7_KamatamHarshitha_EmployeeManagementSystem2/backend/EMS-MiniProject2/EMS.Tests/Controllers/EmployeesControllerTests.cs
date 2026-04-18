using EMS.API.Controllers;
using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace EMS.Tests.Controllers
{
    [TestFixture]
    public class EmployeesControllerTests
    {
        private Mock<IEmployeeRepository> _mockRepo;
        private EmployeeService _employeeService;
        private EmployeesController _controller;

        [SetUp]
        public void Setup()
        {
            // Mock the database access
            _mockRepo = new Mock<IEmployeeRepository>();

            // Pass the mocked repo into the real service
            _employeeService = new EmployeeService(_mockRepo.Object);

            // Pass both into the controller
            _controller = new EmployeesController(_employeeService, _mockRepo.Object);
        }

        [Test]
        public async Task GetEmployee_WhenEmployeeExists_ReturnsOkObjectResult()
        {
            // Arrange
            var fakeEmployee = new Employee { Id = 1, FirstName = "Test", LastName = "User", Email = "test@example.com" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(fakeEmployee);

            // Act
            var result = await _controller.GetEmployee(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<EmployeeResponseDto>());
            var dto = okResult.Value as EmployeeResponseDto;
            Assert.That(dto.FirstName, Is.EqualTo("Test"));
        }

        [Test]
        public async Task GetEmployee_WhenEmployeeDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.GetEmployee(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateEmployee_DuplicateEmail_ReturnsConflictObjectResult()
        {
            // Arrange
            var request = new EmployeeRequestDto { Email = "duplicate@test.com" };
            _mockRepo.Setup(repo => repo.EmailExistsAsync("duplicate@test.com", null)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateEmployee(request);

            // Assert
            Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
            // Ensure we never called AddAsync if the email was a duplicate
            _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Employee>()), Times.Never);
        }

        [Test]
        public async Task CreateEmployee_ValidNewEmployee_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var request = new EmployeeRequestDto
            {
                FirstName = "New",
                LastName = "User",
                Email = "new@test.com",
                Phone = "1234567890",
                Department = "IT",
                Designation = "Developer",
                Salary = 50000,
                JoinDate = DateTime.UtcNow,
                Status = "Active"
            };

            _mockRepo.Setup(repo => repo.EmailExistsAsync("new@test.com", null)).ReturnsAsync(false);
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateEmployee(request);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Test]
        public async Task DeleteEmployee_WhenEmployeeExists_ReturnsOkAndCallsDelete()
        {
            // Arrange
            var fakeEmployee = new Employee { Id = 1, FirstName = "Delete", LastName = "Me" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(fakeEmployee);
            _mockRepo.Setup(repo => repo.DeleteAsync(fakeEmployee)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteEmployee(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            _mockRepo.Verify(repo => repo.DeleteAsync(fakeEmployee), Times.Once);
        }
    }
}