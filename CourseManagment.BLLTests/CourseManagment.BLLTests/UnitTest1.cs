using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CourseManagment.BLL;
using CourseManagment.BLL.DTOModels;
using CourseManagment.BLL.Infrastructure;
using CourseManagment.DAL.Repositories;
using CourseManagment.DAL.Entities.Courses;
using CourseManagment.BLL.Services;
using CourseManagment.DAL.Repositories.RepositoriesInterfaces;
using Moq;
using System.Collections.Generic;
using CourseManagment.DAL.UnitOfWork;
using CourseManagment.DAL;
using System.Linq;
using CourseManagment.DAL.Entities.Lecturers;

namespace CourseManagment.BLLTests
{
    [TestClass]
    public class UnitTestBLL
    {
        private CoursesContext db;
        Mock<IUnitOfWork> UnitOfWork;
        CourseServices courseService;
        LecturerService lecturerService;
        DepartmentService departmentService;

        [TestInitialize]
        public void SetUp()
        {

            // Create a new mock of the repository
            UnitOfWork = new Mock<IUnitOfWork>();
            UnitOfWork.Setup(x => x.Departments.GetAll()).Returns(new List<Department> {new Department{
            DepartmentId=1,
            DepartmentName="TestDep"}
            });

            departmentService = new DepartmentService(UnitOfWork.Object);
            var dep = departmentService.GetDepartments().Where(x => x.DepartmentId == 1).First();

            UnitOfWork.Setup(x => x.Lecturers.GetAll()).Returns(new List<Lecturer> { new Lecturer {
            LecturerId=1,
            LecturerEmail="Lec@gmail.com",
            Department=new Department{
                DepartmentId =dep.DepartmentId,
                DepartmentName=dep.DepartmentName
            },
            FirstName="jack",
            SecondName="wolf",
            Information="Lorem ipsum doet dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. "
            } });
            lecturerService = new LecturerService(UnitOfWork.Object);
            var lec = lecturerService.GetLecturers().Where(x => x.LecturerId == 1).First();
            UnitOfWork.Setup(x => x.Courses.GetAll()).Returns(new List<Course>
                {
                new Course { CourseId=1,
                CourseName="Test",
                Description = "Lorem ipsum doet dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
                CourseStart=DateTime.ParseExact("12.10.2020", "dd.mm.yyyy", null),
                CourseEnd=DateTime.ParseExact("12.11.2020", "dd.mm.yyyy", null),
                ForTerm=1,
                Lecturer=new Lecturer
                {
                    LecturerId=lec.LecturerId,
                    Department=lec.Department,
                    FirstName=lec.FirstName,
                    Information=lec.Information,
                    LecturerEmail=lec.LecturerEmail,
                    SecondName=lec.SecondName
                }
                }
            });
            courseService = new CourseServices(UnitOfWork.Object);
        }



        [TestMethod]

        public void CourseDTO_GetCourses()
        {
            var course = courseService.GetCourses().ToList();
            Assert.AreEqual(1, course.Count);

        }

        [TestMethod]
        public void CourseDTO_GetLecturers()
        {
            var lecturer = lecturerService.GetLecturers().ToList();
            Assert.IsNotNull(lecturer);
        }

        [TestMethod]
        public void CourseDTO_GetDepartments()
        {
            var department =departmentService.GetDepartments().ToList();
            Assert.IsNotNull(department);
        }
    }
}
