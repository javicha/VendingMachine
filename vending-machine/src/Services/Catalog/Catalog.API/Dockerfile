#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Catalog/Catalog.API/Vending.API.csproj", "src/Services/Catalog/Catalog.API/"]
COPY ["src/Common/EventBus.Messages/EventBus.Messages.csproj", "src/Common/EventBus.Messages/"]
COPY ["src/Services/Catalog/Catalog.Application/Vending.Application.csproj", "src/Services/Catalog/Catalog.Application/"]
COPY ["src/Services/Catalog/Catalog.Domain/Vending.Domain.csproj", "src/Services/Catalog/Catalog.Domain/"]
COPY ["src/Common/Domain/Domain.csproj", "src/Common/Domain/"]
COPY ["src/Common/Application/Application/Application.csproj", "src/Common/Application/Application/"]
COPY ["src/Services/Catalog/Catalog.Infrastructure/Vending.Infrastructure.csproj", "src/Services/Catalog/Catalog.Infrastructure/"]
RUN dotnet restore "src/Services/Catalog/Catalog.API/Vending.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Catalog/Catalog.API"
RUN dotnet build "Vending.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vending.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vending.API.dll"]