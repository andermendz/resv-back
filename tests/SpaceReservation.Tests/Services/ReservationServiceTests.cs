using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SpaceReservation.Application.DTOs;
using SpaceReservation.Application.Services;
using SpaceReservation.Domain.Entities;
using SpaceReservation.Domain.Exceptions;
using SpaceReservation.Domain.Repositories;
using Xunit;

namespace SpaceReservation.Tests.Services;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<ISpaceRepository> _spaceRepositoryMock;
    private readonly ReservationService _sut;

    public ReservationServiceTests()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _spaceRepositoryMock = new Mock<ISpaceRepository>();
        _sut = new ReservationService(_reservationRepositoryMock.Object, _spaceRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateReservation_WithValidData_ShouldSucceed()
    {
        // Arrange
        var spaceId = 1;
        var cedula = "123456789";
        var startTime = DateTime.UtcNow.AddHours(1);
        var endTime = startTime.AddHours(2);
        
        var space = new Space("Test Space", "Test Description") { Id = spaceId };
        
        _spaceRepositoryMock
            .Setup(x => x.GetByIdAsync(spaceId))
            .ReturnsAsync(space);
            
        _reservationRepositoryMock
            .Setup(x => x.GetOverlappingReservationsAsync(spaceId, startTime, endTime))
            .ReturnsAsync(new List<Reservation>());
            
        _reservationRepositoryMock
            .Setup(x => x.GetUserOverlappingReservationsAsync(cedula, startTime, endTime))
            .ReturnsAsync(new List<Reservation>());

        // Act
        var result = await _sut.CreateReservationAsync(spaceId, cedula, startTime, endTime);

        // Assert
        result.Should().NotBeNull();
        result.SpaceId.Should().Be(spaceId);
        result.Cedula.Should().Be(cedula);
        result.StartTime.Should().Be(startTime);
        result.EndTime.Should().Be(endTime);
        
        _reservationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Reservation>()), Times.Once);
    }

    [Fact]
    public async Task CreateReservation_WithPastStartTime_ShouldThrowException()
    {
        // Arrange
        var spaceId = 1;
        var cedula = "123456789";
        var startTime = DateTime.UtcNow.AddHours(-1);
        var endTime = startTime.AddHours(2);

        // Act
        var act = () => _sut.CreateReservationAsync(spaceId, cedula, startTime, endTime);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Cannot create reservations in the past");
    }

    [Theory]
    [InlineData(15)] // 15 minutes - too short
    [InlineData(9 * 60)] // 9 hours - too long
    public async Task CreateReservation_WithInvalidDuration_ShouldThrowException(int durationInMinutes)
    {
        // Arrange
        var spaceId = 1;
        var cedula = "123456789";
        var startTime = DateTime.UtcNow.AddHours(1);
        var endTime = startTime.AddMinutes(durationInMinutes);

        // Act
        var act = () => _sut.CreateReservationAsync(spaceId, cedula, startTime, endTime);

        // Assert
        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task CreateReservation_WithOverlappingSpaceReservation_ShouldThrowException()
    {
        // Arrange
        var spaceId = 1;
        var cedula = "123456789";
        var startTime = DateTime.UtcNow.AddHours(1);
        var endTime = startTime.AddHours(2);
        
        var space = new Space("Test Space", "Test Description") { Id = spaceId };
        var existingReservation = new Reservation(space, "other-cedula", startTime, endTime);
        
        _spaceRepositoryMock
            .Setup(x => x.GetByIdAsync(spaceId))
            .ReturnsAsync(space);
            
        _reservationRepositoryMock
            .Setup(x => x.GetOverlappingReservationsAsync(spaceId, startTime, endTime))
            .ReturnsAsync(new List<Reservation> { existingReservation });

        // Act
        var act = () => _sut.CreateReservationAsync(spaceId, cedula, startTime, endTime);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("The space is already reserved for the selected time period");
    }

    [Fact]
    public async Task CreateReservation_WithOverlappingUserReservation_ShouldThrowException()
    {
        // Arrange
        var spaceId = 1;
        var cedula = "123456789";
        var startTime = DateTime.UtcNow.AddHours(1);
        var endTime = startTime.AddHours(2);
        
        var space = new Space("Test Space", "Test Description") { Id = spaceId };
        var otherSpace = new Space("Other Space", "Other Description") { Id = 2 };
        var existingReservation = new Reservation(otherSpace, cedula, startTime, endTime);
        
        _spaceRepositoryMock
            .Setup(x => x.GetByIdAsync(spaceId))
            .ReturnsAsync(space);
            
        _reservationRepositoryMock
            .Setup(x => x.GetOverlappingReservationsAsync(spaceId, startTime, endTime))
            .ReturnsAsync(new List<Reservation>());
            
        _reservationRepositoryMock
            .Setup(x => x.GetUserOverlappingReservationsAsync(cedula, startTime, endTime))
            .ReturnsAsync(new List<Reservation> { existingReservation });

        // Act
        var act = () => _sut.CreateReservationAsync(spaceId, cedula, startTime, endTime);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("You already have a reservation during this time period");
    }

    [Fact]
    public async Task GetAllReservations_ShouldReturnAllReservations()
    {
        // Arrange
        var space = new Space("Test Space", "Test Description") { Id = 1 };
        var reservations = new List<Reservation>
        {
            new(space, "123456789", DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(2)),
            new(space, "987654321", DateTime.UtcNow.AddHours(3), DateTime.UtcNow.AddHours(4))
        };

        _reservationRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(reservations);

        // Act
        var result = await _sut.GetAllReservationsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllBeOfType<ReservationDto>();
    }

    [Fact]
    public async Task GetReservationById_WhenExists_ShouldReturnReservation()
    {
        // Arrange
        var reservationId = 1;
        var space = new Space("Test Space", "Test Description") { Id = 1 };
        var reservation = new Reservation(space, "123456789", DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(2))
        {
            Id = reservationId
        };

        _reservationRepositoryMock
            .Setup(x => x.GetByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        // Act
        var result = await _sut.GetReservationByIdAsync(reservationId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(reservationId);
        result.SpaceId.Should().Be(space.Id);
        result.SpaceName.Should().Be(space.Name);
    }

    [Fact]
    public async Task GetReservationById_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var reservationId = 999;

        _reservationRepositoryMock
            .Setup(x => x.GetByIdAsync(reservationId))
            .ReturnsAsync((Reservation?)null);

        // Act
        var result = await _sut.GetReservationByIdAsync(reservationId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetReservationsBySpaceId_ShouldReturnSpaceReservations()
    {
        // Arrange
        var spaceId = 1;
        var space = new Space("Test Space", "Test Description") { Id = spaceId };
        var reservations = new List<Reservation>
        {
            new(space, "123456789", DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(2)),
            new(space, "987654321", DateTime.UtcNow.AddHours(3), DateTime.UtcNow.AddHours(4))
        };

        _reservationRepositoryMock
            .Setup(x => x.GetBySpaceIdAsync(spaceId))
            .ReturnsAsync(reservations);

        // Act
        var result = await _sut.GetReservationsBySpaceIdAsync(spaceId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllBeOfType<ReservationDto>();
        result.Should().AllSatisfy(r => r.SpaceId.Should().Be(spaceId));
    }

    [Fact]
    public async Task GetReservationsByCedula_ShouldReturnUserReservations()
    {
        // Arrange
        var cedula = "123456789";
        var space = new Space("Test Space", "Test Description") { Id = 1 };
        var reservations = new List<Reservation>
        {
            new(space, cedula, DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(2)),
            new(space, cedula, DateTime.UtcNow.AddHours(3), DateTime.UtcNow.AddHours(4))
        };

        _reservationRepositoryMock
            .Setup(x => x.GetByCedulaAsync(cedula))
            .ReturnsAsync(reservations);

        // Act
        var result = await _sut.GetReservationsByCedulaAsync(cedula);

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllBeOfType<ReservationDto>();
        result.Should().AllSatisfy(r => r.Cedula.Should().Be(cedula));
    }

    [Fact]
    public async Task DeleteReservation_WhenExists_ShouldDelete()
    {
        // Arrange
        var reservationId = 1;
        var space = new Space("Test Space", "Test Description") { Id = 1 };
        var reservation = new Reservation(space, "123456789", DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddHours(2))
        {
            Id = reservationId
        };

        _reservationRepositoryMock
            .Setup(x => x.GetByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        // Act
        await _sut.DeleteReservationAsync(reservationId);

        // Assert
        _reservationRepositoryMock.Verify(x => x.DeleteAsync(reservation), Times.Once);
    }

    [Fact]
    public async Task DeleteReservation_WhenNotExists_ShouldThrowException()
    {
        // Arrange
        var reservationId = 999;

        _reservationRepositoryMock
            .Setup(x => x.GetByIdAsync(reservationId))
            .ReturnsAsync((Reservation?)null);

        // Act
        var act = () => _sut.DeleteReservationAsync(reservationId);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"Reservation with ID {reservationId} not found");
    }
} 