FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
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
