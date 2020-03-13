using AutoMapper;
using CourseManagment.BLL.DTOModels;
using CourseManagment.BLL.Infrastructure;
using CourseManagment.BLL.Services;
using CourseManagment.BLL.Services.Interfaces;
using CourseManagment.DAL;
using CourseManagment.DAL.Entities.Courses;
using CourseManagment.Web.Filters;
using CourseManagment.Web.Infrastructure;
using CourseManagment.Web.Models.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManagment.Web.Controllers
{
    public class HomeController : Controller
    {
        ICourseService courseService;
        ILecturerService lecturerService;
        ITopicService topicService;
        
        public HomeController(ICourseService serv, ILecturerService lect, ITopicService top)
        {
            courseService = serv;
            lecturerService = lect;
            topicService = top;
        }

        [AllowAnonymous]
        public ActionResult Index(int page = 1, string key="")
        {
            int newkey;
            if (!string.IsNullOrEmpty(key))
                newkey = Convert.ToInt32(key);
            else
                newkey = 0;
            IEnumerable<CourseDTO> courseDtos = courseService.GetCourses();
            if (newkey == 2)
                courseDtos = courseService.GetCourses().OrderByDescending(x => x.CourseEnd - x.CourseStart);
 

            var conf = new MapperConfiguration(cfg => cfg.CreateMap<CourseDTO, CourseViewModel>()
                 .ForMember("CourseStart", c => c.MapFrom(z => z.CourseStart.ToString().Split(' ')[0]))
                 .ForMember("CourseEnd", c => c.MapFrom(z => z.CourseEnd.ToString().Split(' ')[0])));
            var mapper = new Mapper(conf);
            var courses = mapper.Map<IEnumerable<CourseDTO>, List<CourseViewModel>>(courseDtos);
            foreach (var i in courses)
            {
                if (i.Description.Length > 200)
                {
                    i.Description.Substring(0, 200);
                }
                
            }
            var removing = courses.Where(x => x.IsDeleted == true).ToList();
            foreach(var i in removing)
            {
                courses.Remove(i);
            }

            IOrderedEnumerable < CourseViewModel > orderedCourses;
            if (newkey == -1)
                orderedCourses = courses.OrderByDescending(x => x.CourseName);
            else if (newkey == 1)
                orderedCourses = courses.OrderBy(x => x.CourseName);
            else
                orderedCourses = courses.OrderByDescending(x => x.CourseStart);

            int pageSize = 2;
            IEnumerable<CourseViewModel> coursesPerPages = orderedCourses.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = courses.Count };
            if (pageInfo.TotalPages < page || page <= 0)
                return HttpNotFound();

            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Courses = coursesPerPages };
            return View(ivm);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }



        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [ExceptionLogger]
        public ActionResult FullCourse(string id)
        {
          
                if (string.IsNullOrEmpty(id))
                    throw new Exception("id doesn't exist");
                CourseDTO courseDto = courseService.GetCourse(Convert.ToInt32(id));
                CourseViewModel course = new CourseViewModel
                {
                    CourseId = courseDto.CourseId,
                    ForTerm = courseDto.ForTerm,
                    CourseName = courseDto.CourseName,
                    Description = courseDto.Description,
                    IsDeleted = courseDto.IsDeleted,
                    CourseEnd = courseDto.CourseEnd.ToString().Split(' ')[0],
                    CourseStart = courseDto.CourseStart.ToString().Split(' ')[0],
                    Image = courseDto.Image,
                    ImageName = courseDto.ImageName,
                    NumberOfStudents = courseDto.NumberOfStudents,
                    Topics = courseDto.Topics,
                    Lecturer = courseDto.Lecturer,
                    Students = courseDto.Students
                };
                return View(course);

        }

        [ExceptionLogger]
        public ActionResult AboutLecturer(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ValidationException("id doesn't exist", "");
            LecturerDTO lecturerDto = lecturerService.GetLecturer(Convert.ToInt32(id));
          
            
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<LecturerDTO, LecturerViewModel>()).CreateMapper();
            var lecturer = mapper.Map<LecturerDTO, LecturerViewModel>(lecturerDto);
            return View(lecturer);
            
        }



        [ExceptionLogger]
        public ActionResult Tags(string id, int page=1)
        {
            if (string.IsNullOrEmpty(id))
                throw new ValidationException("id doesn't exist", "");
            TopicDTO topic = topicService.GetTopic(Convert.ToInt32(id));
            if (topic == null)
                throw new ValidationException("topic doesn't exist", "");

            ViewBag.Topic = topic.TopicName;
            var courseDtos= courseService.GetCoursesByTopic(topic.TopicId).ToList();
            foreach (var i in courseDtos)
            {
                if (i.IsDeleted == true)
                {
                    courseDtos.Remove(i);
                }
            }
                var conf = new MapperConfiguration(cfg => cfg.CreateMap<CourseDTO, CourseViewModel>()
                      .ForMember("CourseStart", c => c.MapFrom(z => z.CourseStart.ToString().Split(' ')[0]))
                      .ForMember("CourseEnd", c => c.MapFrom(z => z.CourseEnd.ToString().Split(' ')[0])));
                var mapper = new Mapper(conf);
                var courses = mapper.Map<IEnumerable<CourseDTO>, List<CourseViewModel>>(courseDtos);

                var orderedCourses = courses.OrderByDescending(x => x.CourseStart);
                int pageSize = 2;
                IEnumerable<CourseViewModel> coursesPerPages = orderedCourses.Skip((page - 1) * pageSize).Take(pageSize);
                PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = courses.Count };
                if (pageInfo.TotalPages < page || page <= 0)
                    return HttpNotFound();

                IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Courses = coursesPerPages };

                return View(ivm);
           
        }


        [ExceptionLogger]
        public PartialViewResult Filter(int page = 1, string key="0")
        {
            int newkey;
            if (!string.IsNullOrEmpty(key))
                newkey = Convert.ToInt32(key);
            else
                newkey = 0;
            IEnumerable<CourseDTO> courseDtos = courseService.GetCourses();
            var conf = new MapperConfiguration(cfg => cfg.CreateMap<CourseDTO, CourseViewModel>()
                 .ForMember("CourseStart", c => c.MapFrom(z => z.CourseStart.ToString().Split(' ')[0]))
                 .ForMember("CourseEnd", c => c.MapFrom(z => z.CourseEnd.ToString().Split(' ')[0])));
            var mapper = new Mapper(conf);
            var courses = mapper.Map<IEnumerable<CourseDTO>, List<CourseViewModel>>(courseDtos);
            foreach (var i in courses)
            {
                if (i.Description.Length > 200)
                {
                    i.Description.Substring(0, 200);
                }
                if (i.IsDeleted == true)
                {
                    courses.Remove(i);
                }
            }
            IOrderedEnumerable<CourseViewModel> orderedCourses;
            if (newkey == -1)
                orderedCourses = courses.OrderByDescending(x => x.CourseName);
            else if(newkey == 1)
                orderedCourses = courses.OrderBy(x => x.CourseName); 
            else
                orderedCourses = courses.OrderByDescending(x => x.CourseStart);

            int pageSize = 2;
            IEnumerable<CourseViewModel> coursesPerPages = orderedCourses.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = courses.Count };

            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Courses = coursesPerPages };
            return PartialView(ivm);
        }

        protected override void Dispose(bool disposing)
        {
            courseService.Dispose();
            lecturerService.Dispose();
            topicService.Dispose();

            base.Dispose(disposing);
        }
    }

   
}