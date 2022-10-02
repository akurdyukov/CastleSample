FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["CastleSample.csproj", "."]
RUN dotnet restore "CastleSample.csproj"
COPY . .
RUN dotnet build "CastleSample.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CastleSample.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV DOTNET_URLS http://0.0.0.0:80
ENTRYPOINT ["dotnet", "CastleSample.dll"]
