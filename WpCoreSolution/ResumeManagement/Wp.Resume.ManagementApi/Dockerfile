#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ResumeManagement/Wp.Resume.ManagementApi/Wp.Resume.ManagementApi.csproj", "ResumeManagement/Wp.Resume.ManagementApi/"]
RUN dotnet restore "ResumeManagement/Wp.Resume.ManagementApi/Wp.Resume.ManagementApi.csproj"
COPY . .
WORKDIR "/src/ResumeManagement/Wp.Resume.ManagementApi"
RUN dotnet build "Wp.Resume.ManagementApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wp.Resume.ManagementApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose ports
EXPOSE 5500/tcp
ENV ASPNETCORE_URLS http://*:5500

ENTRYPOINT ["dotnet", "Wp.Resume.ManagementApi.dll"]