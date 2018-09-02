FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ../GP.Microservices.Api/GP.Microservices.Api.csproj ../GP.Microservices.Api/
RUN dotnet restore ../GP.Microservices.Api/GP.Microservices.Api.csproj
COPY . .
WORKDIR /src/../GP.Microservices.Api
RUN dotnet build GP.Microservices.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GP.Microservices.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GP.Microservices.Api.dll"]
