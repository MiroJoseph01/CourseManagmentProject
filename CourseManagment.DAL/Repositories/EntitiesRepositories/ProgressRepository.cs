using CourseManagment.DAL.Entities.Gradebooks;
using CourseManagment.DAL.Repositories.RepositoriesInterfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories.EntitiesRepositories
{
    /// <summary>
    /// Class ProgressRepository
    /// implements interface IProgressRepository
    /// </summary>
    public class ProgressRepository : IProgressRepository
    {

        private CoursesContext db;

        /// <summary>
        /// Constructor CourseRepository
        /// initializes private field "db"
        /// </summary>
        /// <param name="context"></param>
        public ProgressRepository(CoursesContext context)
        {
            this.db = context;
        }

        public IEnumerable<Progress> GetAll()
        {
            return db.Progress;
        }

        public Progress Get(int id)
        {
            return db.Progress.Find(id);
        }

        public void Create(Progress progress)
        {
            db.Progress.Add(progress);
        }

        public void Update(Progress progress)
        {
            db.Entry(progress).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Progress progress = db.Progress.Find(id);
            if (progress != null)
                db.Progress.Remove(progress);
        }

        public IEnumerable<Progress> GetByStudentId(int id)
        {
            return db.Progress.Where(x => x.Student.StudentId == id);
        }

        public IEnumerable<Progress> GetByCourseId(int id)
        {
            return db.Progress.Where(x => x.Course.CourseId == id);
        }

        public void DeleteByStudentId(int id)
        {
            var del = db.Progress.Where(x => x.Student.StudentId == id);
            foreach (var i in del)
            {
                db.Progress.Remove(i);
            }
        }

        public void DeleteByCourseId(int id)
        {
            var del = db.Progress.Where(x => x.Course.CourseId == id);
            foreach (var i in del)
            {
                db.Progress.Remove(i);
            }

        }


    }
}
