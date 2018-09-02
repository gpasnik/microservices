FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
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
