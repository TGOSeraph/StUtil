using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace StUtil.Net.Mail
{
    /// <summary>
    /// Class for sending emails using Gmail
    /// </summary>
    /// <remarks>
    /// 2013-08-05  - Initial version
    /// </remarks>
    public class Gmail
    {
        /// <summary>
        /// The email address to send from
        /// </summary>
        public string FromAddress { get; private set; }
        /// <summary>
        /// The email address to send to
        /// </summary>
        public string ToAddress { get; private set; }
        /// <summary>
        /// The from accounts password
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Send an email using Gmails SMTP servers
        /// </summary>
        /// <param name="from">The email address to send from</param>
        /// <param name="to">The email address to send to</param>
        /// <param name="password">The from accounts password</param>
        public Gmail(string from, string to, string password)
        {
            this.FromAddress = from;
            this.ToAddress = to;
            this.Password = password;
        }

        /// <summary>
        /// Send an email using Gmails SMTP servers
        /// </summary>
        /// <param name="subject">The subject of the email</param>
        /// <param name="body">The body of the email</param>
        /// <param name="isHtml">If the body is HTML formatted</param>
        public void Send(string subject, string body, bool isHtml = false)
        {
            Gmail.Send(FromAddress, ToAddress, Password, subject, body);
        }

        /// <summary>
        /// Send an email using Gmails SMTP servers
        /// </summary>
        /// <param name="from">The email address to send from</param>
        /// <param name="to">The email address to send to</param>
        /// <param name="password">The from accounts password</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="body">The body of the email</param>
        /// <param name="isHtml">If the body is HTML formatted</param>
        /// <param name="fileToAttach">The file to attach to the email</param>
        public static void Send(string from, string to, string password, string subject, string body, bool isHtml = false, string fileToAttach = null)
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, password)
            };

            using (var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            })
            {
                if (fileToAttach != null)
                {
                    using (System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(fileToAttach))
                    {
                        message.Attachments.Add(attachment);
                        smtp.Send(message);
                    }
                }
                else
                {
                    smtp.Send(message);
                }
            }
        }
    }
}
