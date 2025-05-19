# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PosTech.Contacts.Worker/PosTech.Contacts.Worker.csproj", "src/PosTech.Contacts.Worker/"]
COPY ["src/PosTech.Contacts.ApplicationCore/PosTech.Contacts.ApplicationCore.csproj", "src/PosTech.Contacts.ApplicationCore/"]
COPY ["src/PosTech.Contacts.Infrastructure/PosTech.Contacts.Infrastructure.csproj", "src/PosTech.Contacts.Infrastructure/"]
RUN dotnet restore "./src/PosTech.Contacts.Worker/PosTech.Contacts.Worker.csproj"
COPY . .
WORKDIR "/src/src/PosTech.Contacts.Worker"
RUN dotnet build "./PosTech.Contacts.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PosTech.Contacts.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PosTech.Contacts.Worker.dll"]