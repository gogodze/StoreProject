FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/Presentation/Presentation.csproj", "Presentation/"]

RUN dotnet restore "Domain/Domain.csproj"
RUN dotnet restore "Application/Application.csproj"
RUN dotnet restore "Infrastructure/Infrastructure.csproj"
RUN dotnet restore "Presentation/Presentation.csproj"

COPY . .
RUN dotnet build "Domain/Domain.csproj" -c $BUILD_CONFIGURATION --no-restore
RUN dotnet build "Application/Application.csproj" -c $BUILD_CONFIGURATION --no-restore
RUN dotnet build "Infrastructure/Infrastructure.csproj" -c $BUILD_CONFIGURATION --no-restore
RUN dotnet build "Presentation/Presentation.csproj" -c $BUILD_CONFIGURATION --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Presentation/Presentation.csproj" --no-build --no-restore -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]