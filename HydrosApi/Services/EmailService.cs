using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace HydrosApi.Services
{
    public class EmailService
    {
        public static bool Message(MailMessage message)
        {
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    EnableSsl = false
                };
                smtp.Send(message);
                return true;
            }
            catch (Exception exception)
            {
                //log error
                return false;
            }
        }

        public static bool Message(string to, string subject, string body)
        {
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    EnableSsl = false
                };
                var message = new MailMessage("NoReply@azwater.gov", to, subject, body);
                smtp.Send(message);
                return true;
            }
            catch (Exception exception)
            {
                //log error
                return false;
            }
        }

        public static bool Message(List<string> to, string subject, string body)
        {
            try
            {
                string multipleTo = to.Aggregate((index, current) => index + "," + current);

                SmtpClient smtp = new SmtpClient
                {
                    EnableSsl = false
                };
                var message = new MailMessage("NoReply@azwater.gov", multipleTo, subject, body);
                smtp.Send(message);
                return true;
            }
            catch (Exception exception)
            {
                //log error
                return false;
            }
        }
    }
}