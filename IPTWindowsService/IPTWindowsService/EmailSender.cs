using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace IPTWindowsService
{
    class EmailSender
    {

        string emailAddress;
        string username;
        string password;
        string smtpClient;
        string subject;

        public EmailSender()
        {
            emailAddress = ConfigurationManager.AppSettings["Email"];
            username = ConfigurationManager.AppSettings["Username"];
            password = ConfigurationManager.AppSettings["Password"];
            smtpClient = ConfigurationManager.AppSettings["SmtpClient"];
            subject = ConfigurationManager.AppSettings["EmailSubject"];
        }

        public void Send(string recipentEmail, string content)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(smtpClient);
                mail.From = new MailAddress(emailAddress);
                mail.To.Add(recipentEmail);
                mail.Subject = subject;
                mail.Body = "Mail Send from IPT Search Module\n" + content;

                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(emailAddress, password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }



    }
}
