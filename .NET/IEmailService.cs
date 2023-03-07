using Sabio.Models.Domain;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests;
using Sabio.Models.Requests.NewsletterSubscription;
using Sabio.Models.Requests.Users;

namespace Sabio.Services.Interfaces
{
    public interface IEmailService
    {
        void TestEmail(string toEmail);
        void ContactAdmin(ContactUs model);
        void ConfirmationEmail(ContactUs model);
        void SendRegisterConfirmation(UserAddRequest model, string token);
        void SendResetPassword(UserAuth model, string token);

        void SendNewsletter(NewsletterSub model);
    }
}