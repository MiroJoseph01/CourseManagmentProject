using AutoMapper;
using CourseManagment.BLL.DTOModels;
using CourseManagment.BLL.Infrastructure;
using CourseManagment.BLL.Services.Interfaces;
using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.Entities.Students;
using CourseManagment.DAL.Repositories;
using CourseManagment.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    /// <summary>
    /// Class CourseService
    /// implements interface ICourseService and methods from it
    /// </summary>
    public class CourseServices : ICourseService
    {
        IUnitOfWork Database { get; set; }


        /// <summary>
        /// Constructor CourseServices 
        /// is used to initialize Unit Of Work
        /// </summary>
        /// <param name="uow"></param>
        public CourseServices(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<CourseDTO> GetCourses()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>()
                .ForMember("NumberOfStudents", opt => opt.MapFrom(c => c.Students.Count)));
            var mapper = new Mapper(config);
            var courses = mapper.Map<List<CourseDTO>>(Database.Courses.GetAll());
            return courses;
        }

        public CourseDTO GetCourse(int? id)
        {
            if (id == null)
                throw new ValidationException("Can't find Course's id", "");

            Course course = Database.Courses.Get(id.Value);
            if (course == null)
                throw new ValidationException("This Course wasn't found", "");

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>()
                 .ForMember("NumberOfStudents", opt => opt.MapFrom(c => c.Students.Count)));
            var mapper = new Mapper(config);

            return mapper.Map<CourseDTO>(Database.Courses.Get(id.Value));
        }

        /// <summary>
        /// Method GetCoursesOfLecturer 
        /// returns Courses of certain Lecturer
        /// </summary>
        /// <param name="id">Lecturer Id</param>
        /// <returns></returns>
        public IEnumerable<CourseDTO> GetCoursesOfLecturer(int? id)
        {
            if (id == null)
                throw new ValidationException("Can't find Course's id", "");
            Lecturer lecturer = Database.Lecturers.Get(id.Value);
            if (lecturer == null)
                throw new ValidationException("This Course wasn't found", "");
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>());
            var mapper = new Mapper(config);
            return mapper.Map<List<CourseDTO>>(Database.Courses.GetAll().Where(x => x.Lecturer == lecturer));
        }


        /// <summary>
        /// Method GetCoursesByTopic 
        /// returns Courses of one Topic
        /// </summary>
        /// <param name="id">Topic Id</param>
        /// <returns></returns>
        public IEnumerable<CourseDTO> GetCoursesByTopic(int? id)
        {
            if (id == null)
                throw new ValidationException("Can't find Topic's id", "");
            Topic topic = Database.Topics.Get(id.Value);
            if (topic == null)
                throw new ValidationException("This Topic wasn't found", "");
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Course, CourseDTO>());
            var mapper = new Mapper(config);
            return mapper.Map<List<CourseDTO>>(Database.Courses.GetAll().Where(x => x.Topics.Contains(topic)));
        }

        /// <summary>
        /// Method AddCourse 
        /// is used to add a new Course to database
        /// </summary>
        /// <param name="courseDTO"></param>
        /// <param name="lecId">Lecturer Id</param>
        /// <param name="strtopics">Topics' string</param>
        public void AddCourse(CourseDTO courseDTO,int lecId, string strtopics)
        {
            string[] splited = strtopics.Split(',');
            var topics = Database.Topics.GetAll().ToList();
            List<Topic> res = new List<Topic>();
            foreach (var i in splited)
            {
                if (topics.Where(x => x.TopicName.ToLower() == i.ToLower()).FirstOrDefault() != null)
                {
                    res.Add(topics.Where(x => x.TopicName.ToLower() == i.ToLower()).FirstOrDefault());
                }
                else
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        Database.Topics.Create(new Topic { TopicName = i });
                        Database.Save();
                        var newtopics = Database.Topics.GetAll().ToList();
                        res.Add(newtopics.Where(x => x.TopicName.ToLower() == i.ToLower()).FirstOrDefault());
                    }
                    else
                        throw new ValidationException("Topic is empty", "");
                }

            }



            var lec = Database.Lecturers.Get(lecId);
            if (courseDTO.CourseStart < DateTime.Now
                || courseDTO.CourseStart > courseDTO.CourseEnd
                || courseDTO.CourseEnd < DateTime.Now)
                throw new ValidationException("Invalid data: Start data must be after now, End data must be bigger, then start one", "");
            Course course = new Course
            {
                CourseName = courseDTO.CourseName,
                Description = courseDTO.Description,
                CourseStart = courseDTO.CourseStart,
                CourseEnd = courseDTO.CourseEnd,
                ForTerm = courseDTO.ForTerm,
                IsDeleted = courseDTO.IsDeleted,
                Image = courseDTO.Image,
                ImageName = courseDTO.ImageName,
                Lecturer = lec,
                Topics = res,
            };
            Database.Courses.Create(course);
            Database.Save();
        }

        /// <summary>
        /// Method EditCourse 
        /// is used to edit Course in database
        /// </summary>
        /// <param name="courseDTO"></param>
        /// <param name="lecId">Lecturer Id</param>
        /// <param name="strtopics">Topics' string</param>
        public void EditCourse(CourseDTO courseDTO, int lecId, string strtopics)
        {
            string[] splited = strtopics.Split(',');
            var topics = Database.Topics.GetAll().ToList();
            List<Topic> res = new List<Topic>();
            foreach (var i in splited)
            {
                if (topics.Where(x => x.TopicName.ToLower() == i.ToLower()).FirstOrDefault() != null)
                {
                    res.Add(topics.Where(x => x.TopicName.ToLower() == i.ToLower()).FirstOrDefault());
                }
                else
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        Database.Topics.Create(new Topic { TopicName = i });
                        Database.Save();
                        var newtopics = Database.Topics.GetAll().ToList();
                        res.Add(newtopics.Where(x => x.TopicName.ToLower() == i.ToLower()).FirstOrDefault());
                    }
                    else
                        continue;
                }

            }



            var lec = Database.Lecturers.Get(lecId);
            if (courseDTO.CourseStart < DateTime.Now
                || courseDTO.CourseStart > courseDTO.CourseEnd
                || courseDTO.CourseEnd < DateTime.Now)
                throw new ValidationException("Invalid data: Start data must be after now, End data must be bigger, then start one", "");
            Course course = new Course
            {
                CourseId=courseDTO.CourseId,
                CourseName = courseDTO.CourseName,
                Description = courseDTO.Description,
                CourseStart = courseDTO.CourseStart,
                CourseEnd = courseDTO.CourseEnd,
                ForTerm = courseDTO.ForTerm,
                IsDeleted = courseDTO.IsDeleted,
                Image = courseDTO.Image,
                ImageName = courseDTO.ImageName,
                Lecturer = lec,
                Topics = res,
                Progresses=Database.Progress.GetByCourseId(courseDTO.CourseId).ToList(),
                Students=Database.Courses.Get(courseDTO.CourseId).Students
            };
            Database.Courses.Update(course);
           
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
