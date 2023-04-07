using WEB.Models;
namespace WEB.Services.Interface; 
 public interface IEmailService
    {
        public void SendEmailAsync(EmailViewModel email);
        public void SendEmailAsync(EmailViewModel email, bool sendIndividualEmails = true);
        public  void EmailSendingFunction(string email,string mailbody,string Subject);

    }