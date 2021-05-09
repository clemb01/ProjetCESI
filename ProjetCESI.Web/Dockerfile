FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["ProjetCESI.Web/ProjetCESI.Web.csproj", "ProjetCESI.Web/"]
#COPY ["ProjetCESI.Metier/ProjetCESI.Metier.csproj", "ProjetCESI.Metier/"]
#COPY ["ProjetCESI.Data/ProjetCESI.Data.csproj", "ProjetCESI.Data/"]
#COPY ["ProjetCESI.Core/ProjetCESI.Core.csproj", "ProjetCESI.Core/"]
#RUN dotnet restore "ProjetCESI.Web/ProjetCESI.Web.csproj"
#COPY . .
#WORKDIR "/src/ProjetCESI.Web"
#RUN dotnet build "ProjetCESI.Web.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "ProjetCESI.Web.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "ProjetCESI.Web.dll"]
COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet ProjetCESI.Web.dll