using EMS.API.Data;
using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EMS.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private AppDbContext _context;
        private Mock<IConfiguration> _mockConfig;
        private AuthService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AuthTestDb_" + System.Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Mock used for JWT settings
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("SuperSecretTestKey_Min32CharactersLong!");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("EMS.API");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("EMS.Client");
            _mockConfig.Setup(c => c["Jwt:ExpiryHours"]).Returns("8");

            _service = new AuthService(_context, _mockConfig.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsTokenString()
        {
            // Arrange
            var hashedPw = BCrypt.Net.BCrypt.HashPassword("password123");
            _context.AppUsers.Add(new AppUser { Username = "validuser", PasswordHash = hashedPw, Role = "Admin" });
            await _context.SaveChangesAsync();

            var request = new AuthRequestDto { Username = "validuser", Password = "password123" };

            // Act
            var result = await _service.LoginAsync(request);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Token, Is.Not.Null.And.Not.Empty); // GenerateToken returns non-empty string
        }

        [Test]
        public async Task LoginAsync_WrongPassword_ReturnsFailure()
        {
            // Arrange
            var hashedPw = BCrypt.Net.BCrypt.HashPassword("password123");
            _context.AppUsers.Add(new AppUser { Username = "testuser", PasswordHash = hashedPw, Role = "Viewer" });
            await _context.SaveChangesAsync();

            var request = new AuthRequestDto { Username = "testuser", Password = "WRONGPASSWORD" };

            // Act
            var result = await _service.LoginAsync(request);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Invalid credentials."));
            Assert.That(result.Token, Is.Null);
        }

        [Test]
        public async Task RegisterAsync_DuplicateUsername_ReturnsFailure()
        {
            // Arrange
            _context.AppUsers.Add(new AppUser { Username = "existinguser", PasswordHash = "hash", Role = "Viewer" });
            await _context.SaveChangesAsync();

            var request = new AuthRequestDto { Username = "existinguser", Password = "newpassword" };

            // Act
            var result = await _service.RegisterAsync(request);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Username already exists."));
        }
    }
}