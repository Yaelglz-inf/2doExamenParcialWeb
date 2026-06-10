using MediatR;
using MiApp.Application.Features.TicketPurchases.Commands.BuyTicket;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.TicketPurchases.Queries.GetUserTicketPurchases;

public class GetUserTicketPurchasesQuery : IRequest<IEnumerable<UserTicketPurchaseDto>>
{
    public int UserId { get; set; }
}

public class UserTicketPurchaseDto
{
    public int PurchaseId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string ZoneName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class GetUserTicketPurchasesQueryHandler : IRequestHandler<GetUserTicketPurchasesQuery, IEnumerable<UserTicketPurchaseDto>>
{
    private readonly ITicketPurchaseRepository _purchaseRepository;

    public GetUserTicketPurchasesQueryHandler(ITicketPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<IEnumerable<UserTicketPurchaseDto>> Handle(GetUserTicketPurchasesQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _purchaseRepository.GetPurchasesByUserAsync(request.UserId);

        return purchases.Select(p => new UserTicketPurchaseDto
        {
            PurchaseId = p.Id,
            EventName = p.Event.Name,
            EventDate = p.Event.EventDate,
            ZoneName = p.TicketZone.Name,
            Quantity = p.Quantity,
            UnitPrice = p.UnitPrice,
            TotalPrice = p.TotalPrice,
            PurchaseDate = p.PurchaseDate
        });
    }
}
