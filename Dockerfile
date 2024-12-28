FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/SpaceReservation.Api/SpaceReservation.Api.csproj", "SpaceReservation.Api/"]
COPY ["src/SpaceReservation.Application/SpaceReservation.Application.csproj", "SpaceReservation.Application/"]
COPY ["src/SpaceReservation.Domain/SpaceReservation.Domain.csproj", "SpaceReservation.Domain/"]
COPY ["src/SpaceReservation.Infrastructure/SpaceReservation.Infrastructure.csproj", "SpaceReservation.Infrastructure/"]
RUN dotnet restore "SpaceReservation.Api/SpaceReservation.Api.csproj"

# Copy the rest of the code
COPY src/ .

# Build the application
RUN dotnet build "SpaceReservation.Api/SpaceReservation.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "SpaceReservation.Api/SpaceReservation.Api.csproj" -c Release -o /app/publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SpaceReservation.Api.dll"] 