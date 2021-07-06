param name string
param location string = resourceGroup().location
param locationCode string = 'krc'

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
param openApiDocTitle string = 'GymLogs Publisher Interface'

module st './storageAccount.bicep' = if (storageAccountToProvision) {
    name: 'StorageAccount'
    params: {
        name: name
        location: location
        locationCode: locationCode
        storageAccountSku: storageAccountSku
    }
}

module wrkspc './logAnalyticsWorkspace.bicep' = if (workspaceToProvision) {
    name: 'LogAnalyticsWorkspace'
    params: {
        name: name
        location: location
        locationCode: locationCode
        workspaceSku: workspaceSku
    }
}

module appins './appInsights.bicep' = if (appInsightsToProvision) {
    name: 'ApplicationInsights'
    params: {
        name: name
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
