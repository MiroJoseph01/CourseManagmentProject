using AutoMapper;
using CourseManagment.BLL.DTOModels;
using CourseManagment.BLL.Infrastructure;
using CourseManagment.BLL.Services.Interfaces;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    /// <summary>
    /// Class LecturerService
    /// implements interface ILecturerService and methods from it
    /// </summary>
    public class LecturerService : ILecturerService
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Constructor LecturerService
        /// is used to initialize Unit Of Work
        /// </summary>
        /// <param name="uow"></param>
        public LecturerService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<LecturerDTO> GetLecturers()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Lecturer, LecturerDTO>()).CreateMapper();
            var courses = config.Map<List<LecturerDTO>>(Database.Lecturers.GetAll());
            return courses;
        }

        public LecturerDTO GetLecturer(int? id)
        {
            if (id == null)
                throw new ValidationException("Can't find Lecturer's id", "");

            Lecturer course = Database.Lecturers.Get(id.Value);
            if (course == null)
                throw new ValidationException("This Lecturer wasn't found", "");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Lecturer, LecturerDTO>()).CreateMapper();

            return mapper.Map<LecturerDTO>(Database.Lecturers.Get(id.Value));
        }


       
        public void AddLecturer(LecturerDTO lecturerDTO, int depId)
        {
            var dep = Database.Departments.Get(depId);
            Lecturer lecturer = new Lecturer
            {
                FirstName = lecturerDTO.FirstName,
                SecondName = lecturerDTO.SecondName,
                Department = dep,
                LecturerEmail = lecturerDTO.LecturerEmail,
                Information = lecturerDTO.Information,
                ImageName = lecturerDTO.ImageName,
                Image = lecturerDTO.Image
            };
            Database.Lecturers.Create(lecturer);
            Database.Save();
        }

        /// <summary>
        /// Method EditLecturer 
        /// is used to edit Lecturer in database
        /// </summary>
        /// <param name="lecturerDTO"></param>
        /// <param name="depId">Department Id</param>
        public void EditLecturer(LecturerDTO lecturerDTO, int depId)
        {
            var dep = Database.Departments.Get(depId);
            Lecturer lecturer = new Lecturer
            {
                FirstName = lecturerDTO.FirstName,
                SecondName = lecturerDTO.SecondName,
                LecturerId = lecturerDTO.LecturerId,
                Department = dep,
                Information = lecturerDTO.Information,
                Image = lecturerDTO.Image,
                ImageName = lecturerDTO.ImageName,
                LecturerEmail = lecturerDTO.LecturerEmail,
                Courses = Database.Lecturers.Get(lecturerDTO.LecturerId).Courses
            };
            Database.Lecturers.Update(lecturer);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
