using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Service.DTOs
{
    public class AddStudentRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
