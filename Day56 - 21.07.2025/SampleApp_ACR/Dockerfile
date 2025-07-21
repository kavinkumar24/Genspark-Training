# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "SampleApp_ACR.dll"]


# docker push mycontainerregistrykavin-dggfbxdpbahtb6fw.azurecr.io/onlineauction


# az container create \                                                                                          
#   --resource-group rg-azure-learn \
#   --name saichatapp \
#   --image mycontainerregistrykavin-dggfbxdpbahtb6fw.azurecr.io/onlineauction:latest \
#   --registry-login-server mycontainerregistrykavin-dggfbxdpbahtb6fw.azurecr.io \
#   --registry-username mycontainerregistrykavin \
#   --registry-password eFE/UBThyLOQ++iW4v9rLEyi4Qe3KB/PjkyIehqOu9+ACRCqdXg4 \
#   --dns-name-label onlineauction1234 \
#   --ports 80 \
# --os-type Linux \
# --cpu 1 \
# --memory 0.5




# az container create \
#   --resource-group rg-azure-kavin \
#   --name kavinapp \
#   --image mycontainerregistrykavin-dggfbxdpbahtb6fw.azurecr.io/onlineauction:latest \
#   --registry-login-server mycontainerregistrykavin-dggfbxdpbahtb6fw.azurecr.io \
#   --registry-username $REGISTRY_USER \
#   --registry-password $REGISTRY_PASS \
#   --dns-name-label onlineauction1234 \
#   --ports 80 \
#   --os-type Linux \
#   --cpu 1 \
#   --memory 0.5


