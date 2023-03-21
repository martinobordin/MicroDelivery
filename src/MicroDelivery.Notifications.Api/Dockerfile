#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/MicroDelivery.Notifications.Api/MicroDelivery.Notifications.Api.csproj", "src/MicroDelivery.Notifications.Api/"]
COPY ["src/MicroDelivery.Shared/MicroDelivery.Shared.csproj", "src/MicroDelivery.Shared/"]
RUN dotnet restore "src/MicroDelivery.Notifications.Api/MicroDelivery.Notifications.Api.csproj"
COPY . .
WORKDIR "/src/src/MicroDelivery.Notifications.Api"
RUN dotnet build "MicroDelivery.Notifications.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroDelivery.Notifications.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroDelivery.Notifications.Api.dll"]