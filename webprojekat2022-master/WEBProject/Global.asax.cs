using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WEBProject.Models;
using WEBProject.Models.XML;

namespace WEBProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //XMLWriter.WriteUsers(new List<User>());
            var lista = XMLLoader.GetUsers();
            //XMLWriter.AddData();
            HttpContext.Current.Application["users"] = XMLLoader.GetUsers();
            HttpContext.Current.Application["fitness_centers"] = XMLLoader.GetFitnessCenters();
            HttpContext.Current.Application["group_trainings"] = XMLLoader.GetGroupTrainings();
            HttpContext.Current.Application["comments"] = XMLLoader.GetComments();

           

        }
    }
}
