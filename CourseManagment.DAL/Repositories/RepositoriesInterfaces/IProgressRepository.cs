using CourseManagment.DAL.Entities.Gradebooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories.RepositoriesInterfaces
{
    /// <summary>
    /// Declares functionality of Progress Repositories
    /// </summary>

    public interface IProgressRepository
    {
        IEnumerable<Progress> GetAll();
        Progress Get(int id);
        void Create(Progress item);
        void Update(Progress item);
        void Delete(int id);
        void DeleteByStudentId(int id);
        void DeleteByCourseId(int id);
        IEnumerable<Progress> GetByStudentId(int id);
        IEnumerable<Progress> GetByCourseId(int id);
    }
}
