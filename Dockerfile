#See https://aka.ms/customizecontainer to learn how to customize your debug container 
# and how VS uses this Dockerfile to build your images for faster debugging.

#C:\Repos\AZFunc1

FROM mcr.microsoft.com/azure-functions/dotnet:6.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AZFunc1.csproj", "."]
RUN dotnet restore "./AZFunc1.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AZFunc1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AZFunc1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true