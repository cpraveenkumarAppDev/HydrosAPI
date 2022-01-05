using HydrosApi.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Mvc;

namespace HydrosApi
{
    public class WebApiApplication : HttpApplication
    {
        //protected void Application_Error(object sender, EventArgs e)
        //{
          //  Exception ex = Server.GetLastError();
           // ErrorLogService.LogError(ex);
        //}

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            HttpConfiguration config = GlobalConfiguration.Configuration;
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            
            //When JsonFormatter tries to make json out of the entityframework(EF) objects, it messes up
            //the EF objects can have circular referrences that makes an infinite loop in the
            //recursive algorithms that the formatter is using
            //To avoid problem globally use the Newtonsoft ignore below
            //https://stackoverflow.com/questions/19467673/entity-framework-self-referencing-loop-detected
            config.Formatters.JsonFormatter
                .SerializerSettings
                .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));

        }

       



    }
}
