using EMS.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.API.Services
{
    public interface IEmployeeRepository
    {
        // We expose IQueryable here so the Service layer can apply Filters/Sorts/Pagination
        // BEFORE executing the SQL query via ToListAsync().
        IQueryable<Employee> GetAllAsQueryable();

        Task<Employee> GetByIdAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);

        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(Employee employee);
    }
}