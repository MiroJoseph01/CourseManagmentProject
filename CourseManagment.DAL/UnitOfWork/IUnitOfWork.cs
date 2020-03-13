using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Gradebooks;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.Entities.Students;
using CourseManagment.DAL.Repositories;
using CourseManagment.DAL.Repositories.RepositoriesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.UnitOfWork
{
    /// <summary>
    /// Interface IUnitOfWork
    /// declareswhat parts the class must consist of
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
            IRepository<Course> Courses { get; }
            IRepository<Topic> Topics { get; }
            IRepository<Department> Departments { get; }
            IRepository<Lecturer> Lecturers { get; }
            IRepository<Student> Students { get; }
            IProgressRepository Progress { get; }
            void Save();
    }
}
