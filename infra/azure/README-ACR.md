# Azure Container Registry (ACR) Deployment Guide

This guide will help you create and configure an Azure Container Registry for your Spotify Mixer application using Azure CLI and Bicep.

## 📋 Prerequisites

Before starting, ensure you have the following installed:

1. **Azure CLI** - [Download here](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
2. **Docker Desktop** - [Download here](https://www.docker.com/products/docker-desktop)
3. **PowerShell** (Windows) or **Bash** (Linux/Mac)

## 🚀 Quick Start

### Step 1: Login to Azure

```powershell
az login
```

### Step 2: Deploy ACR

Deploy using Azure CLI:

```powershell
az deployment group create \
  --resource-group "spotify-mixer-rg" \
  --template-file "acr.bicep" \
  --parameters "acr.parameters.json"
```

### Step 3: Login to ACR

```powershell
az acr login --name "spotifymixeracr"
```

### Step 4: Build and Push Your Image

```powershell
# Build the image
docker build -t spotifymixeracr.azurecr.io/spotify-mixer:latest .

# Push to ACR
docker push spotifymixeracr.azurecr.io/spotify-mixer:latest
```

### Step 5: Pull and Run Your Image

```powershell
# Pull from ACR
docker pull spotifymixeracr.azurecr.io/spotify-mixer:latest

# Run the container
docker run -d -p 8080:80 spotifymixeracr.azurecr.io/spotify-mixer:latest
```

## 📁 Files Overview

### `acr.bicep`
- **Purpose**: Infrastructure as Code template for ACR
- **Features**: 
  - Secure configuration with encryption
  - Configurable SKU (Basic, Standard, Premium)
  - Network access controls
  - Retention policies

### `acr.parameters.json`
- **Purpose**: Parameter values for the Bicep template
- **Customizable**: Update values for your environment



## 🔧 Configuration Options

### ACR SKU Comparison

| SKU | Storage | Network Rules | Geo-replication | Cost |
|-----|---------|---------------|-----------------|------|
| Basic | 10 GB | ❌ | ❌ | **FREE** (12 months) |
| Standard | 100 GB | ✅ | ❌ | $0.167/day |
| Premium | 500 GB | ✅ | ✅ | $0.50/day |

### Security Features

- **Encryption**: All data encrypted at rest
- **Network Rules**: Control access by IP/subnet
- **Admin User**: Disabled by default (use Azure AD instead)
- **Anonymous Pull**: Disabled for security

## 🛠️ Manual Deployment Steps

If you prefer to deploy manually:

### 1. Create Resource Group

```powershell
az group create --name "spotify-mixer-rg" --location "East US"
```

### 2. Deploy ACR

```powershell
az deployment group create \
  --resource-group "spotify-mixer-rg" \
  --template-file "acr.bicep" \
  --parameters "acr.parameters.json"
```

### 3. Get ACR Details

```powershell
az acr show --name "spotifymixeracr" --resource-group "spotify-mixer-rg"
```

## 🔐 Security Best Practices

### 1. Use Azure AD Authentication

Instead of admin credentials, use Azure AD:

```powershell
# Assign AcrPush role to your user
az role assignment create \
  --assignee "your-email@domain.com" \
  --role "AcrPush" \
  --scope "/subscriptions/{subscription-id}/resourceGroups/spotify-mixer-rg/providers/Microsoft.ContainerRegistry/registries/spotifymixeracr"
```

### 2. Enable Network Rules

```powershell
# Add your IP to allowed list
az acr network-rule add \
  --name "spotifymixeracr" \
  --ip-address "YOUR_IP_ADDRESS"
```

### 3. Enable Content Trust

```powershell
az acr repository show-tags --name "spotifymixeracr" --repository "spotify-mixer"
```

## 📊 Monitoring and Management

### View ACR Metrics

```powershell
# Get ACR usage
az acr show-usage --name "spotifymixeracr"

# List repositories
az acr repository list --name "spotifymixeracr"

# List tags
az acr repository show-tags --name "spotifymixeracr" --repository "spotify-mixer"
```

### Clean Up Old Images

```powershell
# Delete old tags (keep last 5)
az acr repository delete \
  --name "spotifymixeracr" \
  --image "spotify-mixer:old-tag"
```

## 🚨 Troubleshooting

### Common Issues

1. **Authentication Failed**
   ```powershell
   # Re-login to ACR
   az acr login --name "spotifymixeracr"
   ```

2. **Push Failed**
   ```powershell
   # Check if you have push permissions
   az role assignment list --assignee "your-email@domain.com" --scope "/subscriptions/{subscription-id}/resourceGroups/spotify-mixer-rg/providers/Microsoft.ContainerRegistry/registries/spotifymixeracr"
   ```

3. **Network Access Denied**
   ```powershell
   # Add your IP to allowed list
   az acr network-rule add --name "spotifymixeracr" --ip-address "YOUR_IP_ADDRESS"
   ```

## 💰 Cost Optimization

### Choose the Right SKU
- **Development**: Use Basic SKU
- **Production**: Use Standard or Premium SKU

### Enable Retention Policies
```powershell
# Set retention policy to 7 days
az acr config retention update \
  --name "spotifymixeracr" \
  --days 7 \
  --status enabled
```

## 📞 Support

If you encounter issues:

1. Check Azure CLI version: `az version`
2. Check Bicep version: `bicep --version`
3. Verify Azure subscription: `az account show`
4. Check ACR status: `az acr show --name "spotifymixeracr"`

## 🔗 Useful Links

- [Azure Container Registry Documentation](https://docs.microsoft.com/en-us/azure/container-registry/)
- [Bicep Documentation](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/)
- [Azure CLI Documentation](https://docs.microsoft.com/en-us/cli/azure/) 