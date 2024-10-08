﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SkillUpHub.Auth.API/SkillUpHub.Auth.API.csproj", "SkillUpHub.Auth.API/"]
COPY ["SkillUpHub.Auth.Application/SkillUpHub.Auth.Application.csproj", "SkillUpHub.Auth.Application/"]
COPY ["SkillUpHub.Auth.Contract/SkillUpHub.Auth.Contract.csproj", "SkillUpHub.Auth.Contract/"]
COPY ["SkillUpHub.Auth.Infrastructure/SkillUpHub.Auth.Infrastructure.csproj", "SkillUpHub.Auth.Infrastructure/"]
RUN dotnet restore "SkillUpHub.Auth.API/SkillUpHub.Auth.API.csproj"
COPY . .
WORKDIR "/src/SkillUpHub.Auth.API"
RUN dotnet build "SkillUpHub.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SkillUpHub.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkillUpHub.Auth.API.dll"]
