# Space Reservation System - Backend API

## Description
RESTful API backend service for the Space Reservation System. Provides comprehensive endpoints for space management and reservation handling, supporting the frontend application with robust business logic and data persistence.

## Key Features
- 🔐 Secure API endpoints for space and reservation management
- 📊 Efficient data persistence with Entity Framework Core
- 🔄 Real-time availability checking and conflict prevention
- ⚡ High-performance API responses
- 🛡️ Input validation and error handling
- 📝 Comprehensive logging system

## Technical Requirements
- .NET 7.0 SDK or higher
- SQL Server (2019 or higher)
- Visual Studio 2022 or VS Code with C# extensions

## Installation

1. Clone the repository:
```bash
git clone [REPOSITORY_URL]
```

2. Navigate to the project directory:
```bash
cd SpaceReservation.Backend
```

3. Restore dependencies:
```bash
dotnet restore
```

4. Update the database:
```bash
dotnet ef database update
```

5. Run the application:
```bash
dotnet run --project src/SpaceReservation.Api
```

The API will be available at `https://localhost:7001`

## Project Structure
```
src/
├── SpaceReservation.Api/           # API endpoints and configuration
├── SpaceReservation.Application/   # Business logic and services
├── SpaceReservation.Domain/        # Domain entities and interfaces
└── SpaceReservation.Infrastructure/# Data access and external services
```

## API Endpoints

### Spaces
- GET /api/spaces - Get all spaces
- GET /api/spaces/{id} - Get space by ID
- POST /api/spaces - Create new space
- PUT /api/spaces/{id} - Update space
- DELETE /api/spaces/{id} - Delete space

### Reservations
- GET /api/reservations - Get all reservations
- GET /api/reservations/{id} - Get reservation by ID
- POST /api/reservations - Create new reservation
- PUT /api/reservations/{id} - Update reservation
- DELETE /api/reservations/{id} - Delete reservation

## Technologies Used
- ASP.NET Core 7.0
- Entity Framework Core
- SQL Server
- AutoMapper
- FluentValidation
- Swagger/OpenAPI

## Architecture
- Clean Architecture principles
- Domain-Driven Design (DDD) patterns
- CQRS pattern for separation of read and write operations
- Repository pattern for data access
- Dependency Injection for loose coupling

## Development
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Testing
Run the tests using:
```bash
dotnet test
```

## License
This project is licensed under the [LICENSE_TYPE].

## Related Projects
- [Space Reservation Frontend](FRONTEND_REPO_URL) 