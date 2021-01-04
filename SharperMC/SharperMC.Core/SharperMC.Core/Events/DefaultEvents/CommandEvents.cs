using SharperMC.Core.Commands;

namespace SharperMC.Core.Events.DefaultEvents
{
    public abstract class CommandEventBase : ISenderEvent, ICancellable
    {
        public abstract string EventName { get; }
        public bool IsAsync { get; } = false;
        public ICommandSender Sender { get; }
        public string Message { get; set; }
        public string Prefix { get; set; }
        public EventType Type { get; }

        protected CommandEventBase(ICommandSender sender, string message, string prefix, EventType type)
        {
            Sender = sender;
            Message = message;
            Prefix = prefix;
            Type = type;
        }

        public bool Cancelled { get; set; }
    }

    /// <summary>
    /// Calls after the ICommandSystem is found, but before it is passed to it.
    /// </summary>
    public class CommandPreExecutionEvent : CommandEventBase
    {
        public override string EventName { get; } = "CommandPreExecutionEvent";
        public ICommandSystem System { get; set; }

        public CommandPreExecutionEvent(ICommandSender sender, string message, string prefix, ICommandSystem system) :
            base(sender, message, prefix, EventType.Pre)
        {
            System = system;
        }
    }

    /// <summary>
    /// Calls after the whole command processing is finished.
    /// </summary>
    public class CommandPostExecutionEvent : CommandEventBase
    {
        public override string EventName { get; } = "CommandPostExecutionEvent";
        public ICommandSystem System { get; set; }

        public CommandPostExecutionEvent(ICommandSender sender, string message, string prefix, ICommandSystem system) :
            base(sender, message, prefix, EventType.Post)
        {
            System = system;
        }
    }

    /// <summary>
    /// Calls before and after command execution, but after the Command is found.
    /// </summary>
    public class CommandEvent : CommandEventBase
    {
        public override string EventName { get; } = "CommandEvent";
        public ICommandSystem System { get; set; }
        public Command Command { get; set; }

        public CommandEvent(ICommandSender sender, Command command, string message, string prefix,
            ICommandSystem system, EventType type) :
            base(sender, message, prefix, type)
        {
            System = system;
            Command = command;
        }
    }
}