# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.csproj ./
# COPY . .

RUN dotnet restore

COPY . ./
RUN dotnet build /app/GenpactTest.csproj --configuration Release

FROM build AS testrunner

# Install Playwright via the built project
RUN pwsh -Command "pwsh /app/GenpactTest/bin/Release/net8.0/playwright.ps1 install --with-deps"

CMD ["dotnet", "test", "/app/GenpactTest/GenpactTest.csproj", "--logger:trx"]
