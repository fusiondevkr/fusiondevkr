param name string
param instanceName string
param location string = resourceGroup().location
param locationCode string = 'krc'

@allowed([
    'Free'
    'Standard'
])
param skuName string = 'Free'

@secure()
param ghAuthToken string
param ghRepoUrl string = 'https://github.com/fusiondevkr/fusiondevkr'
param ghRepoBranch string = 'main'
param ghRepoAppLocation string = 'src/Fdk.M365Register.WasmApp'
param ghRepoApiLocation string = 'src/Fdk.M365Register.ApiApp'
param ghRepoArtifactLocation string = 'wwwroot'

var metadata = {
    longName: '{0}-${name}-${instanceName}-${locationCode}'
    shortName: '{0}${name}${instanceName}${locationCode}'
}

var staticApp = {
    name: format(metadata.longName, 'sttapp')
    location: location
    skuName: skuName
    gh: {
        authToken: ghAuthToken
        repoUrl: ghRepoUrl
        branch: ghRepoBranch
        appLocation: ghRepoAppLocation
        apiLocation: ghRepoApiLocation
        artifactLocation: ghRepoArtifactLocation
    }
}

resource sttapp 'Microsoft.Web/staticSites@2020-12-01' = {
    name: staticApp.name
    location: staticApp.location
    sku: {
        name: staticApp.skuName
    }
    properties: {
        repositoryUrl: staticApp.gh.repoUrl
        branch: staticApp.gh.branch
        repositoryToken: staticApp.gh.authToken
        buildProperties: {
            appLocation: staticApp.gh.appLocation
            apiLocation: staticApp.gh.apiLocation
            outputLocation: staticApp.gh.artifactLocation
        }
    }
}

output id string = sttapp.id
output name string = sttapp.name
