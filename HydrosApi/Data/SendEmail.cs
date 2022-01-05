namespace HydrosApi.Data
{
    using System;
    using System.Configuration;
    using System.Net.Mail;

    public partial class SendEmail
    {
        /// <summary>
        /// Message
        /// </summary>
        /// <remarks>
        /// <param name="receiver">receiver (optional list of recipients, when empty, AdministrationEmailAccount must be set in web.config)</param>  
        /// <param name="body">body</param> 
        /// <param name="subject">subject</param> 
        /// <para>Send an email to one or more recipeients</para>  
        /// <para>Default mail values must be set in web config</para>  
        /// <para> add key="ApplicationEmailAccount" value="noreply@azwater.gov" /> (reply to address)</para>
        /// <para> add key ="AdministratorEmailAccount" value="amoskovits@azwater.gov" /> (default email address)</para>
        /// </remarks> 
        public static void Message(string receiver, string body, string subject)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = false;
            if (receiver.Contains(","))
            {
                foreach (var item in receiver.Split(','))
                {
                    MailMessage mailMessage = new MailMessage(ConfigurationManager.AppSettings["ApplicationEmailAccount"].ToString(), item, subject, body);
                    mailMessage.IsBodyHtml = true;
                    Message(mailMessage);
                }
            }
            else
            {
                MailMessage mailMessage = new MailMessage(ConfigurationManager.AppSettings["ApplicationEmailAccount"].ToString(), ConfigurationManager.AppSettings["AdministratorEmailAccount"].ToString(), subject, body);
                mailMessage.IsBodyHtml = true;
                Message(mailMessage);
            }

        }
        public static void Message(MailMessage message)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = false;

                smtp.Send(message);
            }
            catch (Exception exception)
            {
                QueryResult.BundleExceptions(exception);
            }

        }
    }
}

    
 