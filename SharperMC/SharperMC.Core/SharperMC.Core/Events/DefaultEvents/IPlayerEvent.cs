using SharperMC.Core.Entity;

namespace SharperMC.Core.Events.DefaultEvents
{
    public interface IPlayerEvent : IEvent
    {
        public Player Player { get; }
    }
}