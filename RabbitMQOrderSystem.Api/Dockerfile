FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RabbitMQOrderSystem.Api/RabbitMQOrderSystem.Api.csproj", "RabbitMQOrderSystem.Api/"]
COPY ["RabbitMQOrderSystem.Application/RabbitMQOrderSystem.Application.csproj", "RabbitMQOrderSystem.Application/"]
COPY ["RabbitMQOrderSystem.Domain/RabbitMQOrderSystem.Domain.csproj", "RabbitMQOrderSystem.Domain/"]
COPY ["RabbitMQOrderSystem.Infrastructure/RabbitMQOrderSystem.Infrastructure.csproj", "RabbitMQOrderSystem.Infrastructure/"]
COPY ["RabbitMQOrderSystem.Worker/RabbitMQOrderSystem.Worker.csproj", "RabbitMQOrderSystem.Worker/"]

RUN dotnet restore "RabbitMQOrderSystem.Api/RabbitMQOrderSystem.Api.csproj"

COPY . .
WORKDIR "/src/RabbitMQOrderSystem.Api"
RUN dotnet build "RabbitMQOrderSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQOrderSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQOrderSystem.Api.dll"]