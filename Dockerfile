FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SkillUpHub.Auth/SkillUpHub.Auth.API/SkillUpHub.Auth.API.csproj", "SkillUpHub.Auth.API/"]
COPY ["SkillUpHub.Auth/SkillUpHub.Auth.Application/SkillUpHub.Command.Application.csproj", "SkillUpHub.Auth.Application/"]
COPY ["SkillUpHub.Auth/SkillUpHub.Auth.Contract/SkillUpHub.Command.Contract.csproj", "SkillUpHub.Auth.Contract/"]
COPY ["SkillUpHub.Auth/SkillUpHub.Auth.Infrastructure/SkillUpHub.Command.Infrastructure.csproj", "SkillUpHub.Auth.Infrastructure/"]
RUN dotnet restore "SkillUpHub.Auth.API/SkillUpHub.Auth.API.csproj"
COPY ./SkillUpHub.Auth/ .
WORKDIR "/src/SkillUpHub.Auth.API"
RUN dotnet build "SkillUpHub.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SkillUpHub.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkillUpHub.Auth.API.dll"]
