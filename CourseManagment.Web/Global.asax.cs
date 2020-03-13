using CourseManagment.BLL.Infrastructure;
using CourseManagment.Web.Util;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CourseManagment.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            NinjectModule courseModule = new ViewModule();
            NinjectModule serviceModule = new ServiceModule("CourseContext");
            var kernel = new StandardKernel(courseModule, serviceModule);
            kernel.Unbind<ModelValidatorProvider>();
            var ninjectResolver = new NinjectDependencyResolver(kernel);
            DependencyResolver.SetResolver(ninjectResolver);

        }
    }
}
