namespace TDF.Core.Event
{
    public interface IConsumer<in T> where T : IEvent
    {
        void Handler(T eventEntity);
    }
}
