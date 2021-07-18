param name string
param instanceName string
param location string = resourceGroup().location
param locationCode string = 'krc'

param faceApiSku string = 'F0'

var metadata = {
    longName: '{0}-${name}-${instanceName}-${locationCode}'
    shortName: '{0}${name}${instanceName}${locationCode}'
}

var faceapi = {
    name: format(metadata.longName, 'faceapi')
    location: location
    sku: faceApiSku
}

resource face 'Microsoft.CognitiveServices/accounts@2021-04-30' = {
    name: faceapi.name
    location: faceapi.location
    kind: 'Face'
    sku: {
        name: faceapi.sku
    }
    identity: {
        type: 'None'
    }
    properties: {
        customSubDomainName: faceapi.name
        networkAcls: {
            defaultAction: 'Allow'
        }
        publicNetworkAccess: 'Enabled'
    }
}

output id string = face.id
output name string = face.name
