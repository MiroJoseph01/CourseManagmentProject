using CourseManagment.DAL.Entities.Students;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories
{
    /// <summary>
    /// Class StudentRepository
    /// implements interface IRepository for class Student
    /// </summary>
    public class StudentRepository : IRepository<Student>
    {
        private CoursesContext db;

        /// <summary>
        /// Constructor StudentRepository
        /// initializes private field "db"
        /// </summary>
        /// <param name="context"></param>
        public StudentRepository(CoursesContext context)
        {
            this.db = context;
        }

        public IEnumerable<Student> GetAll()
        {
            return db.Students;
        }

        public Student Get(int id)
        {
            return db.Students.Find(id);
        }

        public void Create(Student student)
        {
            db.Students.Add(student);
        }

        public void Update(Student student)
        {
            db.Entry(student).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Student student = db.Students.Find(id);
            if (student != null)
                db.Students.Remove(student);
        }
    }
}
