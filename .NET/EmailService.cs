using Microsoft.AspNetCore.Hosting;
using Sabio.Models.AppSettings;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;
using Sabio.Models.Domain;
using Sabio.Models.Requests.NewsletterSubscription;
using System.Security.Policy;
using Sabio.Models.Requests.Users;
using Sabio.Models.Domain.Users;
using Sabio.Services.Interfaces;


namespace Sabio.Services
{
    public class EmailService : IEmailService
    {
        private readonly IWebHostEnvironment _env;
        private AppKeys _appKeys;
        private SendInBlueAdmin _sib;

        public EmailService (IOptions<AppKeys> appKeys,
            IOptions<SendInBlueAdmin> sib,
            IWebHostEnvironment env)
        {
            _env = env;
            _appKeys = appKeys.Value;
            Configuration.Default.ApiKey.Add("api-key", _appKeys.SendInBlueAppKey);
            _sib = sib.Value;
        }


        public async void TestEmail(string toEmail)
        {

            string senderName = "John Doe";
            string senderEmail = "example@example.com";

            SendSmtpEmailSender email = new SendSmtpEmailSender(senderName, senderEmail);

            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(toEmail);
            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo>();
            to.Add(smtpEmailTo);
            string templateName = "EmailTemplate.html";
            string htmlContent = LoadHtmlTemplate(templateName);
            string textContent = "Text content";
            string subject = "My subject";

            SendSmtpEmail sendSmtpEmail = new SendSmtpEmail(email, to, null, null, htmlContent, textContent, subject);
            await SendEmail(sendSmtpEmail);
        }

        public async void ContactAdmin (ContactUs model)
        {
            SendSmtpEmailSender email = new SendSmtpEmailSender(model.Name, model.Email);

            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(_sib.Email, _sib.Name);
          
            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo>();
            to.Add(smtpEmailTo);
            string textContent = model.Message;
            string subject = "Contact us";
  
            SendSmtpEmail sendSmtpEmail = new SendSmtpEmail(email, to, null, null, null, textContent, subject);
            await SendEmail(sendSmtpEmail);
        }

        public async void ConfirmationEmail(ContactUs model)
        {
        
            SendSmtpEmailSender email = new SendSmtpEmailSender(_sib.Name, _sib.Email);

            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(model.Email, model.Name);
            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo>();
            to.Add(smtpEmailTo);
            string templateName = "ContactUsTemplate.html";
            string htmlContent = LoadHtmlTemplate(templateName);
            string subject = "Thank you for your inquiry";

            SendSmtpEmail sendSmtpEmail = new SendSmtpEmail(email, to, null, null, htmlContent, null, subject);
            await SendEmail(sendSmtpEmail);
        }


        private async Task SendEmail(SendSmtpEmail sendSmtpEmail)
        {
            var apiInstance = new TransactionalEmailsApi();
            var result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
        }


        public string LoadHtmlTemplate(string templateName)
        {
            string tempPath = Path.Combine(_env.WebRootPath, "EmailTemplates", templateName); ;
            string template = File.ReadAllText(tempPath);
            return template;
        }

        public async void SendRegisterConfirmation(UserAddRequest model, string token)
        {
            SendSmtpEmailSender email = new SendSmtpEmailSender(_sib.Name, _sib.Email);
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(model.Email, $"{model.FirstName}");
            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo>();
            to.Add(smtpEmailTo);
            string link = _appKeys.BaseClientUrl + "/confirm" + $"?email={model.Email}&token={token}"; string htmlContent = LoadHtmlTemplate("ConfirmRegTemplate.html").Replace("{{FirstName}}", model.FirstName).Replace("{{domain}}", link);
            string subject = "Hasty - Confirm New User Email";

            SendSmtpEmail sendSmtpEmail = new SendSmtpEmail(email, to, null, null, htmlContent, null, subject);
            await SendEmail(sendSmtpEmail);

        }

        public async void SendResetPassword(UserAuth model, string token)
        {
            SendSmtpEmailSender email = new SendSmtpEmailSender(_sib.Name, _sib.Email);
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(model.Email);
            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo>();
            to.Add(smtpEmailTo);
            string link = _appKeys.BaseClientUrl + "/changepassword" + $"?email={model.Email}&token={token}"; string htmlContent = LoadHtmlTemplate("ResetPasswordTemplate.html").Replace("{{domain}}", link);
            string subject = "Hasty - Change Password";

            SendSmtpEmail sendSmtpEmail = new SendSmtpEmail(email, to, null, null, htmlContent, null, subject);
            await SendEmail(sendSmtpEmail);

        }

        
        public async void SendNewsletter(NewsletterSub model)
        {
            SendSmtpEmailSender email = new SendSmtpEmailSender(_sib.Name, _sib.Email);
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(model.Email);
            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo>();
            to.Add(smtpEmailTo);
            string link = _appKeys.BaseClientUrl + "/unsubscribe" + $"?email={model.Email}"; 
            string htmlContent = LoadHtmlTemplate("NewsletterTemplate.html").Replace("{{domain}}", link);
            string subject = "Hasty - Newsletter";

            SendSmtpEmail sendSmtpEmail = new SendSmtpEmail(email, to, null, null, htmlContent, null, subject);
            await SendEmail(sendSmtpEmail);

        }
        
    }
}


