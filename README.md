# Passenger Management System - Backend

A robust .NET 7 Web API designed for secure passenger record management, identity document verification, and administrative API key modulation.

## 🚀 Quick Start

### Prerequisites
- **.NET 7 SDK**
- **PostgreSQL** (Database)
- **MinIO** (Object storage for images)
- **Docker** (Optional, for containerized deployment)

### Setup & Installation
1.  **Clone the repository**:
    ```bash
    git clone [repository-url]
    cd interview_backend-main
    ```

2.  **Database Initialization**:
    Run the consolidated script in your PostgreSQL instance:
    ```bash
    psql -U [username] -d [database] -f Backend_Test/init.sql
    ```

3.  **Configuration**:
    Update `Backend_Test/appsettings.json` with your credentials:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=your_db;Username=postgres;Password=your_pass"
      },
      "Jwt": {
        "Key": "your_very_secret_key_minimum_32_chars",
        "Issuer": "Backend_Test",
        "Audience": "Backend_Test"
      },
      "Minio": {
        "Endpoint": "localhost:9000",
        "AccessKey": "minioadmin",
        "SecretKey": "minioadmin"
      }
    }
    ```

4.  **Run the application**:
    ```bash
    cd Backend_Test
    dotnet run
    ```

## ✨ Key Features

- **Layered Security**: Combines **JWT Bearer** authentication with secondary **API Key** validation.
- **Dapper ORM**: High-performance database operations with raw SQL flexibility.
- **Identity Management**: Separate workflows for standard Users and Administrators.
- **Object Storage**: Integrated with **MinIO** for resilient profile and document image handling.
- **Documentation**: Auto-generated Swagger UI and comprehensive Markdown reference.

## 🛠️ Tech Stack

- **Framework**: .NET 7 Web API
- **ORM**: Dapper (Fast & Lightweight)
- **Database**: PostgreSQL (via Npgsql)
- **Object Storage**: MinIO
- **Security**: JWT, BCrypt, API Key Middleware
- **Architecture**: Repository Pattern + Service Layer

## 📚 Documentation

Detailed documentation is available in the `Backend_Test/` directory:
- [API Reference](./Backend_Test/API_DOCUMENTATION.md) - Detailed endpoint definitions and request/response examples.
- [Database ERD](./Backend_Test/DATABASE_ERD.md) - Visual schema and relationship mappings.

## 🐳 Docker Deployment

To run the system using Docker:
```bash
docker build -t passenger-backend -f Backend_Test/Dockerfile .
docker run -p 5000:80 passenger-backend
```

## 🤝 Contributing
1. Create a feature branch
2. Commit your changes (use conventional commits)
3. Submit a Pull Request

---
**License**: Private / Proprietary
