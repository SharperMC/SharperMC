using SharperMC.Core.Commands;

namespace SharperMC.Core.Events.DefaultEvents
{
    public abstract class SenderEvent : IEvent
    {
        public abstract string EventName { get; }
        public abstract bool IsAsync { get; }
        public abstract ICommandSender Sender { get; }
    }
}