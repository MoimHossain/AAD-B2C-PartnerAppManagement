


using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;


var tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");
var clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
var clientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET");
var directoryExtensionAppObjectId = Environment.GetEnvironmentVariable("AZURE_DIRECTORY_EXTENSION_APP_OBJECT_ID");


const string EXT_PROP_PARTNER_NAME = "VendorName";

const string EXT_PROP_PARTNER_VALUE = "Contoso Inc.";

List<string> extensionPropertyNames = [EXT_PROP_PARTNER_NAME];

var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
var graphClient = new GraphServiceClient(clientSecretCredential);


var response = await graphClient.Applications[directoryExtensionAppObjectId].ExtensionProperties.GetAsync();
if(response != null && response.Value != null)
{
    #region Define Extension Property
    var extPropDefinitions = response.Value;
    foreach (var propName in extensionPropertyNames)
    {
        var exists = extPropDefinitions
            .Exists(extProp => extProp != null 
                && extProp.Name != null 
                && extProp.Name.EndsWith(propName, StringComparison.OrdinalIgnoreCase));
        if(!exists)
        {
            var requestBody = new ExtensionProperty
            {
                Name = propName,
                DataType = "String",
                TargetObjects = ["User", "Application"]
            };
            var createResult = await graphClient
                .Applications[directoryExtensionAppObjectId]
                .ExtensionProperties
                .PostAsync(requestBody);
        }
    }
    #endregion


    #region Create New App with Extension property

    var partnerNameExtPropDefinition = response.Value
        .FirstOrDefault(extProp => extProp != null 
        && extProp.Name != null 
        && extProp.Name.EndsWith(EXT_PROP_PARTNER_NAME, StringComparison.OrdinalIgnoreCase));
    if (partnerNameExtPropDefinition != null && partnerNameExtPropDefinition.Name != null)
    {
        var extPropSysName = partnerNameExtPropDefinition.Name;
        var newApplication = new Application
        {
            DisplayName = $"TestApp-{DateTime.Now.ToShortDateString()}",
            SignInAudience = "AzureADMyOrg",
            Tags = ["TestApp"],
            AdditionalData = new Dictionary<string, object>
            {
                {
                    extPropSysName , EXT_PROP_PARTNER_VALUE
                },
            },
            Web = new Microsoft.Graph.Models.WebApplication
            {
                RedirectUris = ["https://microsoft.com"]
            }
        };
        var app = await graphClient.Applications.PostAsync(newApplication);

        Console.WriteLine($"New App created with Extension Property: {extPropSysName}");

        // validate
        app = await graphClient.Applications[app.Id].GetAsync();
        // delete
        await graphClient.Applications[app.Id].DeleteAsync();
    }
    #endregion
}




Console.ReadLine();
//

//An extension property exists with the name extension_fe5a97a983bb46a89ec73ed12408cd77_PartnerName.