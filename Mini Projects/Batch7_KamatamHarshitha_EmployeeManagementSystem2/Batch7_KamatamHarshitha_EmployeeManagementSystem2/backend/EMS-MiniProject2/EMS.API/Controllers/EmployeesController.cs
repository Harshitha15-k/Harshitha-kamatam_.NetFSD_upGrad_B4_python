using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Base level: Any authenticated user (Admin or Viewer)
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly IEmployeeRepository _repository;

        public EmployeesController(EmployeeService employeeService, IEmployeeRepository repository)
        {
            _employeeService = employeeService;
            _repository = repository;
        }

        // GET: api/employees (Server-side search, filter, sort, paginate)
        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeQueryParams queryParams)
        {
            var result = await _employeeService.GetPagedEmployeesAsync(queryParams);
            return Ok(result);
        }

        // GET: api/employees/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _employeeService.GetDashboardSummaryAsync();
            return Ok(result);
        }

        // GET: api/employees/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null) return NotFound();

            return Ok(_employeeService.MapToDto(employee));
        }

        // POST: api/employees
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admins can create
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _repository.EmailExistsAsync(request.Email))
            {
                return Conflict(new { Email = "Email already exists in the system." });
            }

            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Department = request.Department,
                Designation = request.Designation,
                Salary = request.Salary,
                JoinDate = request.JoinDate,
                Status = request.Status
            };

            await _repository.AddAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, _employeeService.MapToDto(employee));
        }

        // PUT: api/employees/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can update
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingEmployee = await _repository.GetByIdAsync(id);
            if (existingEmployee == null) return NotFound();

            if (await _repository.EmailExistsAsync(request.Email, id))
            {
                return Conflict(new { Email = "Email already exists for another employee." });
            }

            existingEmployee.FirstName = request.FirstName;
            existingEmployee.LastName = request.LastName;
            existingEmployee.Email = request.Email;
            existingEmployee.Phone = request.Phone;
            existingEmployee.Department = request.Department;
            existingEmployee.Designation = request.Designation;
            existingEmployee.Salary = request.Salary;
            existingEmployee.JoinDate = request.JoinDate;
            existingEmployee.Status = request.Status;
            existingEmployee.UpdatedAt = System.DateTime.UtcNow;

            await _repository.UpdateAsync(existingEmployee);
            return Ok(_employeeService.MapToDto(existingEmployee));
        }

        // DELETE: api/employees/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Only Admins can delete
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null) return NotFound();

            await _repository.DeleteAsync(employee);
            return Ok(new { message = "Employee deleted successfully." });
        }
    }
}