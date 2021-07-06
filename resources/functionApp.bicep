param name string
param location string = resourceGroup().location
param locationCode string = 'krc'

param storageAccountId string
param storageAccountName string
param appInsightsId string
param consumptionPlanId string

param functionAppTimezone string = 'Korea Standard Time'

param openApiVersion string = 'v2'
param openApiDocVersion string = 'v1.0.0'
param openApiDocTitle string = 'Fusion Dev Korea App Interface'

var metadata = {
    longName: '{0}-${name}-${locationCode}'
    shortName: '{0}${name}${locationCode}'
}

var storage = {
    id: storageAccountId
    name: storageAccountName
}
var consumption = {
    id: consumptionPlanId
}
var appInsights = {
    id: appInsightsId
}
var functionApp = {
    name: format(metadata.longName, 'fncapp')
    location: location
    timezone: functionAppTimezone
    openapi: {
        version: openApiVersion
        docVersion: openApiDocVersion
        docTitle: openApiDocTitle
    }
}

resource fncapp 'Microsoft.Web/sites@2020-12-01' = {
    name: functionApp.name
    location: functionApp.location
    kind: 'functionapp'
    properties: {
        serverFarmId: consumption.id
        httpsOnly: true
        siteConfig: {
            cors: {
                allowedOrigins: [
                    'https://functions.azure.com'
                    'https://functions-staging.azure.com'
                    'https://functions-next.azure.com'
                    'https://flow.microsoft.com'
                    'https://asia.flow.microsoft.com'
                ]
            }
            appSettings: [
                // Common Settings
                {
                    name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
                    value: '${reference(appInsights.id, '2020-02-02-preview', 'Full').properties.InstrumentationKey}'
                }
                {
                    name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
                    value: '${reference(appInsights.id, '2020-02-02-preview', 'Full').properties.connectionString}'
                }
                {
                    name: 'AzureWebJobsStorage'
                    value: 'DefaultEndpointsProtocol=https;AccountName=${storage.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storage.id, '2021-02-01').keys[0].value}'
                }
                {
                    name: 'FUNCTIONS_EXTENSION_VERSION'
                    value: '~3'
                }
                {
                    name: 'FUNCTION_APP_EDIT_MODE'
                    value: 'readonly'
                }
                {
                    name: 'FUNCTIONS_WORKER_RUNTIME'
                    value: 'dotnet'
                }
                {
                    name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
                    value: 'DefaultEndpointsProtocol=https;AccountName=${storage.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storage.id, '2021-02-01').keys[0].value}'
                }
                {
                    name: 'WEBSITE_CONTENTSHARE'
                    value: functionApp.name
                }
                {
                    name: 'WEBSITE_TIME_ZONE'
                    value: functionApp.timezone
                }
                // OpenAPI Settings
                {
                    name:  'OpenApi__Version'
                    value: functionApp.openapi.version
                }
                {
                    name:  'OpenApi__DocVersion'
                    value: functionApp.openapi.docVersion
                }
                {
                    name:  'OpenApi__DocTitle'
                    value: functionApp.openapi.docTitle
                }
            ]
        }
    }
}

output id string = fncapp.id
output name string = fncapp.name
