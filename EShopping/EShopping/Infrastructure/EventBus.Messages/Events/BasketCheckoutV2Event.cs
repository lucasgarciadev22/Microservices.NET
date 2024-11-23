namespace EventBus.Messages.Events;

public class BasketCheckoutV2Event : BaseIntegrationEvent
{
    public string? UserName { get; set; }
    public decimal? TotalPrice { get; set; }
}
