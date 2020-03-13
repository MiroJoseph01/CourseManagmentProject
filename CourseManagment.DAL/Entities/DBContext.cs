using CourseManagment.DAL.Entities;
using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Gradebooks;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.Entities.Students;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL
{

    /// <summary>
    /// Class CoursesContext
    /// Declares Database
    /// </summary>
    public class CoursesContext: DbContext
    {
        public CoursesContext(string connectionString)
                : base(connectionString)
        {  }

        static CoursesContext()
        {
            Database.SetInitializer<CoursesContext>(new DataInitializer());
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Progress> Progress { get; set; }


        /// <summary>
        /// Method OnModelCreating()
        /// Is ovverided to create connection Many-to-Many
        /// betweeen Courses and Students,
        /// between Courses and Topics
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Course>().HasMany(c => c.Students)
           .WithMany(s => s.Courses)
           .Map(t => t.MapLeftKey("CourseId")
           .MapRightKey("StudentId")
           .ToTable("Gradebooks"));

            modelBuilder.Entity<Course>().HasMany(c => c.Topics)
           .WithMany(s => s.Courses)
           .Map(t => t.MapLeftKey("CorseId")
           .MapRightKey("TopicId")
           .ToTable("CoursesAndTopics"));
        }
    }

}
