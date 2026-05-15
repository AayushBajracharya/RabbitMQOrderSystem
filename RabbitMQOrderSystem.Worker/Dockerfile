FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RabbitMQOrderSystem.Worker/RabbitMQOrderSystem.Worker.csproj", "RabbitMQOrderSystem.Worker/"]
COPY ["RabbitMQOrderSystem.Application/RabbitMQOrderSystem.Application.csproj", "RabbitMQOrderSystem.Application/"]
COPY ["RabbitMQOrderSystem.Domain/RabbitMQOrderSystem.Domain.csproj", "RabbitMQOrderSystem.Domain/"]
COPY ["RabbitMQOrderSystem.Infrastructure/RabbitMQOrderSystem.Infrastructure.csproj", "RabbitMQOrderSystem.Infrastructure/"]

RUN dotnet restore "RabbitMQOrderSystem.Worker/RabbitMQOrderSystem.Worker.csproj"

COPY . .
WORKDIR "/src/RabbitMQOrderSystem.Worker"
RUN dotnet build "RabbitMQOrderSystem.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQOrderSystem.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQOrderSystem.Worker.dll"]