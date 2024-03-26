using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using WEB.Models;
using WEB.Services.Interface;
namespace WEB.Service
{
  public class SmtpSettings
  {
    public string Server { get; set; }
    public string Port { get; set; }
    public string FromAddress { get; set; }
    public string UserId { get; set; }
    public string Pwd { get; set; }
    public bool IsDevelopmentMode { get; set; }
  }

  public class EmailService : IEmailService
  {

    private readonly SmtpSettings smtpSettings;

    private readonly SmtpClient client;
   public EmailService(IOptions<SmtpSettings> smtpSetting)
    {
      smtpSettings = smtpSetting.Value;
      client = new SmtpClient(smtpSettings.Server);

      if (!string.IsNullOrEmpty(smtpSettings.Port))
        client.Port = int.Parse(smtpSettings.Port);

      if (!string.IsNullOrEmpty(smtpSettings.UserId))
        client.Credentials = new NetworkCredential(smtpSettings.UserId, smtpSettings.Pwd);

      client.EnableSsl = false;

    }

    public void SendEmailAsync(EmailViewModel email)
    {
      SendEmailAsync(email, false);
    }

    public async void SendEmailAsync(EmailViewModel email, bool sendIndividualEmails = true)
    {
      //Code block - Restrict sending emails to users in dev and test modes
      if (smtpSettings.IsDevelopmentMode)
      {

      }
      //End of code block.

      if (client != null)
      {
        if (sendIndividualEmails)
        {
          //sends separate emails to each recipient
          foreach (string emailId in email.ToList)
          {
            MailMessage msg = new MailMessage
            {
              From = new MailAddress(smtpSettings.FromAddress),
              Subject = email.Subject,
              Body = email.Body,
              IsBodyHtml = true
            };
            msg.To.Add(emailId.Trim());
            //msg.To.Add("gurushev.p@daimlertruck.com");
            //msg.Bcc.Add("mohammedashik.s@intelizign.com");
            // msg.Bcc.Add("sikkandar.kadhar@intelizign.com");
            client.SendCompleted += EmailSendCompleted;
            //await client.SendMailAsync(msg);
          }
        }
        else
        {
          //sends a single email to all recipients
          MailMessage msg = new MailMessage
          {
            From = new MailAddress(smtpSettings.FromAddress),
            Subject = email.Subject,
            Body = email.Body,
            IsBodyHtml = true
          };

          msg.To.Add(string.Join(",", email.ToList));
          //msg.To.Add("gurushev.p@daimlertruck.com");
          //msg.Bcc.Add("mohammedashik.s@intelizign.com");
          client.SendCompleted += EmailSendCompleted;
          await client.SendMailAsync(msg);
        }
      }
    }

    private void EmailSendCompleted(object sender, AsyncCompletedEventArgs e)
    {
      if (e.Error == null)
      {
        //logger.LogError("Send email completed!");
      }
      else
      {
        //logger.LogError(e.Error);
      }

    }
    public class SmtpSettings
    {
      public string Server = "53.151.100.102";
      public string Port = "25";
      //public string FromAddress = "DICV-CMT@DAIMLER.COM";Mail from address have been changed to MFTBC-CMT@DAIMLER.COM as per requirement-08-12-2023
      public string FromAddress = "dta_qm_portal@daimlertruck.com";//"MFTBC-CMT@DAIMLER.COM";//dta_qm_portal@daimlertruck.com
			public string UserId = "DICV-EBOM@DAIMLER.COM";
      public string Pwd = "Dicv@123";
      public bool IsDevelopmentMode = true;
    }
    public async void EmailSendingFunction(string email,string mailbody,string Subject)
    {
        
      if (email != null)
      {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                SmtpSettings smtpvalue = new SmtpSettings();
                message.From = new MailAddress(smtpvalue.FromAddress);
                message.To.Add(new MailAddress(email.Trim()));
                //message.To.Add(new MailAddress("srinidhis189@gmail.com"));
                //message.To.Add("gurushev.p@daimlertruck.com");
                //message.Bcc.Add("mohammedashik.s@intelizign.com");  
                message.Subject = Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = mailbody;
                smtp.Port = int.Parse(smtpvalue.Port);
                smtp.Host = smtpvalue.Server; //for gmail host  
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(smtpvalue.UserId, smtpvalue.Pwd);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            
            }
      //else
      //{

      //}
    }
  }
}
