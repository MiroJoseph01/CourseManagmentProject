using CourseManagment.BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services.Interfaces
{
    /// <summary>
    /// Interface IProgressService
    /// declares functionality related to class ProgressDTO
    /// </summary>
    public interface IProgressService
    {
        IEnumerable<ProgressDTO> GetProgress();
        ProgressDTO GetProgress(int? id);
        ProgressDTO GetProgressByStudentIdAndCourseId(int? studentId, int? courseId);
        void Rate(int? studentId, int? courseId, int Mark);
        void Dispose();
    }
}
