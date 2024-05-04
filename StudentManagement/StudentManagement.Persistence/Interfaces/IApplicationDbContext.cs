using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Domain;

namespace StudentManagement.Persistence.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Student> Students { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
