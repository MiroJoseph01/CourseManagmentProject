using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.Entities.Students;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Entities
{
    /// <summary>
    /// Class DataInitializer
    /// It ovverrides Seed method to initialize database
    /// first time or after changes
    /// </summary>
    public class DataInitializer : DropCreateDatabaseIfModelChanges<CoursesContext>
    {
        
    
        protected override void Seed(CoursesContext db)
        {

            Department department = new Department { DepartmentName = "Computer Science" };
            db.Departments.Add(department);
            db.SaveChanges();

            Lecturer lecturer = new Lecturer { FirstName = "Mike", SecondName = "Miller",
                Information = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. ",
                Department = db.Departments.Where(x => x.DepartmentName == "Computer Science").FirstOrDefault(),
                LecturerEmail = "MrMikeMiller@gmail.com"
            };
            db.Lecturers.Add(lecturer);
            db.SaveChanges();

            Topic topic1 = new Topic { TopicName = "Technologies" };
            Topic topic2 = new Topic { TopicName = "AIKnowledge" };
            db.Topics.Add(topic1);
            db.Topics.Add(topic2);

            Student student = new Student { FirstName = "David", SecondName = "Brown", StudentEmail = "DavidBrown@gmail.com", StudentTerm = 1 };
            db.Students.Add(student);
            db.SaveChanges();

            Course course = new Course
            {
                    CourseName = "AI for first term",
                    Description= "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Lecturer = db.Lecturers.FirstOrDefault(x => x.LecturerEmail == "MrMikeMiller@gmail.com"),
                    Students = db.Students.Where(x => x.StudentEmail == "DavidBrown@gmail.com").ToList(),
                    Topics = db.Topics.Where(x => x.TopicName == "Technologies" || x.TopicName == "AIKnowledge").ToList(),
                    ForTerm = 1,
                    CourseStart = DateTime.ParseExact("12.10.2020", "dd.mm.yyyy", null),
                    CourseEnd = DateTime.ParseExact("12.12.2020", "dd.mm.yyyy", null),
            };
            db.Courses.Add(course);
            db.SaveChanges();

            List<Course> crs = new List<Course>();
            crs.Add(course);
            student.Courses =crs;
            db.SaveChanges();

            var temp = db.Students.ToList();
        }
    }

}
