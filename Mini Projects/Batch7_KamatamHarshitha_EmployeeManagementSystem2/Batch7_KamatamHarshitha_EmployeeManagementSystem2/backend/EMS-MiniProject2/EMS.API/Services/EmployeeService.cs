using EMS.API.DTOs;
using EMS.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.API.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<EmployeeResponseDto>> GetPagedEmployeesAsync(EmployeeQueryParams queryParams)
        {
            var query = _repository.GetAllAsQueryable();

            // 1. Filter
            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var searchTerm = queryParams.Search.ToLower();
                query = query.Where(e =>
                    (e.FirstName + " " + e.LastName).ToLower().Contains(searchTerm) ||
                    e.Email.ToLower().Contains(searchTerm));
            }
            if (!string.IsNullOrWhiteSpace(queryParams.Department))
            {
                query = query.Where(e => e.Department == queryParams.Department);
            }
            if (!string.IsNullOrWhiteSpace(queryParams.Status))
            {
                query = query.Where(e => e.Status == queryParams.Status);
            }

            // 2. Sort
            var isDesc = queryParams.SortDir?.ToLower() == "desc";
            query = queryParams.SortBy?.ToLower() switch
            {
                "salary" => isDesc ? query.OrderByDescending(e => e.Salary) : query.OrderBy(e => e.Salary),
                "joindate" => isDesc ? query.OrderByDescending(e => e.JoinDate) : query.OrderBy(e => e.JoinDate),
                _ => isDesc ? query.OrderByDescending(e => e.LastName).ThenByDescending(e => e.FirstName)
                            : query.OrderBy(e => e.LastName).ThenBy(e => e.FirstName) // Default sort by Name
            };

            // 3. Paginate
            var totalCount = await query.CountAsync();

            // Cap PageSize to 100 as per requirements
            var pageSize = Math.Min(queryParams.PageSize, 100);
            var skip = (queryParams.Page - 1) * pageSize;

            var employees = await query.Skip(skip).Take(pageSize).ToListAsync();

            // 4. Map to DTO
            var data = employees.Select(MapToDto);

            return new PagedResult<EmployeeResponseDto>
            {
                Data = data,
                TotalCount = totalCount,
                Page = queryParams.Page,
                PageSize = pageSize
            };
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var query = _repository.GetAllAsQueryable();
            var total = await query.CountAsync();

            var breakdown = await query
                .GroupBy(e => e.Department)
                .Select(g => new DepartmentCountDto
                {
                    Department = g.Key,
                    Count = g.Count(),
                    Percentage = total == 0 ? 0 : (int)Math.Round((double)g.Count() / total * 100)
                })
                .OrderBy(d => d.Department)
                .ToListAsync();

            var recent = await query
                .OrderByDescending(e => e.CreatedAt)
                .ThenByDescending(e => e.Id)
                .Take(5)
                .ToListAsync();

            return new DashboardSummaryDto
            {
                Total = total,
                Active = await query.CountAsync(e => e.Status == "Active"),
                Inactive = await query.CountAsync(e => e.Status == "Inactive"),
                Departments = breakdown.Count,
                DepartmentBreakdown = breakdown,
                RecentEmployees = recent.Select(MapToDto).ToList()
            };
        }

        // Helper mapper
        public EmployeeResponseDto MapToDto(Employee emp) => new EmployeeResponseDto
        {
            Id = emp.Id,
            FirstName = emp.FirstName,
            LastName = emp.LastName,
            Email = emp.Email,
            Phone = emp.Phone,
            Department = emp.Department,
            Designation = emp.Designation,
            Salary = emp.Salary,
            JoinDate = emp.JoinDate.ToString("yyyy-MM-dd"),
            Status = emp.Status
        };
    }
}