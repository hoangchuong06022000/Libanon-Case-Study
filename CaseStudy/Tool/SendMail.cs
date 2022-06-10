using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CaseStudy.Tool
{
    public class SendMail
    {
        private string sRandomOTP;

        private string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };


        public SendMail()
        {
            this.sRandomOTP = GenerateRandomOTP(6, saAllowedCharacters);
        }
        public void sendConfirm(string subject, string emailTo, string mess)
        {
            var senderEmail = new MailAddress("libanon@outlook.com.vn");
            var receiverEmail = new MailAddress(emailTo);
            var password = "MOYmeoHONG321@";
            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var message = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = mess,
            })
            {
                smtp.Send(message);
            }
        }

        public void sendOTP(string subject, string emailTo)
        {
            var senderEmail = new MailAddress("libanon@outlook.com.vn");
            var receiverEmail = new MailAddress(emailTo);
            var password = "MOYmeoHONG321@";
            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password),
            };
            using (var message = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = subject,
                Body = this.sRandomOTP
            })
            {
                smtp.Send(message);
            }
        }
        public string getOTP()
        {
            return this.sRandomOTP;
        }
        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {

            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }
    }
}