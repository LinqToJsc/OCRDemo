namespace TDF.Core.Event
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventEntity) where T : class, IEvent;
    }
}
