
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


    var appReponse = await graphClient.Applications.GetAsync();
    if(appReponse != null && appReponse.Value != null)
    {
        appReponse.Value.ForEach(app =>
        {
            Console.WriteLine($"Application: {app.DisplayName}");
        });
    }
    



    // get user by user id

    var user = await graphClient.Users["e3646e12-0bc1-4b09-82bb-9b7580595065"].GetAsync();



    //await RegisterAppAsync(graphClient);



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

static async Task RegisterAppAsync(GraphServiceClient graphClient)
{
    var newApplication = new Application
    {
        DisplayName = $"MOIMHOSSAIN-{DateTime.Now.Ticks}",
        SignInAudience = "AzureADMyOrg",
        Tags = ["Pizza", "Dominos"],
        ExtensionProperties = new List<ExtensionProperty>
        {
            new ExtensionProperty
            {
                DataType = "String",
                Name = "extension_foo",
                TargetObjects = ["Application"]
            }
        },
        Web = new WebApplication
        {
            RedirectUris = ["https://microsoft.com"]
        }
    };

    var app = await graphClient.Applications.PostAsync(newApplication);
}