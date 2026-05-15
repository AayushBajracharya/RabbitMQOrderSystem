# RabbitMQ + Redis + EF Core Order System

A mid-level .NET example demonstrating Clean Architecture with modern backend tools.

## Tech Stack
- **.NET 8** + Clean Architecture
- **RabbitMQ** (MassTransit) - Message Queue
- **Redis** - Distributed Caching
- **PostgreSQL** + EF Core - Database
- **Docker** - Infrastructure

## Architecture Layers
- **Domain** → Entities & Events
- **Application** → Interfaces & Business Logic
- **Infrastructure** → RabbitMQ, Redis, EF Core implementations
- **API** → REST endpoints
- **Worker** → Background consumers

## Main Patterns Used
- Event-Driven Architecture
- Cache-Aside Pattern
- Repository Pattern
- Publish/Subscribe with RabbitMQ

## How to Run

1. Start infrastructure:
   ```bash
   docker compose -f docker/rabbitmq/docker-compose.yml up -d
