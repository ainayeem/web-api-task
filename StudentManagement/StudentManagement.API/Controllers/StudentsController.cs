using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Persistence.Data;
using StudentManagement.Persistence.Domain;
using StudentManagement.Persistence.Repositories;
using StudentManagement.Service.DTOs;
using System.Text.Json;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository SQLStudentRepository;
        private readonly ILogger<StudentsController> logger;
        

        public StudentsController(IStudentRepository SQLStudentRepository, ILogger<StudentsController> logger)
        {
            this.SQLStudentRepository = SQLStudentRepository;
            this.logger = logger;
            
        }

        // GET all students
        // GET: /api/students?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        [Authorize(Roles = "Reader")]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration =20)]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber ,
            [FromQuery] int pageSize)
        {
            logger.LogInformation("=======Get all students started=======");

            // get data from db- domain model
            var studentsDomain = await SQLStudentRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            // map domain modetl to DTOs
            var studentsDto = new List<StudentDto>();
            foreach (var studentDomain in studentsDomain)
            {
                studentsDto.Add(new StudentDto()
                {
                    Id = studentDomain.Id,
                    Name = studentDomain.Name,
                    Email = studentDomain.Email,
                    EnrollmentDate = studentDomain.EnrollmentDate,
                });
            }

            logger.LogInformation($"=======Get all students finished=======\n {JsonSerializer.Serialize(studentsDto)}");

            // return DTOs
            return Ok(studentsDto);
        }

        // GET single student by id
        // GET: /api/students/id
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // get student domain model from db
            var studentDomain = await SQLStudentRepository.GetByIdAsync(id);

            if(studentDomain == null)
            {
                logger.LogError("student not found with given id");

                return NotFound();
            }

            // map student domain to student dto
            var studentDto = new StudentDto()
            {
                Id = studentDomain.Id,
                Name = studentDomain.Name,
                Email = studentDomain.Email,
                EnrollmentDate = studentDomain.EnrollmentDate
            };

            // return dto back to client
            return Ok(studentDto);
        }

        // POST to create new student
        // POST: /api/students
        [HttpPost]
        [Authorize(Roles = "Writer")]  
        public async Task<IActionResult> Create([FromBody] AddStudentRequestDto addStudentRequestDto)
        {
            //map DTO to domain model
            var studentDomainModel = new Student
            {
                Name = addStudentRequestDto.Name,
                Email = addStudentRequestDto.Email,
                EnrollmentDate = addStudentRequestDto.EnrollmentDate,
            };

            // use domain model to create student
            studentDomainModel = await SQLStudentRepository.CreateAsync(studentDomainModel);

            // map domain model back to dto
            var studentDto = new StudentDto
            {
                Name = studentDomainModel.Name,
                Email = studentDomainModel.Email,
                EnrollmentDate = studentDomainModel.EnrollmentDate,
            };

            return CreatedAtAction(nameof(GetById), new { id = studentDto.Id }, studentDto);
        }

        // UPDATE student
        // PUT: /api/students/id
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStudentRequestDto updateStudentRequestDto)
        {
            // map dto to domain model
            var studentDomainModel = new Student
            {
                Name = updateStudentRequestDto.Name,
                Email = updateStudentRequestDto.Email,
                EnrollmentDate = updateStudentRequestDto.EnrollmentDate,
            };


            // update student
            studentDomainModel = await SQLStudentRepository.UpdateAsync(id, studentDomainModel);

            if(studentDomainModel == null)
            {
                return NotFound();
            }

            

            // convert domain model to dto
            var studentDto = new StudentDto
            {
                Name = studentDomainModel.Name,
                Email = studentDomainModel.Email,
                EnrollmentDate = studentDomainModel.EnrollmentDate,
            };

            return Ok(studentDto);
        }

        // DELETE student
        // DELETE: /api/students/id
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var studentDomainModel = await SQLStudentRepository.DeleteAsync(id);

            if( studentDomainModel == null)
            {
                return NotFound();
            }

            

            // return deleted student back
            // map domain model to dto
            var studentDto = new StudentDto
            {
                Name = studentDomainModel.Name,
                Email = studentDomainModel.Email,
                EnrollmentDate = studentDomainModel.EnrollmentDate,
            };

            return Ok(studentDto);
        }

    }
}
