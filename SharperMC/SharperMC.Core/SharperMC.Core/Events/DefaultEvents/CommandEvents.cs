using SharperMC.Core.Commands;

namespace SharperMC.Core.Events.DefaultEvents
{
    public abstract class CommandEventBase : ISenderEvent
    {
        public abstract string EventName { get; }
        public bool IsAsync { get; } = false;
        public ICommandSender Sender { get; }
        public virtual string Message { get; set; }
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
        private string _message;
        public override string EventName { get; } = "CommandEvent";
        public string Label { get; protected set; }
        public Command Command { get; set; }

        public override string Message
        {
            get => _message;
            set
            {
                _message = value;

                var i = _message.IndexOf(' ');
                Label = i < 0 ? _message : _message.Substring(0, i);
            }
        }

        public CommandEvent(ICommandSender sender, Command command, string message, string label, string prefix,
            ICommandSystem system, EventType type) :
            base(sender, message, prefix, system, type)
        {
            Command = command;
            Label = label;
        }

        public bool Cancelled { get; set; }
    }
}