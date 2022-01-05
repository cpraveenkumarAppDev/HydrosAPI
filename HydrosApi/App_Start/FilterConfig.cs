namespace HydrosApi.App_Start
{    
    using System.Web.Mvc;
    using System.IO;
    using Data;
    using System;
    using System.Web;
    using System.Web.Http.Filters;
    using System.Web.Http.ExceptionHandling;
    using System.Configuration;
     

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {           
            filters.Add(new HandleErrorAttribute());
        }
    }

    /// <summary>
    /// Logger
    /// </summary>      
   

    public class Logger : ExceptionLogger
    {
        /// <summary>
        /// HandleException
        /// </summary>
        /// <remarks>
        /// <param name="exception">exception</param>
        /// <param name="email">email (optional, default=false)</param>       
        /// <para>Write exception to log file</para>   
        /// <para>Set email parameter to true to send an email</para>        
        /// </remarks> 
        public void HandleException(Exception exception, bool email = false)
        {
            var msg = "";
            var log = QueryResult.BundleExceptions(exception);
            //Write the exception to your logs

            var currentContext = HttpContext.Current;
            string currentIp = currentContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string ip = currentIp == null ? currentContext.Request.UserHostAddress : currentIp;

            string filePath = currentContext.Server.MapPath("~/Logging");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            FileStream fs = new FileStream(String.Format("{0}/Error.txt", filePath), FileMode.OpenOrCreate, FileAccess.ReadWrite);

            StreamWriter s = new StreamWriter(fs);
            s.WriteLine(string.Format("Hydros {0}: IP: {1}, Date: {2}", ConfigurationManager.AppSettings["Environment"], ip, DateTime.Now));
            if (exception.Message != null)
            {
                msg += $"Message: {exception.Message}{Environment.NewLine}";
            }

            if (exception.StackTrace != null)
            {
                msg += $"Stack: {exception.StackTrace}{Environment.NewLine}";
            }

            log += msg;

            s.Write($"{log}{Environment.NewLine}");
            s.Close();
            fs.Close();
            if (email == true)
            {
                var errorOrigin = string.Format("Error {0}", ConfigurationManager.AppSettings["Environment"]);
                SendEmail.Message("", log, errorOrigin);
            }
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <remarks>
        /// <param name="context">ExceptionLoggerContext</param>        
        /// <para>Write exception to log file and Send mail to recipient(s) set in web.config</para>        
        /// </remarks> 
        public override void Log(ExceptionLoggerContext context)
            {
                var exception = context.Exception;
                HandleException(exception, true);
            }
        }
    }