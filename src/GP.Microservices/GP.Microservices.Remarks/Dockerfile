FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ../GP.Microservices.Remarks/GP.Microservices.Remarks.csproj ../GP.Microservices.Remarks/
RUN dotnet restore ../GP.Microservices.Remarks/GP.Microservices.Remarks.csproj
COPY . .
WORKDIR /src/../GP.Microservices.Remarks
RUN dotnet build GP.Microservices.Remarks.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GP.Microservices.Remarks.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GP.Microservices.Remarks.dll"]
