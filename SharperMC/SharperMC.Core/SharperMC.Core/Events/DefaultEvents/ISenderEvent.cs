using SharperMC.Core.Commands;

namespace SharperMC.Core.Events.DefaultEvents
{
    public interface ISenderEvent : IEvent
    {
        public ICommandSender Sender { get; }
    }
}