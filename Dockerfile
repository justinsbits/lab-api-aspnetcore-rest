#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["lab-api-aspnetcore-rest/CommanderREST.csproj", "lab-api-aspnetcore-rest/"]
COPY ["lab-data-dotnetcore-ef/CommanderData.csproj", "lab-data-dotnetcore-ef/"]
RUN dotnet restore "lab-api-aspnetcore-rest/CommanderREST.csproj"
COPY . .
WORKDIR "/src/lab-api-aspnetcore-rest"
RUN dotnet build "CommanderREST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommanderREST.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommanderREST.dll"]