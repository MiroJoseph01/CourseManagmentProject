using AutoMapper;
using CourseManagment.BLL.DTOModels;
using CourseManagment.BLL.Infrastructure;
using CourseManagment.BLL.Services.Interfaces;
using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Students;
using CourseManagment.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    /// <summary>
    /// Class StudentService
    /// implements interface IStudentService and methods from it
    /// </summary>
    public class StudentService:IStudentService
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Constructor StudentServices 
        /// is used to initialize Unit Of Work
        /// </summary>
        /// <param name="uow"></param>
        public StudentService(IUnitOfWork uow)
        {
            Database = uow;
        }
       
        public IEnumerable<StudentDTO> GetStudents()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            var students = config.Map<List<StudentDTO>>(Database.Students.GetAll());
            return students;
        }

        public StudentDTO GetStudent(int? id)
        {
            if (id == null)
                throw new ValidationException("Can't find Student's id", "");

            Student student = Database.Students.Get(id.Value);
            if (student == null)
                throw new ValidationException("This Student wasn't found", "");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();

            return mapper.Map<StudentDTO>(Database.Students.Get(id.Value));
        }

        public void AddStudent(StudentDTO studentDTO)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StudentDTO, Student>()).CreateMapper();

            var student = mapper.Map<Student>(studentDTO);

            Database.Students.Create(student);
            Database.Save();
        }

        /// <summary>
        /// Method GetStudentByEmail 
        /// is used to get StudentDTo by Email
        /// </summary>
        /// <param name="email">Email value</param>
        /// <returns></returns>
        public StudentDTO GetStudentByEmail(string email)
        {
            if(string.IsNullOrEmpty(email))
                throw new ValidationException("Can't find Student's email", "");
            var student = Database.Students.GetAll().First(x => x.StudentEmail == email);
            if (student==null)
                throw new ValidationException("This Student wasn't found", "");
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Student, StudentDTO>()).CreateMapper();
            return mapper.Map<StudentDTO>(student);
        }

        public void AddCourse(int studentId, int courseId)
        {
            Database.Students.Get(studentId).Courses.Add(Database.Courses.Get(courseId));
            Database.Save();
        }

        /// <summary>
        /// Method BlockUnblock 
        /// changes value of property isBanned of current Student
        /// </summary>
        /// <param name="studentId">Student Id</param>
        /// <param name="pos">Value of property</param>
        public void BlockUnblock(int studentId, bool pos)
        {
            Database.Students.Get(studentId).IsBanned = pos;
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
