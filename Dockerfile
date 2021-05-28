FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

COPY --from=publish /src/publish .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet ProjetCESI.Web.dll