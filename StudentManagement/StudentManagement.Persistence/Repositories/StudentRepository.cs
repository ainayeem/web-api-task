using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Domain;
using StudentManagement.Persistence.Interfaces;

namespace StudentManagement.Persistence.Repositories;

public class StudentRepository
{
    private readonly IApplicationDbContext _applicationDbContext;

    public StudentRepository(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task AddAsync(Student student)
    {
        await _applicationDbContext.Students.AddAsync(student);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<IList<Student>> GetAllAsync()
    {
        return await _applicationDbContext.Students.ToListAsync();
    }
}
