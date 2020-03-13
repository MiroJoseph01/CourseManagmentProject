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
    /// Class DepartmentRepository
    /// implements interface IRepository for class Department
    /// </summary>
    public class DepartmentRepository : IRepository<Department>
    {
        private CoursesContext db;

        /// <summary>
        /// Constructor DepartmentRepository
        /// initializes private field "db"
        /// </summary>
        /// <param name="context"></param>
        public DepartmentRepository(CoursesContext context)
        {
            this.db = context;
        }

        public IEnumerable<Department> GetAll()
        {
            return db.Departments;
        }

        public Department Get(int id)
        {
            return db.Departments.Find(id);
        }

        public void Create(Department department)
        {
            db.Departments.Add(department);
        }

        public void Update(Department department)
        {
            db.Entry(department).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Department department = db.Departments.Find(id);
            if (department != null)
                db.Departments.Remove(department);
        }
    }
}
