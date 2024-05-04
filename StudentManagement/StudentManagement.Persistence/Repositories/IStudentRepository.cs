using StudentManagement.Persistence.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Persistence.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 100);
        Task<Student?> GetByIdAsync(Guid id);
        Task<Student> CreateAsync(Student student);
        Task<Student?> UpdateAsync(Guid id, Student student);
        Task<Student?> DeleteAsync(Guid id);
    }
}
