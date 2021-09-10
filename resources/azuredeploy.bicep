param name string
param instanceName string
param location string = resourceGroup().location
param locationCode string = 'krc'

// Face API
param faceApiSkuToProvision bool = false
param faceApiSku string = 'F0'

// Storage
param storageAccountToProvision bool = true
param storageAccountSku string = 'Standard_LRS'

// Log Analytics Workspace
param workspaceToProvision bool = true
param workspaceSku string = 'PerGB2018'

// Application Insights
param appInsightsToProvision bool = true

// Consumption Plan
param consumptionPlanToProvision bool = true
param isLinux bool = false

// Function App
param functionAppToProvision bool = true
param functionAppTimezone string = 'Korea Standard Time'
param openApiVersion string = 'v3'
param openApiDocVersion string = 'v1.0.0'
param openApiDocTitle string = 'Fusion Dev Korea App Interface'

// Static App
param staticAppToProvision bool = true

@secure()
param ghAuthToken string = ''

module face './faceapi.bicep' = if (faceApiSkuToProvision) {
    name: 'FaceApi'
    params: {
        name: name
        instanceName: instanceName
        location: location
        locationCode: locationCode
        faceApiSku: faceApiSku
    }
}

module st './storageAccount.bicep' = if (storageAccountToProvision) {
    name: 'StorageAccount'
    params: {
        name: name
        instanceName: instanceName
        location: location
        locationCode: locationCode
        storageAccountSku: storageAccountSku
    }
}

module wrkspc './logAnalyticsWorkspace.bicep' = if (workspaceToProvision) {
    name: 'LogAnalyticsWorkspace'
    params: {
        name: name
        instanceName: instanceName
        location: location
        locationCode: locationCode
        workspaceSku: workspaceSku
    }
}

module appins './appInsights.bicep' = if (appInsightsToProvision) {
    name: 'ApplicationInsights'
    params: {
        name: name
        instanceName: instanceName
        location: location
        locationCode: locationCode
        workspaceId: wrkspc.outputs.id
    }
}

module csplan './consumptionPlan.bicep' = if (consumptionPlanToProvision) {
    name: 'ConsumptionPlan'
    params: {
        name: name
        location: location
        locationCode: locationCode
        isLinux: isLinux
    }
}

module fncapp './functionApp.bicep' = if (functionAppToProvision) {
    name: 'FunctionApp'
    params: {
        name: name
        instanceName: instanceName
        location: location
        locationCode: locationCode
        storageAccountId: st.outputs.id
        storageAccountName: st.outputs.name
        appInsightsId: appins.outputs.id
        consumptionPlanId: csplan.outputs.id
        functionAppTimezone: functionAppTimezone
        openApiVersion: openApiVersion
        openApiDocVersion: openApiDocVersion
        openApiDocTitle: openApiDocTitle
    }
}

module sttapp './staticWebApp.bicep' = if (staticAppToProvision) {
    name: 'StaticApp'
    params: {
        name: name
        instanceName: instanceName
        location: 'eastasia'
        locationCode: 'ea'
        ghAuthToken: ghAuthToken
    }
}
