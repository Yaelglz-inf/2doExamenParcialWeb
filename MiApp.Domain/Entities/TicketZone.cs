namespace MiApp.Domain.Entities;

public class TicketZone
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty; // "VIP", "Preferente", "General"
    public decimal Price { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableTickets { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones
    public Event Event { get; set; } = null!;
    public ICollection<TicketPurchase> TicketPurchases { get; set; } = new List<TicketPurchase>();
}
