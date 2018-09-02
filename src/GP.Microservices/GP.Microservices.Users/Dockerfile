FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ../GP.Microservices.Users/GP.Microservices.Users.csproj ../GP.Microservices.Users/
RUN dotnet restore ../GP.Microservices.Users/GP.Microservices.Users.csproj
COPY . .
WORKDIR /src/../GP.Microservices.Users
RUN dotnet build GP.Microservices.Users.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GP.Microservices.Users.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GP.Microservices.Users.dll"]
