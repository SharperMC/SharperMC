using SharperMC.Core.Commands;

namespace SharperMC.Core.Events.DefaultEvents
{
    public abstract class CommandEventBase : ISenderEvent
    {
        public abstract string EventName { get; }
        public bool IsAsync { get; } = false;
        public ICommandSender Sender { get; }
        public string Message { get; set; }
        public string Prefix { get; set; }
        public ICommandSystem System { get; set; }
        public EventType Type { get; }

        protected CommandEventBase(ICommandSender sender, string message, string prefix, ICommandSystem system, EventType type)
        {
            Sender = sender;
            Message = message;
            Prefix = prefix;
            System = system;
            Type = type;
        }
    }

    /// <summary>
    /// Calls after the ICommandSystem is found, but before it is passed to it.
    /// </summary>
    public class CommandPreExecutionEvent : CommandEventBase, ICancellable
    {
        public override string EventName { get; } = "CommandPreExecutionEvent";

        public CommandPreExecutionEvent(ICommandSender sender, string message, string prefix, ICommandSystem system) :
            base(sender, message, prefix, system, EventType.Pre)
        {
        }

        public bool Cancelled { get; set; }
    }

    /// <summary>
    /// Calls after the whole command processing is finished.
    /// </summary>
    public class CommandPostExecutionEvent : CommandEventBase
    {
        public override string EventName { get; } = "CommandPostExecutionEvent";

        public CommandPostExecutionEvent(ICommandSender sender, string message, string prefix, ICommandSystem system) :
            base(sender, message, prefix, system, EventType.Post)
        {
        }
    }

    /// <summary>
    /// Calls before and after command execution, but after the Command is found.
    /// </summary>
    public class CommandEvent : CommandEventBase, ICancellable
    {
        public override string EventName { get; } = "CommandEvent";
        public Command Command { get; set; }

        public CommandEvent(ICommandSender sender, Command command, string message, string prefix,
            ICommandSystem system, EventType type) :
            base(sender, message, prefix, system, type)
        {
            Command = command;
        }

        public bool Cancelled { get; set; }
    }
}