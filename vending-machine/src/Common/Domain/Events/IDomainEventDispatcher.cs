namespace Domain.Events
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(IDomainEvent devent);
    }
}
