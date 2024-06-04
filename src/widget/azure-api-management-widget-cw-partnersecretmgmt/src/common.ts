import { useSecrets } from "./hooks"

export interface AppReg {
    clientId: string;
    secretId: string;
    displayName: string;
    clientSecret: string;
    tenantId: string;
    startDateTime: Date;
    endDateTime: Date;    
} 

export interface SecretContext {    
    managementApiUrl: string | undefined;
    apiVersion: string | undefined;
    token: any;
    userId: any;
    hostname: any;
    origin: any;
}

export const Config = {
    backendUrl: "https://b2c-backend.wittyfield-afb7ce64.westeurope.azurecontainerapps.io/api"
}

export const Utility = {
    convert: (dateValue: Date | undefined) => {
        if (dateValue === undefined) {
            return "";
        }
        try {
            var parsedDate = new Date(dateValue);
            return parsedDate.toLocaleString();
        }
        catch(er) {
            console.log(er);
        }
        return "";
    }
}


export const Backend = {
    list: async (path: string, context: SecretContext ) => {        
        const fqdn = `${Config.backendUrl}${path}`;
        const response = await fetch( fqdn , 
            {
                headers: {
                    Authorization: context.token,
                    "Content-Type": "application/json",
                    "xmh-apiVersion": `${context.apiVersion}`,
                    "xmh-hostName": `${context.hostname}`,
                    "xmh-managementApiUrl": `${context.managementApiUrl}`, 
                    "xmh-origin": `${context.origin}`,
                    "xmh-userId": `${context.userId}`                    
                }
            });        
        return response.json();
    },
    generate: async (path: string, context: SecretContext, description: string) => {
        const fqdn = `${Config.backendUrl}${path}`;
        const response = await fetch( fqdn , {
            method: "POST",
            headers: {
                Authorization: context.token, 
                "Content-Type": "application/json",
                "xmh-apiVersion": `${context.apiVersion}`,
                "xmh-hostName": `${context.hostname}`,
                "xmh-managementApiUrl": `${context.managementApiUrl}`, 
                "xmh-origin": `${context.origin}`,
                "xmh-userId": `${context.userId}`
            },
            body: JSON.stringify({displayName: description})
        });
        return response.json();
    }
}