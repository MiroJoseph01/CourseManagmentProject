using AutoMapper;
using CourseManagment.BLL.DTOModels;
using CourseManagment.BLL.Infrastructure;
using CourseManagment.BLL.Services.Interfaces;
using CourseManagment.DAL.Entities.Gradebooks;
using CourseManagment.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    /// <summary>
     /// Class ProgressService
     /// implements interface IProgressService and methods from it
     /// </summary>
    public class ProgressService:IProgressService
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Constructor ProgressServices 
        /// is used to initialize Unit Of Work
        /// </summary>
        /// <param name="uow"></param>
        public ProgressService(IUnitOfWork uow)
        {
            Database = uow;
        }


        public IEnumerable<ProgressDTO> GetProgress()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Progress, ProgressDTO>()).CreateMapper();
            var progress = config.Map<List<ProgressDTO>>(Database.Progress.GetAll());
            return progress;
        }

        public ProgressDTO GetProgress(int? id)
        {
            if (id == null)
                throw new ValidationException("Can't find Progress id", "");

            Progress pr = Database.Progress.Get(id.Value);
            if (pr == null)
                throw new ValidationException("This Progress wasn't found", "");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Progress, ProgressDTO>()).CreateMapper();

            return mapper.Map<ProgressDTO>(Database.Progress.Get(id.Value));
        }

        /// <summary>
        /// Method GetProgressByStudentIdAndCourseId 
        /// returns ProgressDTO of current Student and needed Course 
        /// </summary>
        /// <param name="studentId">Student Id</param>
        /// <param name="courseId">Course Id</param>
        /// <returns></returns>
        public ProgressDTO GetProgressByStudentIdAndCourseId(int? studentId, int? courseId )
        {
            if (studentId == null || courseId == null)
                throw new ValidationException("Cant find the values of parameters", "");
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Progress, ProgressDTO>()).CreateMapper();
            var progress = config.Map<List<ProgressDTO>>(Database.Progress.GetAll()
                .Where(x=>x.Student.StudentId==studentId&&x.Course.CourseId==courseId)).FirstOrDefault();
            return progress;
        }
        /// <summary>
        /// Method Rate 
        /// is used to add mark for current Student and needed Course
        /// </summary>
        /// <param name="studentId">Student Id</param>
        /// <param name="courseId">Course Id</param>
        /// <param name="Mark">Student Mark</param>
        public void Rate(int? studentId, int? courseId, int Mark)
        {
            var course = Database.Courses.Get(courseId.Value);
            var student = Database.Students.Get(studentId.Value);
            Database.Progress.Create(new Progress
            {
                Course = course,
                Student = student,
                GradeBookMark = Mark
            });
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
