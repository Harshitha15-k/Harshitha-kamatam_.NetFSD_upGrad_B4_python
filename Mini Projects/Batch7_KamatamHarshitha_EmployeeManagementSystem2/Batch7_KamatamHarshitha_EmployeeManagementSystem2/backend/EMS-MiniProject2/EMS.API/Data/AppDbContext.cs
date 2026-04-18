using EMS.API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EMS.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enforce Unique Indexes
            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<AppUser>().HasIndex(u => u.Username).IsUnique();

            // Seed Users (Pre-hashed passwords for 'admin123' and 'viewer123')
            // Using BCrypt work factor 11. 
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = 1, Username = "admin", PasswordHash = "$2a$11$caF4aQAwgV6XAzbNu2XgVeRBSWTALoPefESzxtO58ianwOVDZ9Lmu", Role = "Admin", CreatedAt = DateTime.UtcNow },
                new AppUser { Id = 2, Username = "viewer", PasswordHash = "$2a$11$gve9PCLry7K9q23jKtdcQ.hdc0UXII8BU8jwvcSFkLCrLrwZYucL6", Role = "Viewer", CreatedAt = DateTime.UtcNow }
            );

            // Seed Employees (Grabbing the first 3 from your previous data.js for brevity)
            // Note: In your actual project, add all 15 here.
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FirstName = "Harshitha", LastName = "Kamatam", Email = "harshitha.kamatam@gmail.com", Phone = "9876543289", Department = "Engineering", Designation = "Software Engineer", Salary = 950000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 2, FirstName = "Santhosh", LastName = "Kamatam", Email = "santhosh.kamatam@yahoo.com", Phone = "9123456790", Department = "Marketing", Designation = "Marketing Executive", Salary = 680000m, JoinDate = new DateTime(2020, 07, 01, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 3, FirstName = "Goutham", LastName = "Kamatam", Email = "goutham.kamatam@outlook.com", Phone = "9876512398", Department = "HR", Designation = "HR Executive", Salary = 620000m, JoinDate = new DateTime(2019, 11, 20, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 4,FirstName = "Rishitha",LastName = "Kola", Email = "rishitha.kola@gmail.com", Phone = "9989929004", Department = "Finance",Designation = "Financial Analyst",Salary = 990000m,JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc),Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 5, FirstName = "Sandeeep", LastName = "Kalisetty", Email = "sandy.kalisetty@gmail.com", Phone = "8765429876", Department = "Operations", Designation = "Supply chain", Salary = 950000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 6, FirstName = "Ranaveer", LastName = "Thota", Email = "rana.thota@gmail.com", Phone = "9872347658", Department = "Engineering", Designation = "Software Engineer", Salary = 850000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 7, FirstName = "Thaswin", LastName = "Miriyala", Email = "thaswin.miriyala@gmail.com", Phone = "9224463786", Department = "Marketing", Designation = "Marketing Executive", Salary = 950000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 8, FirstName = "Parthi", LastName = "Miriyala", Email = "parthi.miriyala@gmail.com", Phone = "9933452133", Department = "Hr", Designation = "Hr Executive", Salary = 750000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 9, FirstName = "Jaya", LastName = "MirKam", Email = "jaya.km@gmail.com", Phone = "8899554474", Department = "Finance", Designation = "Financial Analyst", Salary = 650000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 10, FirstName = "Veeri", LastName = "Korsipati", Email = "veeri.korsipati@gmail.com", Phone = "9888226608", Department = "Operations", Designation = "Supply chain", Salary = 850000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 11, FirstName = "Yaswanth", LastName = "Thota", Email = "yash.thota@gmail.com", Phone = "9887351437", Department = "Engineering", Designation = "Software Engineer", Salary = 450000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 12, FirstName = "Krishna", LastName = "Parlapalli", Email = "krishna.parlapalli@gmail.com", Phone = "9997755331", Department = "Hr", Designation = "HR Executive", Salary = 950000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 13, FirstName = "Ram", LastName = "Garre", Email = "ram.garre@gmail.com", Phone = "9886644645", Department = "Engineering", Designation = "Software Engineer", Salary = 880000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 14, FirstName = "Lucky", LastName = "Kamatam", Email = "lucky.kamatam@gmail.com", Phone = "9009865124", Department = "Engineering", Designation = "Software Engineer", Salary = 990000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Employee { Id = 15, FirstName = "Hanu", LastName = "Mara", Email = "hanu.mara@gmail.com", Phone = "9974672431", Department = "Hr", Designation = "HR Executive", Salary = 950000m, JoinDate = new DateTime(2021, 03, 15, 0, 0, 0, DateTimeKind.Utc), Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
        }
    }
}