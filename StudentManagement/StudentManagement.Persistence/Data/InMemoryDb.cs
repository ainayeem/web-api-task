using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Domain;
using StudentManagement.Persistence.Interfaces;

namespace StudentManagement.Persistence.Data;

public class InMemoryDb : DbContext, IApplicationDbContext
{
    public InMemoryDb(DbContextOptions<InMemoryDb> dbContextOptions) 
           : base(dbContextOptions)
    {

    }

    public DbSet<Student> Students { get; set; }
}
