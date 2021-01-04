using SharperMC.Core.Commands;

namespace SharperMC.Core.Events.DefaultEvents
{
    public class ChatEventBase : ISenderEvent, ICancellable
    {
        public string EventName { get; } = "ChatEvent";
        public bool IsAsync { get; } = false; // probably true idk
        public ICommandSender Sender { get; }
        public string Message { get; set; }

        protected ChatEventBase(string message, ICommandSender sender)
        {
            Message = message;
            Sender = sender;
        }

        public bool Cancelled { get; set; }
    }

    /// <summary>
    /// Called before chat is handled (before commands).
    /// </summary>
    public class PreChatEvent : ChatEventBase
    {
        public bool ProcessAsCommand { get; set; }

        public PreChatEvent(string message, bool processAsCommand, ICommandSender sender) : base(message, sender)
        {
            ProcessAsCommand = processAsCommand;
        }
    }

    /// <summary>
    /// Called before chat is handled (after commands).
    /// </summary>
    public class ChatEvent : ChatEventBase
    {
        /// <summary>
        /// {0} is player name, {1} is message.
        ///
        /// TODO: Improve this
        /// </summary>
        public string Format { get; set; }

        public ChatEvent(string message, string format, ICommandSender sender) : base(message, sender)
        {
            Format = format;
        }
    }
}