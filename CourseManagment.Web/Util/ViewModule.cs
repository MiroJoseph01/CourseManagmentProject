using CourseManagment.BLL.Services;
using CourseManagment.BLL.Services.Interfaces;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Util
{
    /// <summary>
    /// Class ViewModule 
    /// extends class NinjectModule and overrides method Load() 
    /// to bind services
    /// </summary>
    public class ViewModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICourseService>().To<CourseServices>();
            Bind<ILecturerService>().To<LecturerService>();
            Bind<IDepartmentService>().To<DepartmentService>();
            Bind<ITopicService>().To<TopicService>();
            Bind<IStudentService>().To<StudentService>();
            Bind<IProgressService>().To<ProgressService>();
        }
    }
}