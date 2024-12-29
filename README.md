# Sistema de Reserva de Espacios - API Backend

## Descripción
Servicio API RESTful para el Sistema de Reserva de Espacios. Proporciona endpoints completos para la gestión de espacios y manejo de reservas, apoyando la aplicación frontend con una robusta lógica de negocio y persistencia de datos.

## Características Principales
- 🔐 Endpoints API seguros para gestión de espacios y reservas
- 📊 Persistencia eficiente de datos con Entity Framework Core
- 🔄 Verificación de disponibilidad en tiempo real y prevención de conflictos
- ⚡ Respuestas API de alto rendimiento
- 🛡️ Validación de entrada y manejo de errores
- 📝 Sistema integral de registro de actividades

## Requisitos Técnicos
- .NET 9.0 SDK
- SQL Server (2019 o superior)
- Visual Studio 2022 o VS Code con extensiones C#

## Instalación

1. Clonar el repositorio:
```bash
git clone [REPOSITORY_URL]
```

2. Navegar al directorio del proyecto:
```bash
cd SpaceReservation.Backend
```

3. Restaurar dependencias:
```bash
dotnet restore
```

4. Actualizar la base de datos:
```bash
dotnet ef database update
```

5. Ejecutar la aplicación:
```bash
dotnet run --project src/SpaceReservation.Api
```

La API estará disponible en `https://localhost:7001`

## Estructura del Proyecto
```
SpaceReservation.Backend/
├── src/
│   ├── SpaceReservation.Api/           # Endpoints API y configuración
│   ├── SpaceReservation.Application/   # Lógica de negocio y servicios
│   ├── SpaceReservation.Domain/        # Entidades de dominio e interfaces
│   └── SpaceReservation.Infrastructure/# Acceso a datos y servicios externos
└── tests/
    └── SpaceReservation.Tests/         # Pruebas unitarias y de integración
```

## Endpoints API

### Espacios
- GET /api/spaces - Obtener todos los espacios
- GET /api/spaces/{id} - Obtener espacio por ID
- POST /api/spaces - Crear nuevo espacio
- PUT /api/spaces/{id} - Actualizar espacio
- DELETE /api/spaces/{id} - Eliminar espacio

### Reservas
- GET /api/reservations - Obtener todas las reservas
- GET /api/reservations/{id} - Obtener reserva por ID
- GET /api/reservations/space/{spaceId} - Obtener reservas por espacio
- GET /api/reservations/user/{cedula} - Obtener reservas por cédula
- POST /api/reservations - Crear nueva reserva
- DELETE /api/reservations/{id} - Eliminar reserva

## Tecnologías Utilizadas
- ASP.NET Core 9.0
- Entity Framework Core
- SQL Server
- AutoMapper
- FluentValidation
- Swagger/OpenAPI

## Arquitectura
- Principios de Arquitectura Limpia
- Patrones de Diseño Dirigido por Dominio (DDD)
- Patrón CQRS para separación de operaciones de lectura y escritura
- Patrón Repositorio para acceso a datos
- Inyección de Dependencias para bajo acoplamiento

## Pruebas
El proyecto incluye pruebas unitarias exhaustivas utilizando:
- xUnit como framework de pruebas
- Moq para simulación de dependencias
- FluentAssertions para aserciones más legibles

### Ejecutar las Pruebas

Para ejecutar todas las pruebas:
```bash
dotnet test
```

Para ejecutar pruebas con cobertura:
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
```

### Estructura de Pruebas
Las pruebas están organizadas por componente:
- `ReservationServiceTests.cs`: Pruebas para el servicio de reservas
  - Validación de creación de reservas
  - Manejo de conflictos de horarios
  - Operaciones CRUD
- `SpaceServiceTests.cs`: Pruebas para el servicio de espacios
  - Operaciones CRUD de espacios
  - Validaciones de negocio

## Desarrollo
1. Hacer fork del repositorio
2. Crear rama de funcionalidad (`git checkout -b feature/NuevaFuncionalidad`)
3. Confirmar cambios (`git commit -m 'Agregar nueva funcionalidad'`)
4. Subir la rama (`git push origin feature/NuevaFuncionalidad`)
5. Abrir un Pull Request

## Licencia
Este proyecto está licenciado bajo MIT License.

## Proyectos Relacionados
- [Frontend de Reserva de Espacios](FRONTEND_REPO_URL) 