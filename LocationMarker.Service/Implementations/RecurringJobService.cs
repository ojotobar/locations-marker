using CSharpTypes.Extensions.String;
using LocationMarker.Data.Interfaces;
using LocationMarker.Entities.Models;
using LocationMarker.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using DRY.MailJetClient.Library;
using Hangfire.Server;
using Hangfire.Console;

namespace LocationMarker.Service.Implementations
{
    public class RecurringJobService : IRecurringJobService
    {
        private readonly IRepositoryManager repository;
        private readonly IConfiguration configuration;
        private readonly string adminEmail;
        private readonly ILogger<RecurringJobService> logger;
        private readonly IMailjetClientService mailjetClient;

        public RecurringJobService(IRepositoryManager repository, IConfiguration configuration, 
            ILogger<RecurringJobService> logger, IMailjetClientService mailjetClient)
        {
            this.repository = repository;
            this.configuration = configuration;
            this.adminEmail = configuration["MailJet:AlertEmail"] ?? "";
            this.logger = logger;
            this.mailjetClient = mailjetClient;
        }

        public async Task AddOrRotateApiKey(PerformContext context)
        {
            logger.LogInformation($"{DateTimeOffset.UtcNow:u} Key Rotation Started");
            context.WriteLine($"{DateTimeOffset.UtcNow:u} Key Rotation Started");
            var existingKey = await repository.ApiKey.FindAsync(k => !k.IsDeprecated);
            if (existingKey != null)
            {
                var newKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.UtcNow.Ticks.ToString()));
                logger.LogInformation($"{DateTimeOffset.UtcNow:u} Generating new ApiKey");
                existingKey.CreatedOn = DateTime.UtcNow;
                existingKey.UpdatedOn = DateTime.UtcNow;
                existingKey.ApiKey = newKey;

                logger.LogInformation("Updating the ApiKey with the newly generated key...");
                await repository.ApiKey.EditAsync(k => k.Id.Equals(existingKey.Id), existingKey);
                logger.LogInformation($"{DateTimeOffset.UtcNow:u} Updated the ApiKey with the newly generated key");

                await AlertAdmin(newKey, false);
            }
            else
            {
                logger.LogInformation("Adding new ApiKey to the Database...");
                var newKeyEntity = new ApiKeys
                {
                    ApiKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.UtcNow.Ticks.ToString()))
                };
                await repository.ApiKey.AddAsync(newKeyEntity);

                await AlertAdmin(newKeyEntity.ApiKey, true);
                logger.LogInformation("Added a new ApiKey to the Database...");
            }
        }

        private static string GetMessage(string key)
        {
            var message = $@"
                <p style=""font-family:Segoe UI, sans-serif; font-size:14px; color:#333;"">
                    Hi Admin,<br/><br/>
                    This is to notify you that your <strong>Location Marker API Key</strong> has expired and a new one has been generated.<br/><br/>
                    Below is your new API Key for accessing sensitive resources on the Location Marker API:<br/><br/>
                    <code style=""display:inline-block; background:#f2f2f2; color:#2c2c2c; padding:10px 15px; font-size:16px; border-radius:6px; font-weight:bold;"">
                        {key}
                    </code><br/><br/>
                    <strong>Please keep this key confidential and delete this message after storing it securely.</strong><br/><br/>
                    Regards,<br/>
                    <em>Location Marker Team</em>
                </p>".Trim();

            return message;
        }

        private async Task AlertAdmin(string newKey, bool isNew)
        {
            if (adminEmail.IsNotNullOrEmpty())
            {
                var dateTimeOffset = DateTimeOffset.UtcNow;
                var beforeSendingMessage = isNew ? $"{dateTimeOffset:u} Alerting the Admin of the generation of ApiKey..." :
                    $"{dateTimeOffset:u} Alerting the Admin of the change in ApiKey...";
                var afterSendingMessage = isNew ? $"{dateTimeOffset:u} The Admin alerted of the Api Key generation..." :
                    $"{dateTimeOffset:u} The Admin alerted of the change in ApiKey...";
                var subject = isNew ? "Api Key Generation Notification" : "Api Key Change Notification";

                logger.LogInformation(beforeSendingMessage);
                await mailjetClient.SendAsync(adminEmail, GetMessage(newKey), subject);
                logger.LogInformation(afterSendingMessage);
            }
        }
    }
}
