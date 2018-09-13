FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ../GP.Microservices.Storage/GP.Microservices.Storage.csproj ../GP.Microservices.Storage/
RUN dotnet restore ../GP.Microservices.Storage/GP.Microservices.Storage.csproj
COPY . .
WORKDIR /src/../GP.Microservices.Storage
RUN dotnet build GP.Microservices.Storage.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GP.Microservices.Storage.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GP.Microservices.Storage.dll"]
