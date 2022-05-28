#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.5-alpine3.15-arm32v7 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0.300-alpine3.15-arm32v7 AS build
WORKDIR /src
COPY ["MB.EventResults.Blazor/Server/MB.EventResults.Blazor.Server.csproj", "MB.EventResults.Blazor/Server/"]
COPY ["MB.EventResults.Blazor/Client/MB.EventResults.Blazor.Client.csproj", "MB.EventResults.Blazor/Client/"]
COPY ["MB.EventResults.Blazor/Shared/MB.EventResults.Blazor.Shared.csproj", "MB.EventResults.Blazor/Shared/"]
COPY ["MB.EventResults.Blazor/MB.OResults.Core/MB.OResults.Core.csproj", "MB.EventResults.Blazor/MB.OResults.Core/"]
RUN dotnet restore "MB.EventResults.Blazor/Server/MB.EventResults.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/MB.EventResults.Blazor/Server"
RUN dotnet build "MB.EventResults.Blazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MB.EventResults.Blazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MB.EventResults.Blazor.Server.dll"]