using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Domain;
using StudentManagement.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Persistence.Data
{
    public class StudentManagementDbContext : DbContext, IApplicationDbContext
    {
        public StudentManagementDbContext(DbContextOptions<StudentManagementDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Student> Students { get; set; }
    }
}
