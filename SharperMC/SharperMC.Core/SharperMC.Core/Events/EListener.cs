using System;

namespace SharperMC.Core.Events
{
    public class EListener : Attribute
    {
        public Type EventType { get; }
        public EventPriority Priority { get; }

        public EListener(Type eventType, EventPriority priority = EventPriority.Medium)
        {
            EventType = eventType;
            Priority = priority;
        }
    }

    public enum EventPriority
    {
        ReallyLow,
        Low,
        Medium,
        High,
        ReallyHigh,
        Monitor
    }
}