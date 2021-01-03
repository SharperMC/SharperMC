using SharperMC.Core.Entity;

namespace SharperMC.Core.Events.DefaultEvents
{
    public abstract class PlayerEvent : IEvent
    {
        public abstract string EventName { get; }
        public abstract bool IsAsync { get; }
        public abstract Player Player { get; }
    }
}