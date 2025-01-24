using CSharpTypes.Extensions.String;
using DRY.MailJetClient.Library;
using LocationMarker.Data.Interfaces;
using LocationMarker.Entities.Models;
using LocationsMarker.API.Configurations;
using System.Text;

namespace LocationsMarker.API.Extensions
{
    internal static class SeedHandlerExtensions
    {
        const int KEY_AGE = 30;

        public static async Task SeedSystemData(this WebApplication app, ILogger<Program> logger)
        {
            var sp = app.Services.CreateScope().ServiceProvider;
            await SeedApiKeys(sp, app.Configuration, logger);
        }

        public static async Task SetConstants(this WebApplication app)
        {
            var sp = app.Services.CreateScope().ServiceProvider;
            var repo = sp.GetRequiredService<IRepositoryManager>();
            if(repo != null)
            {
                var apiKey = await repo.ApiKey.FindAsync(k => !k.IsDeprecated);
                if(apiKey != null)
                {
                    AppConstants.ApiKey = apiKey.ApiKey;
                }
            }
        }

        private static async Task SeedApiKeys(IServiceProvider serviceProvider, 
            IConfiguration configuration, ILogger<Program> logger)
        {
            var repository = serviceProvider.GetRequiredService<IRepositoryManager>();
            if (repository != null)
            {
                var adminEmail = configuration["MailJet:AlertEmail"];
                var existingKey = await repository.ApiKey.FindAsync(k => !k.IsDeprecated);
                if (existingKey != null)
                {
                    logger.LogInformation("There's an existing key");
                    var age = (DateTime.UtcNow - existingKey.CreatedOn).Days;
                    if(age > KEY_AGE)
                    {
                        var newKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.UtcNow.Ticks.ToString()));
                        logger.LogInformation("Generating new ApiKey");
                        existingKey.CreatedOn = DateTime.UtcNow;
                        existingKey.UpdatedOn = DateTime.UtcNow;
                        existingKey.ApiKey = newKey;

                        logger.LogInformation("Updating the ApiKey with the newly generated key...");
                        await repository.ApiKey.EditAsync(k => k.Id.Equals(existingKey.Id), existingKey);
                        logger.LogInformation("Updated the ApiKey with the newly generated key");

                        await AlertAdmin(serviceProvider, logger, adminEmail!, newKey, false);
                    }
                    else
                    {
                        logger.LogInformation("Key generation skipped!!!");
                    }
                }
                else
                {
                    logger.LogInformation("Adding new ApiKey to the Database...");
                    var newKeyEntity = new ApiKeys
                    {
                        ApiKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.UtcNow.Ticks.ToString()))
                    };
                    await repository.ApiKey.AddAsync(newKeyEntity);

                    await AlertAdmin(serviceProvider, logger, adminEmail!, newKeyEntity.ApiKey, true);
                    logger.LogInformation("Added a new ApiKey to the Database...");
                }
            }
        }

        private static string GetMessage(string key)
        {
            return $@"<p>
                Hi Admin,<br/>
                This is notify you that Location Marker API Key have been expired and a new one generate.<br/>
                Below is your new API Key to access sensitive resource on the Location Marker API.<br/>
                {key}.<br/>
                Please keep it secret and delete this message ASAP.<br/>
                Regards.<br/>
                Location Marker Team.<br/>
                </p>".Trim();
        }

        private static async Task AlertAdmin(IServiceProvider serviceProvider, ILogger<Program> logger, 
            string userEmail, string newKey, bool isNew)
        {
            var emailService = serviceProvider.GetRequiredService<IMailjetClientService>();
            if (emailService != null && userEmail.IsNotNullOrEmpty())
            {
                var beforeSendingMessage = isNew ? "Alerting the Admin of the generation of ApiKey..." :
                    "Alerting the Admin of the change in ApiKey...";
                var afterSendingMessage = isNew ? "Admin alerted of the Api Key generation..." :
                    "The Admin. alerted of the change in ApiKey...";
                var subject = isNew ? "Api Key Generation Notification" : "Api Key Change Notification";

                logger.LogInformation(beforeSendingMessage);
                await emailService.SendAsync(userEmail, GetMessage(newKey), subject);
                logger.LogInformation(afterSendingMessage);
            }
        }
    }
}
