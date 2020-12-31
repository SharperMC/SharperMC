namespace SharperMC.Core.Events
{
    public interface IEvent
    {
        public string EventName { get; }
        public bool IsAsync { get; }
    }

    // Thx darkmagician6
    public enum EventType
    {
        Pre = 0,
        On = 1,
        Post = 2,
        Send = 3,
        Receive = 4,
    }
}