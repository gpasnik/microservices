FROM microsoft/aspnetcore:2.0
WORKDIR /app
EXPOSE 80
COPY ./bin/Release/publish .
ENTRYPOINT ["dotnet", "GP.Microservices.Api.dll"]
