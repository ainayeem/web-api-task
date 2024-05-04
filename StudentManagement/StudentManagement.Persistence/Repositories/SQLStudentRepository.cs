using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Data;
using StudentManagement.Persistence.Domain;
using StudentManagement.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Persistence.Repositories
{
    public class SQLStudentRepository : IStudentRepository
    {
        

        private readonly StudentManagementDbContext dbContext;

        public SQLStudentRepository(StudentManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        

        public async Task<Student> CreateAsync(Student student)
        {
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> DeleteAsync(Guid id)
        {
            var existingStudent = await dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);

            if (existingStudent == null)
            {
                return null;
            }

            dbContext.Students.Remove(existingStudent);
            await dbContext.SaveChangesAsync();
            return existingStudent;
        }

        public async Task<List<Student>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            //return await dbContext.Students.ToListAsync(); 
            var students = dbContext.Students.AsQueryable();

            // filtering
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    students = students.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // sorting
            if( string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    students = isAscending ? students.OrderBy(x => x.Name) : students.OrderByDescending(x => x.Name); 
                }
                else if (sortBy.Equals("Email", StringComparison.OrdinalIgnoreCase))
                {
                    students = isAscending ? students.OrderBy(x => x.Email) : students.OrderByDescending(x => x.Email);
                }
            }

            // pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await students.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Student?> UpdateAsync(Guid id, Student student)
        {
            var existingStudent = await dbContext.Students.FirstOrDefaultAsync(x=>x.Id == id);

            if (existingStudent == null)
            {
                return null;
            }

            
            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.EnrollmentDate = student.EnrollmentDate;

            await dbContext.SaveChangesAsync();
            return existingStudent;

        }
    }
}
