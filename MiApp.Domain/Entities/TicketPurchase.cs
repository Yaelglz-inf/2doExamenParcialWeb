namespace MiApp.Domain.Entities;

public class TicketPurchase
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int TicketZoneId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; } // Calculado: UnitPrice * Quantity
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    // Relaciones
    public Event Event { get; set; } = null!;
    public TicketZone TicketZone { get; set; } = null!;
    public User User { get; set; } = null!;
}
