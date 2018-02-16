using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace Hermeco.Cultura.Aniversario
{
    internal class Utility
    {
        internal static void sendEmail(string smtpServer, string subject, string htmlPath, string body, string from, string to, string pathAttachment, bool isBodyHtml, string CC, Dictionary<string, string> parameters = null)
        {

            MailMessage mail = new MailMessage(from, to) { From = new MailAddress(from, "Yo Soy Cultura OFFCORSS"), IsBodyHtml = isBodyHtml, Subject = subject };

            SmtpClient client = new SmtpClient() { Port = 25, DeliveryMethod = SmtpDeliveryMethod.Network, UseDefaultCredentials = false, Host = smtpServer };

            if (isBodyHtml)
            {
                mail.IsBodyHtml = true;

                using (StreamReader reader = File.OpenText(htmlPath))
                {
                    var inlineLogo = new LinkedResource(AppDomain.CurrentDomain.BaseDirectory + "Email//img//tarjeta-Aniversario-.jpg");
                    inlineLogo.ContentId = Guid.NewGuid().ToString();
                    string body2 = string.Format(@reader.ReadToEnd(), inlineLogo.ContentId);
                    var view = AlternateView.CreateAlternateViewFromString(body2, null, "text/html");
                    view.LinkedResources.Add(inlineLogo);
                    mail.AlternateViews.Add(view);
                }
            }
            else
            {
                mail.Body = body;
            }

            if (!string.IsNullOrEmpty(pathAttachment))
            {
                mail.Attachments.Add(new Attachment(pathAttachment));
            }

            if (!string.IsNullOrEmpty(CC))
            {
                var MailCC = new MailAddress(CC);
                mail.CC.Add(MailCC);
            }

            client.Send(mail);
        }
    }
}