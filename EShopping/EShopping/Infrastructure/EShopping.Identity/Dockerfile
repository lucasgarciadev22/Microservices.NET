
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Infrastructure/EShopping.Identity/EShopping.Identity.csproj", "Infrastructure/EShopping.Identity/"]
RUN dotnet restore "./Infrastructure/EShopping.Identity/EShopping.Identity.csproj"
COPY . .
WORKDIR "/src/Infrastructure/EShopping.Identity"
RUN dotnet build "./EShopping.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EShopping.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopping.Identity.dll"]