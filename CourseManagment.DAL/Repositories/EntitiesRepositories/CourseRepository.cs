using CourseManagment.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories
{
    /// <summary>
    /// Class CourseRepository
    /// implements interface IRepository for class Course
    /// </summary>
    public class CourseRepository : IRepository<Course>
    {
        private CoursesContext db;

        /// <summary>
        /// Constructor CourseRepository
        /// initializes private field "db"
        /// </summary>
        /// <param name="context"></param>
        public CourseRepository(CoursesContext context)
        {
            this.db = context;
        }



        public IEnumerable<Course> GetAll()
        {
            return db.Courses.Include(x=>x.Topics);
        }

        public Course Get(int id)
        {
            return db.Courses.Find(id);
        }

        public void Create(Course course)
        {
            db.Courses.Add(course);
        }

        public void Update(Course course)
        {
            var crs = db.Courses.Where(x => x.CourseId == course.CourseId).FirstOrDefault();
            crs.Topics = course.Topics;
            crs.CourseName = course.CourseName;
            crs.Description = course.Description;
            crs.CourseStart = course.CourseStart;
            crs.CourseEnd = course.CourseEnd;
            crs.ForTerm = course.ForTerm;
            crs.Lecturer = course.Lecturer;
            crs.Progresses = course.Progresses;
            
            crs.Image = course.Image;
            crs.ImageName = course.ImageName;
            crs.IsDeleted = course.IsDeleted;
            crs.Students = course.Students;


            db.SaveChanges();
            
            
        }

        public void Delete(int id)
        {
            Course course = db.Courses.Find(id);
            if (course != null)
                db.Courses.Remove(course);
        }
    }
}
