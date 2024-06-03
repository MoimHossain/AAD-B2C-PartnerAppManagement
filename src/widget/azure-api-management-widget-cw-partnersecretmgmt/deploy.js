const {deployNodeJS} = require("@azure/api-management-custom-widgets-tools")

const serviceInformation = {
	"resourceId": "subscriptions/7f2413b7-93b1-4560-a932-220c34c9db29/resourceGroups/api-management-demo/providers/Microsoft.ApiManagement/service/celeste-apim",
	"managementApiEndpoint": "https://management.azure.com",
	"apiVersion": "2022-08-01"
}
const name = "cw-partnersecretmgmt"
const fallbackConfigPath = "./static/config.msapim.json"
const config = {
	"interactiveBrowserCredentialOptions": {
		"redirectUri": "http://localhost:1337",
		"tenantId": "16b3c013-d300-468d-ac64-7eda0820b6d3"
	}
}

deployNodeJS(serviceInformation, name, fallbackConfigPath, config)
