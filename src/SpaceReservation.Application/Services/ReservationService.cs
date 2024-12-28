using SpaceReservation.Application.DTOs;
using SpaceReservation.Domain.Entities;
using SpaceReservation.Domain.Exceptions;
using SpaceReservation.Domain.Repositories;

namespace SpaceReservation.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ISpaceRepository _spaceRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        ISpaceRepository spaceRepository)
    {
        _reservationRepository = reservationRepository;
        _spaceRepository = spaceRepository;
    }

    public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
    {
        var reservations = await _reservationRepository.GetAllAsync();
        return reservations.Select(ToDto);
    }

    public async Task<ReservationDto?> GetReservationByIdAsync(int id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        return reservation != null ? ToDto(reservation) : null;
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsBySpaceIdAsync(int spaceId)
    {
        var reservations = await _reservationRepository.GetBySpaceIdAsync(spaceId);
        return reservations.Select(ToDto);
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsByCedulaAsync(string cedula)
    {
        var reservations = await _reservationRepository.GetByCedulaAsync(cedula);
        return reservations.Select(ToDto);
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsByDateRangeAsync(DateTime? startDate, DateTime? endDate)
    {
        var reservations = await _reservationRepository.GetByDateRangeAsync(startDate, endDate);
        return reservations.Select(ToDto);
    }

    public async Task<ReservationDto> CreateReservationAsync(int spaceId, string cedula, DateTime startTime, DateTime endTime)
    {
        // Validate minimum/maximum duration
        var duration = endTime - startTime;
        if (duration.TotalMinutes < 30)
            throw new DomainException("Minimum reservation duration is 30 minutes");
        if (duration.TotalHours > 8)
            throw new DomainException("Maximum reservation duration is 8 hours");

        // Validate future dates
        if (startTime < DateTime.UtcNow)
            throw new DomainException("Cannot create reservations in the past");

        var space = await _spaceRepository.GetByIdAsync(spaceId);
        if (space == null)
            throw new DomainException($"Space with ID {spaceId} not found");

        // Check for overlapping reservations for the space
        var overlappingReservations = await _reservationRepository.GetOverlappingReservationsAsync(spaceId, startTime, endTime);
        if (overlappingReservations.Any())
            throw new DomainException("The space is already reserved for the selected time period");

        // Check for overlapping reservations for the user
        var userOverlappingReservations = await _reservationRepository.GetUserOverlappingReservationsAsync(cedula, startTime, endTime);
        if (userOverlappingReservations.Any())
            throw new DomainException("You already have a reservation during this time period");

        var reservation = new Reservation(space, cedula, startTime, endTime);
        await _reservationRepository.AddAsync(reservation);
        
        return ToDto(reservation);
    }

    public async Task DeleteReservationAsync(int id)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id);
        if (reservation == null)
            throw new DomainException($"Reservation with ID {id} not found");

        await _reservationRepository.DeleteAsync(reservation);
    }

    private static ReservationDto ToDto(Reservation reservation)
    {
        return new ReservationDto(
            reservation.Id,
            reservation.SpaceId,
            reservation.Space.Name,
            reservation.Cedula,
            reservation.StartTime,
            reservation.EndTime
        );
    }
} 