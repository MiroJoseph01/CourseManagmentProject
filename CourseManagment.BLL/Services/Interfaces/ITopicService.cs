using CourseManagment.BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services.Interfaces
{
    /// <summary>
    /// Interface ITopicService
    /// declares functionality related to class TopicDTO
    /// </summary>
    public interface ITopicService
    {
        IEnumerable<TopicDTO> GetTopics();
        TopicDTO GetTopic(int? id);
        void Dispose();
    }
}
