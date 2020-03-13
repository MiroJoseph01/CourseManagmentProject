using AutoMapper;
using CourseManagment.BLL.DTOModels;
using CourseManagment.BLL.Infrastructure;
using CourseManagment.BLL.Services.Interfaces;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services
{
    /// <summary>
    /// Class DepartmentService
    /// implements interface IDepartmentService and methods from it
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Constructor DepartmentService 
        /// is used to initialize Unit Of Work
        /// </summary>
        /// <param name="uow"></param>
        public DepartmentService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public IEnumerable<DepartmentDTO> GetDepartments()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Department, DepartmentDTO>())
                .CreateMapper();
            var courses = config.Map<List<DepartmentDTO>>(Database.Departments.GetAll());
            return courses;
        }

        public DepartmentDTO GetDepartment(int? id)
        {
            if (id == null)
                throw new ValidationException("Can't find Department's id", "");

            Department course = Database.Departments.Get(id.Value);
            if (course == null)
                throw new ValidationException("This Department wasn't found", "");

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Department, DepartmentDTO>())
                .CreateMapper();
         

            return config.Map<DepartmentDTO>(Database.Departments.Get(id.Value));
        }

        public void CreateDepartment(DepartmentDTO departmentDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DepartmentDTO, Department>())
               .CreateMapper();
            Database.Departments.Create(config.Map<Department>(departmentDto));
            Database.Save();
        }

        public void DeleteDepartment(int id)
        {
            Database.Departments.Delete(id);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    
    }
}
