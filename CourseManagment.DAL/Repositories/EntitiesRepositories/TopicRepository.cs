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
    /// Class TopicRepository
    /// implements interface IRepository for class Topic
    /// </summary>
    public class TopicRepository : IRepository<Topic>
    {
        private CoursesContext db;

        /// <summary>
        /// Constructor TopicRepository
        /// initializes private field "db"
        /// </summary>
        /// <param name="context"></param>
        public TopicRepository(CoursesContext context)
        {
            this.db = context;
        }

        public IEnumerable<Topic> GetAll()
        {
            return db.Topics;
        }

        public Topic Get(int id)
        {
            return db.Topics.Find(id);
        }

        public void Create(Topic topic)
        {
            db.Topics.Add(topic);
        }

        public void Update(Topic topic)
        {
            db.Entry(topic).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Topic topic = db.Topics.Find(id);
            if (topic != null)
                db.Topics.Remove(topic);
        }
    }
}
