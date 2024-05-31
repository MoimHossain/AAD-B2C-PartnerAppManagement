
using AadB2CAppRegMgmtApp;
using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Applications.Item.AddPassword;
using Microsoft.Graph.Models;

var config = ConfigReader.Instance;
var clientSecretCredential = new ClientSecretCredential(
    config.TenantId, config.ClientId, config.ClientSecret);

try
{
    string[] scopes = { "https://graph.microsoft.com/.default" };

    AccessToken token = await clientSecretCredential.GetTokenAsync(new TokenRequestContext(scopes));

    Console.WriteLine($"Access Token: {token.Token}");


    var graphClient = new GraphServiceClient(clientSecretCredential);
    var appObjectId = "754fdcaa-7744-4b75-abab-f99aaca481bd";

    var application = await graphClient.Applications[appObjectId].GetAsync();

    if (application != null)
    {
        Console.WriteLine($"Application found: {application.DisplayName}");


        


        //await CreateClientSecretAsync(graphClient, appObjectId);
    }
    else
    {
        Console.WriteLine("Application not found.");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

static async Task CreateClientSecretAsync(GraphServiceClient graphClient, string appObjectId)
{
    var passwordCredentialResponse = await graphClient.Applications[appObjectId]
        .AddPassword.PostAsync(new AddPasswordPostRequestBody
        {
            PasswordCredential = new PasswordCredential
            {
                DisplayName = "New Client Secret",
                StartDateTime = DateTimeOffset.Now,
                EndDateTime = DateTimeOffset.Now.AddYears(2)
            }
        });

    if (passwordCredentialResponse != null)
    {
        Console.WriteLine("Client secret created successfully.");
        Console.WriteLine($"Secret ID: {passwordCredentialResponse.CustomKeyIdentifier}");
        Console.WriteLine($"Secret Value: {passwordCredentialResponse.SecretText}");
    }
}