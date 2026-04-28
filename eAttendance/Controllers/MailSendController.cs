using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace eAttendance.Controllers
{
    public class MailSendController : Controller
    {


        public static bool SendEmail(string to, string subject, string body)
        {
            bool flag = true;
            SmtpSection section = WebConfigurationManager.OpenWebConfiguration("~/net.config").GetSection("system.net/mailSettings/smtp") as SmtpSection;
            try
            {
                if (section != null)
                {
                    SmtpClient smtpClient = new SmtpClient();
                    MailMessage message = new MailMessage();
                    NetworkCredential networkCredential = new NetworkCredential();
                    smtpClient.Host = section.Network.Host;
                    smtpClient.Port = section.Network.Port;
                    smtpClient.EnableSsl = Convert.ToBoolean("true");
                    smtpClient.UseDefaultCredentials = true;
                    networkCredential.UserName = section.Network.UserName;
                    networkCredential.Password = section.Network.Password;
                    smtpClient.Credentials = (ICredentialsByHost)networkCredential;
                    message.From = new MailAddress(section.From);
                    message.To.Add(to);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    smtpClient.Send(message);
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
    }
}