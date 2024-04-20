namespace EventBus.Messages.Events;

public class BaseIntegrationEvent
{
    public Guid Id { get; }
    public DateTime CreationDate { get; set; }

    public BaseIntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    public BaseIntegrationEvent(Guid id, DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }
}
