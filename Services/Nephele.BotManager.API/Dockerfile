# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Nephele.BotManager.API/Nephele.BotManager.API.csproj", "Services/Nephele.BotManager.API/"]
COPY ["Infrastructure/Nephele.BotManager.Repository/Nephele.BotManager.Repository.csproj", "Infrastructure/Nephele.BotManager.Repository/"]
COPY ["Common/Nephele.BotManager.Extensions/Nephele.BotManager.Extensions.csproj", "Common/Nephele.BotManager.Extensions/"]
COPY ["Common/Nephele.BotManager.Settings/Nephele.BotManager.Settings.csproj", "Common/Nephele.BotManager.Settings/"]
COPY ["Infrastructure/Nephele.BotManager.Models/Nephele.BotManager.Models.csproj", "Infrastructure/Nephele.BotManager.Models/"]
COPY ["Infrastructure/Nephele.BotManager.Services/Nephele.BotManager.Services.csproj", "Infrastructure/Nephele.BotManager.Services/"]
RUN dotnet restore "./Services/Nephele.BotManager.API/Nephele.BotManager.API.csproj"
COPY . .
WORKDIR "/src/Services/Nephele.BotManager.API"
RUN dotnet build "./Nephele.BotManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Nephele.BotManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nephele.BotManager.API.dll"]