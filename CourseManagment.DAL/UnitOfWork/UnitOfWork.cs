using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.Entities.Students;
using CourseManagment.DAL.Repositories;
using CourseManagment.DAL.Repositories.EntitiesRepositories;
using CourseManagment.DAL.Repositories.RepositoriesInterfaces;
using CourseManagment.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL
{
    /// <summary>
    /// Class EFUnitOfWork
    /// implements interface IUnitOfWork
    /// Class is used to unite the access to database
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        private CoursesContext context;
        private CourseRepository courseRepository;
        private DepartmentRepository departmentRepository;
        private LecturerRepository lecturerRepository;
        private ProgressRepository progressRepository;
        private StudentRepository studentRepository;
        private TopicRepository topicRepository;

        /// <summary>
        /// Constructor EFUnitOfWork
        /// initializes CourseContent to have access to Database
        /// </summary>
        /// <param name="connectionString"> connection string</param>
        public EFUnitOfWork(string connectionString)
        {
            context = new CoursesContext(connectionString);
        }

        public IRepository<Course> Courses
        {
            get
            {
                if (courseRepository == null)
                    courseRepository = new CourseRepository(context);
                return courseRepository;
            }
        }

        public IRepository<Department> Departments
        {
            get
            {
                if (departmentRepository == null)
                    departmentRepository = new DepartmentRepository(context);
                return departmentRepository;
            }
        }

        public IRepository<Lecturer> Lecturers
        {
            get
            {
                if (lecturerRepository == null)
                    lecturerRepository = new LecturerRepository(context);
                return lecturerRepository;
            }
        }

        public IRepository<Topic> Topics
        {
            get
            {
                if (topicRepository == null)
                    topicRepository = new TopicRepository(context);
                return topicRepository;
            }
        }

        public IRepository<Student> Students
        {
            get
            {
                if (studentRepository == null)
                    studentRepository = new StudentRepository(context);
                return studentRepository;
            }
        }

        public IProgressRepository Progress
        {
            get
            {
                if (progressRepository == null)
                    progressRepository = new ProgressRepository(context);
                return progressRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    
}
    
