#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Basket.API/Basket.API.csproj", "Services/Basket.API/"]
COPY ["Services/Basket.API/Basket.Application.csproj", "Services/Basket.Application/"]
COPY ["Services/Basket.API/Basket.Core.csproj", "Services/Basket.Core/"]
COPY ["Services/Basket.API/Basket.Infrastructure.csproj", "Services/Basket.Infrastructure/"]
RUN dotnet restore "Services/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/Services/Basket.API"
RUN dotnet build "Basket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]