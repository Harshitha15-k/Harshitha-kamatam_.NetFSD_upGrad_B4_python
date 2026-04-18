using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.API.DTOs
{
    // 1. Used for POST (Create) and PUT (Update)
    public class EmployeeRequestDto
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Must be a 10-digit number")] public string Phone { get; set; }
        [Required] public string Department { get; set; }
        [Required] public string Designation { get; set; }
        [Required, Range(1, double.MaxValue, ErrorMessage = "Salary must be positive")] public decimal Salary { get; set; }
        [Required] public DateTime JoinDate { get; set; }
        [Required] public string Status { get; set; }
    }

    // 2. Used for returning Employee data safely
    public class EmployeeResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public decimal Salary { get; set; }
        public string JoinDate { get; set; } // Formatted as YYYY-MM-DD for the frontend
        public string Status { get; set; }
    }

    // 3. Binds to GET query string parameters
    public class EmployeeQueryParams
    {
        public string? Search { get; set; }     // <-- Added ?
        public string? Department { get; set; } // <-- Added ?
        public string? Status { get; set; }     // <-- Added ?
        public string? SortBy { get; set; } = "name";
        public string? SortDir { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    // 4. The Paginated Response Envelope
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPrevPage => Page > 1;
    }

    // 5. Dashboard Summary
    public class DashboardSummaryDto
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Inactive { get; set; }
        public int Departments { get; set; }
        public List<DepartmentCountDto> DepartmentBreakdown { get; set; }
        public List<EmployeeResponseDto> RecentEmployees { get; set; }
    }

    public class DepartmentCountDto
    {
        public string Department { get; set; }
        public int Count { get; set; }
        public int Percentage { get; set; }
    }
}