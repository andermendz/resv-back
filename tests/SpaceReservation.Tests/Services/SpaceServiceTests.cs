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

public class SpaceServiceTests
{
    private readonly Mock<ISpaceRepository> _spaceRepositoryMock;
    private readonly SpaceService _sut;

    public SpaceServiceTests()
    {
        _spaceRepositoryMock = new Mock<ISpaceRepository>();
        _sut = new SpaceService(_spaceRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllSpaces_ShouldReturnAllSpaces()
    {
        // Arrange
        var spaces = new List<Space>
        {
            new("Space 1", "Description 1") { Id = 1 },
            new("Space 2", "Description 2") { Id = 2 }
        };

        _spaceRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(spaces);

        // Act
        var result = await _sut.GetAllSpacesAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllBeOfType<SpaceDto>();
    }

    [Fact]
    public async Task GetSpaceById_WhenExists_ShouldReturnSpace()
    {
        // Arrange
        var spaceId = 1;
        var space = new Space("Test Space", "Test Description") { Id = spaceId };

        _spaceRepositoryMock
            .Setup(x => x.GetByIdAsync(spaceId))
            .ReturnsAsync(space);

        // Act
        var result = await _sut.GetSpaceByIdAsync(spaceId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(spaceId);
        result.Name.Should().Be(space.Name);
        result.Description.Should().Be(space.Description);
    }

    [Fact]
    public async Task GetSpaceById_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var spaceId = 999;

        _spaceRepositoryMock
            .Setup(x => x.GetByIdAsync(spaceId))
            .ReturnsAsync((Space?)null);

        // Act
        var result = await _sut.GetSpaceByIdAsync(spaceId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateSpace_ShouldCreateAndReturnSpace()
    {
        // Arrange
        var name = "New Space";
        var description = "New Description";
        var space = new Space(name, description);

        _spaceRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Space>()))
            .ReturnsAsync(space);

        // Act
        var result = await _sut.CreateSpaceAsync(name, description);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.Description.Should().Be(description);

        _spaceRepositoryMock.Verify(x => x.AddAsync(It.Is<Space>(s => 
            s.Name == name && 
            s.Description == description)), Times.Once);
    }

    [Fact]
    public async Task UpdateSpace_WhenExists_ShouldUpdate()
    {
        // Arrange
        var spaceId = 1;
        var space = new Space("Old Name", "Old Description") { Id = spaceId };
        var newName = "Updated Name";
        var newDescription = "Updated Description";

        _spaceRepositoryMock
            .Setup(x => x.GetByIdAsync(spaceId))
            .ReturnsAsync(space);

        _spaceRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Space>()))
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateSpaceAsync(spaceId, newName, newDescription);

        // Assert
        _spaceRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Space>(s => 
            s.Id == spaceId && 
            s.Name == newName && 
            s.Description == newDescription)), Times.Once);
    }

    [Fact]
    public async Task UpdateSpace_WhenNotExists_ShouldThrowException()
    {
        // Arrange
        var spaceId = 999;
        var newName = "Updated Name";
        var newDescription = "Updated Description";

        _spaceRepositoryMock
            .Setup(x => x.GetByIdAsync(spaceId))
            .ReturnsAsync((Space?)null);

        // Act
        var act = () => _sut.UpdateSpaceAsync(spaceId, newName, newDescription);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"Space with ID {spaceId} not found");
    }

    [Fact]
    public async Task DeleteSpace_WhenExists_ShouldDelete()
    {
        // Arrange
        var spaceId = 1;
        var space = new Space("Test Space", "Test Description") { Id = spaceId };

        _spaceRepositoryMock
            .Setup(x => x.GetByIdAsync(spaceId))
            .ReturnsAsync(space);

        // Act
        await _sut.DeleteSpaceAsync(spaceId);

        // Assert
        _spaceRepositoryMock.Verify(x => x.DeleteAsync(space), Times.Once);
    }

    [Fact]
    public async Task DeleteSpace_WhenNotExists_ShouldThrowException()
    {
        // Arrange
        var spaceId = 999;

        _spaceRepositoryMock
            .Setup(x => x.GetByIdAsync(spaceId))
            .ReturnsAsync((Space?)null);

        // Act
        var act = () => _sut.DeleteSpaceAsync(spaceId);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage($"Space with ID {spaceId} not found");
    }
} 