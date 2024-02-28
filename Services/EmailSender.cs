using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace RealEstatePipeline.Services
{
    public class EmailSender :  IEmailSender
    {
        private readonly string _apiKey;

        public EmailSender(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var client = new SendGridClient(_apiKey);
                var from = new EmailAddress("eaglesalejandro@gmail.com", "Match Make Realty");
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "Hello world", htmlMessage);
                var response = await client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                {
                    // Log the unsuccessful status code and response body for debugging
                    var errorMessage = await response.Body.ReadAsStringAsync();
                 
                    // Log or handle the error message appropriately
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error sending email: {ex.Message}");
            }

           
        }
    }
}
