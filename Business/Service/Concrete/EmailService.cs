
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Collections.Generic;

public class EmailService
{
    private readonly SmtpClient smtp;

    public EmailService()
    {
        smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("rizamertyagci@gmail.com", "myiodpjkqpddwbss")
        };
    }

    public void SendAdvancedEmail(
        string fromEmail, 
        string fromName, 
        List<string> toEmails, 
        List<string> ccEmails, 
        List<string> bccEmails, 
        string subject, 
        string htmlBody, 
        List<string> attachmentPaths)
    {
        try
        {
            var fromAddress = new MailAddress(fromEmail, fromName);
            var message = new MailMessage()
            {
                From = fromAddress,
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
            };

            toEmails.ForEach(email => message.To.Add(email));
            ccEmails.ForEach(email => message.CC.Add(email));
            bccEmails.ForEach(email => message.Bcc.Add(email));

            attachmentPaths.ForEach(path => {
                if (File.Exists(path))
                {
                    var attachment = new Attachment(path, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(path);
                    disposition.ModificationDate = File.GetLastWriteTime(path);
                    disposition.ReadDate = File.GetLastAccessTime(path);
                    message.Attachments.Add(attachment);
                }
            });

            smtp.Send(message);
        }
        catch (Exception ex)
        {
            // Hata işleme - bu durumda, hatayı yakalayıp konsola yazdırıyoruz,
            // Ancak gerçek bir uygulamada daha gelişmiş hata işlemeye ihtiyaç duyabilirsiniz.
            Console.WriteLine("E-posta gönderilirken bir hata oluştu: " + ex.ToString());
        }
    }
}
