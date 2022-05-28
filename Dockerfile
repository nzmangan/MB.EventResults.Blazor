#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./MB.EventResults.Blazor/Server/MB.EventResults.Blazor.Server.csproj", "Server/"]
COPY ["./MB.EventResults.Blazor/Client/MB.EventResults.Blazor.Client.csproj", "Client/"]
COPY ["./MB.EventResults.Blazor/Shared/MB.EventResults.Blazor.Shared.csproj", "Shared/"]
COPY ["./MB.EventResults.Blazor/MB.OResults.Core/MB.OResults.Core.csproj", "MB.OResults.Core/"]
RUN dotnet restore "Server/MB.EventResults.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "MB.EventResults.Blazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MB.EventResults.Blazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MB.EventResults.Blazor.Server.dll"]