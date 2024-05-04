using StudentManagement.Persistence.Domain;
using StudentManagement.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Service.Mapper
{
    public class StudentMapperService
    {
        public Student MapUpdateStudentRequestDtoToUpdateStudent(UpdateStudentRequestDto updateStudentRequestDto)
        {
            return new Student
            {
                Name = updateStudentRequestDto.Name,
                Email = updateStudentRequestDto.Email,
                EnrollmentDate = updateStudentRequestDto.EnrollmentDate,
            };
        }

        public StudentDto MapStudentToStudentDto(Student student)
        {
            return new StudentDto
            {
                Name = student.Name,
                Email = student.Email,
                EnrollmentDate = student.EnrollmentDate,
            };
        }
    }
}
