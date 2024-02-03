#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
RUN apk add --no-cache \
        icu-data-full \
        icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Planor.WebUI/Planor.WebUI.csproj", "Planor.WebUI/"]
COPY ["src/Planor.Application/Planor.Application.csproj", "Planor.Application/"]
COPY ["src/Planor.Domain/Planor.Domain.csproj", "Planor.Domain/"]
COPY ["src/Planor.Infrastructure/Planor.Infrastructure.csproj", "Planor.Infrastructure/"]
RUN dotnet restore "Planor.WebUI/Planor.WebUI.csproj"
COPY . .
WORKDIR "/src/src/Planor.WebUI"
RUN dotnet build "Planor.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planor.WebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://*:80
ENTRYPOINT ["dotnet", "Planor.WebUI.dll"]
