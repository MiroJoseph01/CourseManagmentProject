using AutoMapper;
using CourseManagment.BLL.DTOModels;
using CourseManagment.BLL.Infrastructure;
using CourseManagment.BLL.Services.Interfaces;
using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    /// <summary>
    /// Class TopicService
    /// implements interface ITopicService and methods from it
    /// </summary>
    public class TopicService : ITopicService
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Constructor TopicServices 
        /// is used to initialize Unit Of Work
        /// </summary>
        /// <param name="uow"></param>
        public TopicService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public IEnumerable<TopicDTO> GetTopics()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Topic, TopicDTO>()).CreateMapper();
            var courses = config.Map<List<TopicDTO>>(Database.Topics.GetAll());
            return courses;
        }

        public TopicDTO GetTopic(int? id)
        {
            if (id == null)
                throw new ValidationException("Can't find Topic's id", "");

            Topic topic = Database.Topics.Get(id.Value);
            if (topic == null)
                throw new ValidationException("This Topic wasn't found", "");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Topic, TopicDTO>()).CreateMapper();

            return mapper.Map<TopicDTO>(Database.Topics.Get(id.Value));
        }



        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
