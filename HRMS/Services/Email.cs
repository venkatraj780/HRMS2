using System.Net.Mail;

namespace HRMS.Services
{
    public static class Email
    {
        public static bool Send(string ToEmail, string Subject, string Body)
        {
            bool mailSent = false;
            try
            {
                string appPasswordForFromEmail = "";
                string fromEmail = "";

                SmtpClient mailServer = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromEmail, appPasswordForFromEmail)
                };

                MailMessage msg = new MailMessage(fromEmail, ToEmail)
                {
                    Subject = Subject,
                    Body = Body,
                    IsBodyHtml = true
                };

                mailServer.Send(msg);
                mailSent = true;

            }
            catch (Exception ex)
            {
                mailSent = false;
            }
            return mailSent;
        }
    }
}
