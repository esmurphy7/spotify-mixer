// Azure Container Registry (ACR) Bicep Template
// This template creates a secure ACR with proper RBAC and networking

@description('The name of the Azure Container Registry')
param acrName string

@description('The location for the ACR')
param location string = resourceGroup().location

@description('The SKU for the ACR (Basic, Standard, Premium)')
@allowed(['Basic', 'Standard', 'Premium'])
param sku string = 'Basic'

@description('Enable admin user for ACR (not recommended for production)')
param enableAdminUser bool = false

@description('Enable public network access')
param publicNetworkAccess string = 'Enabled'

@description('Enable anonymous pull access')
param anonymousPullEnabled bool = false

@description('Enable data endpoint for ACR')
param dataEndpointEnabled bool = false

@description('Enable retention policy for automatic cleanup')
param enableRetentionPolicy bool = true

@description('Number of days to retain images (1-365)')
param retentionDays int = 7

@description('Tags for the ACR resource')
param tags object = {}

// Azure Container Registry
resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: acrName
  location: location
  sku: {
    name: sku
  }
  properties: {
    adminUserEnabled: enableAdminUser
    publicNetworkAccess: publicNetworkAccess
    networkRuleBypassOptions: 'AzureServices'
    encryption: {
      status: 'disabled'
    }
  }
  tags: tags
}

// Output the ACR login server
output acrLoginServer string = acr.properties.loginServer

// Output the ACR name
output acrName string = acr.name

// Output the ACR resource ID
output acrResourceId string = acr.id

// Output the admin credentials (if enabled)
output adminUsername string = enableAdminUser ? acr.listCredentials().username : ''

// Output the admin password (if enabled)
output adminPassword string = enableAdminUser ? acr.listCredentials().passwords[0].value : '' 