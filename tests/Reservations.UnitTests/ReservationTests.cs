using System;
using Xunit;

public class ReservationTests
{
    [Fact]
    public void Overlaps_ShouldReturnTrue_WhenReservationsOverlap()
    {
        // Arrange
        var reservation1 = new Reservation
        {
            SpaceId = Guid.NewGuid(),
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(2)
        };

        var reservation2 = new Reservation
        {
            SpaceId = reservation1.SpaceId,
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(3)
        };

        // Act
        var result = reservation1.Overlaps(reservation2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidDuration_ShouldReturnFalse_WhenDurationTooShort()
    {
        // Arrange
        var reservation = new Reservation
        {
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMinutes(15)
        };

        // Act
        var result = reservation.IsValidDuration();

        // Assert
        Assert.False(result);
    }
} 