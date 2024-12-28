using SpaceReservation.Domain.Common;

namespace SpaceReservation.Domain.Entities;

public class Space : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    private Space() { } // For EF Core

    public Space(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Space name cannot be empty", nameof(name));
            
        Name = name;
        Description = description ?? string.Empty;
    }

    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Space name cannot be empty", nameof(name));
            
        Name = name;
        Description = description ?? string.Empty;
    }
} 