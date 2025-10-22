# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Challenge.Devsu.Api/Challenge.Devsu.Api.csproj", "Challenge.Devsu.Api/"]
COPY ["Challenge.Devsu.Application/Challenge.Devsu.Application.csproj", "Challenge.Devsu.Application/"]
COPY ["Challenge.Devsu.Common/Challenge.Devsu.Common.csproj", "Challenge.Devsu.Common/"]
COPY ["Challenge.Devsu.Core/Challenge.Devsu.Core.csproj", "Challenge.Devsu.Core/"]
COPY ["Challenge.Devsu.Infrastructure/Challenge.Devsu.Infrastructure.csproj", "Challenge.Devsu.Infrastructure/"]
COPY ["Challenge.Devsu.Shared/Challenge.Devsu.Shared.csproj", "Challenge.Devsu.Shared/"]

RUN dotnet restore "./Challenge.Devsu.Api/Challenge.Devsu.Api.csproj"
COPY . .
WORKDIR "/src/Challenge.Devsu.Api"
RUN dotnet build "./Challenge.Devsu.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Challenge.Devsu.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Challenge.Devsu.Api.dll"]
