namespace MiApp.Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public EventStatus Status { get; set; } = EventStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int CreatedBy { get; set; } // UserId del admin que creó

    // Relaciones
    public ICollection<TicketZone> TicketZones { get; set; } = new List<TicketZone>();
    public ICollection<TicketPurchase> TicketPurchases { get; set; } = new List<TicketPurchase>();
}

public enum EventStatus
{
    Active,
    Cancelled
}
