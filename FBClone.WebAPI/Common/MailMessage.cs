using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Net.Mime;

namespace FBClone.WebAPI.Common
{
    public class MailMessage
    {
        private string mailServer { get; set; }
        private int smtpPort { get; set; }
        private string smtpEmail { get; set; }
        private string smtpPassword { get; set; }
        private string systemFromEmail { get; set; }

        public MailMessage()
        {
            this.mailServer = ConfigurationManager.AppSettings["SmtpMailServer"];
            this.smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
            this.smtpEmail = ConfigurationManager.AppSettings["SmtpEmail"];
            this.smtpPassword = ConfigurationManager.AppSettings["SmtpPwd"];
        }
        public MailMessage(string form)
        {
            this.mailServer = ConfigurationManager.AppSettings["SmtpMailServer"];
            this.smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
            this.smtpEmail = ConfigurationManager.AppSettings["SmtpEmail"];
            this.smtpPassword = ConfigurationManager.AppSettings["SmtpPwd"];

            switch (form)
            {
                case "ContactForm":
                    this.systemFromEmail = ConfigurationManager.AppSettings["ContactEmail"];                    
                    break;
                case "Registration":
                    this.systemFromEmail = ConfigurationManager.AppSettings["NoReplyEmail"];
                    break;
                case "SendRegistrationConfirmation":
                    this.systemFromEmail = ConfigurationManager.AppSettings["NoReplyEmail"];
                    break;
                case "SendPasswordReset":
                    this.systemFromEmail = ConfigurationManager.AppSettings["NoReplyEmail"];
                    break;
                default:
                    break;
            }
        }
        public void SendRegistrationConfirmation(string text, string html, string email, string subject)
        {
            var mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress(this.systemFromEmail);
            mail.To.Add(new MailAddress(email));
            mail.Subject = subject;
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            SMTPDeliverMail(mail);
        }

        public void SendForgotPassword(string text, string html, string email, string subject)
        {
            var mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress(this.systemFromEmail);
            mail.To.Add(new MailAddress(email));
            mail.Subject = subject;
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
            SMTPDeliverMail(mail);
        }

        public void ReceiveContactFormMessage(string name, string email, string phone, string subject, string message)
        {
            var mail = new System.Net.Mail.MailMessage();
            mail.From = new MailAddress(email);
            mail.To.Add(this.smtpEmail);
            mail.Subject = subject;
            mail.Body = String.Format("From: {0}\n Phone: {1}\n Begin Message: {2}", email, phone, message);

            SMTPDeliverMail(mail);
        }

        #region Internal Utility Methods
        private void SMTPDeliverMail(System.Net.Mail.MailMessage mail)
        {
            var smtp = new SmtpClient(this.mailServer, this.smtpPort);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(this.smtpEmail, this.smtpPassword);
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
            }
        }
        #endregion
    }
}