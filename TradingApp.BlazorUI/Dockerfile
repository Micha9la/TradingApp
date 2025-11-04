# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Stage 0: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore dependencies
COPY TradingApp.BlazorUI/TradingApp.BlazorUI.csproj TradingApp.BlazorUI/
COPY TradingApp/TradingApp.csproj TradingApp/
RUN dotnet restore "TradingApp.BlazorUI/TradingApp.BlazorUI.csproj"

# Copy everything else
COPY . .

# Stage 2: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/TradingApp.BlazorUI
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published files from publish stage
COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "TradingApp.BlazorUI.dll"]


