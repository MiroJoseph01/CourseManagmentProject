using CourseManagment.DAL.Entities.Lecturers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories
{
    /// <summary>
    /// Class LecturerRepository
    /// implements interface IRepository for class Lecturer
    /// </summary>
    public class LecturerRepository : IRepository<Lecturer>
    {
        private CoursesContext db;

        /// <summary>
        /// Constructor LecturerRepository
        /// initializes private field "db"
        /// </summary>
        /// <param name="context"></param>
        public LecturerRepository(CoursesContext context)
        {
            this.db = context;
        }

        public IEnumerable<Lecturer> GetAll()
        {
            return db.Lecturers;
        }

        public Lecturer Get(int id)
        {
            return db.Lecturers.Find(id);
        }

        public void Create(Lecturer lecturer)
        {
            db.Lecturers.Add(lecturer);
        }

        public void Update(Lecturer lecturer)
        {
            var lct = db.Lecturers.Where(x => x.LecturerId == lecturer.LecturerId).FirstOrDefault();
            lct.FirstName = lecturer.FirstName;
            lct.SecondName = lecturer.SecondName;
            lct.Information = lecturer.Information;
            lct.LecturerEmail = lecturer.LecturerEmail;
            lct.Image = lecturer.Image;
            lct.ImageName = lecturer.ImageName;
            lct.Courses = lecturer.Courses;
            lct.Department = lecturer.Department;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Lecturer lecturer = db.Lecturers.Find(id);
            if (lecturer != null)
                db.Lecturers.Remove(lecturer);
        }
    }
}
