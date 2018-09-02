FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ../GP.Microservices.Statistics/GP.Microservices.Statistics.csproj ../GP.Microservices.Statistics/
RUN dotnet restore ../GP.Microservices.Statistics/GP.Microservices.Statistics.csproj
COPY . .
WORKDIR /src/../GP.Microservices.Statistics
RUN dotnet build GP.Microservices.Statistics.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GP.Microservices.Statistics.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GP.Microservices.Statistics.dll"]
