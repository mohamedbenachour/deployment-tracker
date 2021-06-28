FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /built-app
COPY built-app/ ./

ENTRYPOINT ["dotnet", "DeploymentTrackerCore.dll"]