#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Project files
COPY ["MoodTracker.Api/MoodTracker.Api.csproj", "MoodTracker.Api/"]
COPY ["MoodTracker.Core/MoodTracker.Core.csproj", "MoodTracker.Core/"]
COPY ["MoodTracker.Data/MoodTracker.Data.csproj", "MoodTracker.Data/"]

# Restore dependencies
RUN dotnet restore "MoodTracker.Api/MoodTracker.Api.csproj"

# Copy source code
COPY . .

# Build
WORKDIR "/src/MoodTracker.Api"
RUN dotnet build "MoodTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MoodTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoodTracker.Api.dll"]