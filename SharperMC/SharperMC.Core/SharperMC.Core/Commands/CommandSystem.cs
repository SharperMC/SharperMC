using System;
using System.Collections.Generic;
using System.Linq;
using SharperMC.Core.Events;
using SharperMC.Core.Events.DefaultEvents;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Commands
{
    public class CommandSystem : ICommandSystem
    {
        protected IEnumerable<string> Prefixes { get; set; }
        protected readonly Dictionary<string[], Command> CommandMap = new Dictionary<string[], Command>();

        public CommandSystem(IEnumerable<string> prefixes)
        {
            Prefixes = prefixes;
        }
        
        public virtual IEnumerable<string> GetPrefixes()
        {
            return Prefixes;
        }

        public virtual void ParseCommand(ICommandSender sender, string message, string prefix)
        {
            var origMessage = message;
            try
            {
                message = message.Trim();
                while (message.Contains("  ")) message = message.Replace("  ", " ");
                var split = message.Split(' ');
                var label = split.Length == 1 ? message : message.Substring(message.IndexOf(' '));
                var command = GetCommand(label);

                {
                    var e = new CommandEvent(sender, command, origMessage, label, prefix, this, EventType.Pre);
                    EventManager.CallEvent(e);
                    if (e.Cancelled) return;
                    if (e.Message != origMessage)
                    {
                        origMessage = e.Message;
                        
                        message = origMessage.Trim();
                        while (message.Contains("  ")) message = message.Replace("  ", " ");
                        
                        split = message.Split(' ');
                    }

                    command = e.Command;
                    sender = e.Sender;
                    label = e.Label;
                    prefix = e.Prefix;
                }
                
                if (command == default(Command))
                {
                    UnknownCommand(sender, label);
                    return;
                }

                string[] args;
                if (split.Length > 1)
                {
                    args = new string[split.Length - 1];
                    Array.Copy(split, 1, args, 0, split.Length - 1);
                }
                else args = new string[0];

                command.Execute(sender, label, args, origMessage);
                
                {
                    var e = new CommandEvent(sender, command, origMessage, label, prefix, this, EventType.Post);
                    EventManager.CallEvent(e);
                }
            }
            catch (Exception ex)
            {
                ConsoleFunctions.WriteWarningLine(ex.ToString());
                sender.SendChat("An error occured when executing this command.", TextColor.Red);
            }
        }

        public virtual IEnumerable<string> ParseTab(ICommandSender sender, string message, string prefix)
        {
            if (string.IsNullOrEmpty(message))
            {
                var strings = new List<string>();
                foreach (var keyValuePair in CommandMap)
                {
                    strings.AddRange(keyValuePair.Key);
                }
                return strings;
            }
            var origMessage = message;
            try
            {
                message = message.Trim();
                while (message.Contains("  ")) message = message.Replace("  ", " ");
                var split = message.Split(' ');
                var command = GetCommand(split[0]);
                if (command == default(Command)) return new List<string>();

                string[] args;
                if (split.Length > 1)
                {
                    args = new string[split.Length - 1];
                    Array.Copy(split, 1, args, 0, split.Length - 1);
                }
                else args = new string[0];

                return command.TabComplete(sender, split[0], args, origMessage).ToList();
            }
            catch (Exception ex)
            {
                ConsoleFunctions.WriteWarningLine(ex.ToString());
                sender.SendChat("An error occured when tab-completing the command.", TextColor.Red);
            }
            return new List<string>();
        }

        protected virtual void UnknownCommand(ICommandSender sender, string label)
        {
            sender.SendChat("Unknown command: " + label, TextColor.Red);
        }

        public virtual void AddCommand(Command command)
        {
            var aliases = new string[command.Aliases.Length + 1];
            command.Aliases.CopyTo(aliases, 1);
            aliases[0] = command.Name;
            for (var i = 0; i < aliases.Length; i++)
                aliases[i] = aliases[i].ToLower();
            CommandMap.Add(aliases, command);
        }

        public virtual Command GetCommand(string label)
        {
            label = label.ToLower();
            return CommandMap.FirstOrDefault(s => s.Key.Contains(label)).Value;
        }
    }
}