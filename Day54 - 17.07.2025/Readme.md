
# Deploy Azure Container registry

- az login
- az group create --name rg-azure-learn --location eastus
- ACR_NAME=myacrregistry
- az acr create --resource-group rg-azure-learn --name $ACR_NAME --sku Premium
- inside the cloud shell, run the following command to login to the ACR
  - code Dockerfile
  - Paste the content of the Dockerfile
  - az acr build --registry $ACR_NAME --image myapp:latest .
  - az acr repository list --name $ACR_NAME --output table
  - az acr update --name $ACR_NAME --admin-enabled true
  - az acr credential show --name $ACR_NAME
  - az container create --resource-group rg-azure-learn --name acr-tasks --image $ACR_NAME.azurecr.io/helloacrtasks:v1 --registry-login-server $ACR_NAME.azurecr.io --ip-address Public --location eastus --registry-username <admin-username> --registry-password <admin-password> --os-type Linux --cpu 1 --memory 1
  - az container show --resource-group rg-azure-learn --name acr-tasks --query ipAddress.ip --output table
  - check that ip in the browser
- replication
  - az acr replication create --registry $ACR_NAME --location eastus
  - az acr replication list --registry $ACR_NAME --output table
- finally
  - az group delete --name rg-azure-learn g --yes --no-wait


# Create a private container registry and push an image from cmd
- az login
- az group create --name rg-azure-learn --location eastus
- ACR_NAME=myacrregistry
- az acr create --resource-group rg-azure-learn --name mycontainerregistry --sku Standard --role-assignment-mode 'rbac-abac' --dnl-scope TenantReuse
- az acr login --name mycontainerregistry
- docker pull mcr.microsoft.com/azuredocs/aci-helloworld:latest (in locally installed docker)
- docker tag mcr.microsoft.com/azuredocs/aci-helloworld:latest mycontainerregistry.azurecr.io/aci-helloworld:latest
- docker push mycontainerregistry.azurecr.io/aci-helloworld:latest
- az acr repository list --name mycontainerregistry --output table
- docer run mycontainerregistry.azurecr.io/aci-helloworld:latest
- az acr update --name mycontainerregistry --admin-enabled true
- az acr credential show --name mycontainerregistry
- az container create --resource-group rg-azure-learn --name acr-tasks --image mycontainerregistry.azurecr.io/aci-helloworld:latest --registry-login-server mycontainerregistry.azurecr.io --ip-address Public --location eastus --registry-username <admin-username> --registry-password <admin-password> --os-type Linux --cpu 1 --memory 1
- az container show --resource-group rg-azure-learn --name acr-tasks --query ipAddress.ip --output table
- check that ip in the browser
