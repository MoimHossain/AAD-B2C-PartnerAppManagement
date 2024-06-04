using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Applications.Item.AddPassword;
using Microsoft.Graph.Models;

namespace B2CBackend
{
    public class B2CAppService(
        ILogger<B2CAppService> logger,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        ApimService apimService,
        ConfigReader configReader)
    {
        public async Task<IResult> GetAppRegsAsync(InvocationContext? context)
        {
            if(context == null)
            {
                return Results.BadRequest("Invalid context");
            }

            var user = await apimService.GetUserAsync(context);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            var apps = await ListAppRegsAsync(context.UserId);
            return Results.Ok(apps);
        }

        public async Task<IResult> NewAppRegsAsync(InvocationContext? context, GeneratePayload? payload)
        {
            if (context == null || payload == null)
            {
                return Results.BadRequest("Invalid context");
            }

            var user = await apimService.GetUserAsync(context);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            List<AppReg> appRegs = [];
            var app = await GetAppAsync(context.UserId);

            GraphServiceClient graphClient = GetGraphClient(configReader);

            if (app == null)
            {
                var newApplication = new Application
                {
                    DisplayName = $"{context.UserId}",
                    SignInAudience = "AzureADMyOrg",
                    Tags = [$"{context.UserId}"],          
                    Web = new Microsoft.Graph.Models.WebApplication
                    {
                        RedirectUris = ["https://microsoft.com"]
                    }
                };
                app = await graphClient.Applications.PostAsync(newApplication);
            }


            if (app != null)
            {
                if (app.PasswordCredentials != null && app.PasswordCredentials.Count > 0)
                {
                    foreach (var pc in app.PasswordCredentials)
                    {
                        appRegs.Add(new AppReg(
                            app.AppId, $"{pc.KeyId}",
                            pc.DisplayName, pc.SecretText,
                            configReader.TenantId,
                            pc.StartDateTime, pc.EndDateTime));
                    }
                }

                var passwordCredentialResponse = await graphClient.Applications[app.Id]
                    .AddPassword.PostAsync(new AddPasswordPostRequestBody
                    {
                        PasswordCredential = new PasswordCredential
                        {
                            DisplayName = $"{payload.DisplayName}",
                            StartDateTime = DateTimeOffset.Now,
                            EndDateTime = DateTimeOffset.Now.AddYears(2)
                        }
                    });

                if (passwordCredentialResponse != null)
                {
                    appRegs.Add(new AppReg(
                        app.AppId, $"{passwordCredentialResponse.KeyId}",
                        passwordCredentialResponse.DisplayName, passwordCredentialResponse.SecretText,
                        configReader.TenantId,
                        passwordCredentialResponse.StartDateTime, passwordCredentialResponse.EndDateTime));
                }
            }

            return Results.Ok(appRegs);
        }

        public async Task<List<AppReg>> ListAppRegsAsync(string? prefix)
        {
            List<AppReg> appRegs = [];
            var app = await GetAppAsync(prefix);

            if (app != null)
            {
                if (app.PasswordCredentials != null && app.PasswordCredentials.Count > 0)
                {
                    foreach (var pc in app.PasswordCredentials)
                    {
                        appRegs.Add(new AppReg(
                            app.AppId, $"{pc.KeyId}",
                            pc.DisplayName, pc.SecretText,
                            configReader.TenantId,
                            pc.StartDateTime, pc.EndDateTime));
                    }
                }
            }
            return appRegs;
        }

        public async Task<Microsoft.Graph.Models.Application?> GetAppAsync(string? prefix)
        {   
            try
            {
                GraphServiceClient graphClient = GetGraphClient(configReader);

                var response = await graphClient.Applications.GetAsync();
                if (response != null && response.Value != null)
                {
                    foreach (var app in response.Value)
                    {
                        if ($"{app.DisplayName}".Contains($"{prefix}", StringComparison.OrdinalIgnoreCase))
                        {
                            return app;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting app registrations");
            }
            return null;
        }

        private static GraphServiceClient GetGraphClient(ConfigReader configReader)
        {
            var clientSecretCredential = new ClientSecretCredential(
                configReader.TenantId, configReader.ClientId, configReader.ClientSecret);
            var graphClient = new GraphServiceClient(clientSecretCredential);
            return graphClient;
        }
    }
}