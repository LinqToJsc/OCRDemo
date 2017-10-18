namespace TDF.Core.Event
{
    public interface IEvent
    {
        bool CancelBubble { get; set; }

        int Order { get; set; }

        bool Async { get; set; }
    }
}
