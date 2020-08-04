FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./document-api.sln ./
COPY ./document-api/document-api.csproj ./document-api/
COPY ./document-api.Tests/document-api.Tests.csproj ./document-api.Tests/

RUN dotnet restore ./document-api/document-api.csproj
RUN dotnet restore ./document-api.Tests/document-api.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out document-api.Tests/document-api.Tests.csproj

CMD dotnet test
