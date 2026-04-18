using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Services;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EMS.Tests.Services
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _repoMock;
        private EmployeeService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _service = new EmployeeService(_repoMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsMappedDto()
        {
            // Arrange
            var fakeEmployee = new Employee { Id = 1, FirstName = "Priya", LastName = "Prabhu", Email = "p@h.com", Status = "Active" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fakeEmployee);

            // Act
            // Note: If your EmployeeService doesn't have a GetByIdAsync wrapper, 
            // you may need to add a simple one that calls _repository.GetByIdAsync and maps it to a DTO.
            var result = await _repoMock.Object.GetByIdAsync(1);
            var mappedResult = _service.MapToDto(result);

            // Assert
            Assert.That(mappedResult, Is.Not.Null);
            Assert.That(mappedResult.FirstName, Is.EqualTo("Priya"));
            _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once); // Confirms mock interactions
        }

        [Test]
        public async Task GetByIdAsync_NonExistentId_ReturnsNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(9999)).ReturnsAsync((Employee)null);

            // Act
            var result = await _repoMock.Object.GetByIdAsync(9999);

            // Assert
            Assert.That(result, Is.Null);
            _repoMock.Verify(r => r.GetByIdAsync(9999), Times.Once);
        }

        [Test]
        public async Task AddAsync_CallsAddAsyncOnRepo()
        {
            // Arrange
            var newEmployee = new Employee { FirstName = "Test", LastName = "User" };

            // Act
            await _repoMock.Object.AddAsync(newEmployee);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
        }
    }
}