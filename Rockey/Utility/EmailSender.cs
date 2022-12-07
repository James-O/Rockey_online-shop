using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace Rockey.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient("3c899ae1b1d8fe8e26fcec3bec5b5e49", "74835d34177fee564aa8901979b57622");
             
                //Version = ApiVersion.V3_1
            
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray {
             new JObject {
              {
               "From",
               new JObject {
                {"Email", "ogbonnasunday42@gmail.com"},
                {"Name", "Ogbonna"}
               }
              }, {
               "To",
               new JArray {
                new JObject {
                 {
                  "Email",
                  email
                 }, {
                  "Name",
                  "JovicsTech"
                 }
                }
               }
              }, {
               "Subject",
               subject
              }, {
               "HTMLPart",
               body
              }
             }
            });
        await client.PostAsync(request);
        }
    }
}
