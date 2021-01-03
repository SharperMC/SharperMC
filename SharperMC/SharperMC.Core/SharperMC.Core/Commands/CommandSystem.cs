using System.Collections.Generic;
using System.Linq;

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
        
        public IEnumerable<string> GetPrefixes()
        {
            return Prefixes;
        }

        public void ParseCommand(ICommandSender sender, string message)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> ParseTab(ICommandSender sender, string message)
        {
            throw new System.NotImplementedException();
        }

        public void AddCommand(Command command)
        {
            var aliases = new string[command.Aliases.Length + 1];
            command.Aliases.CopyTo(aliases, 0);
            aliases[aliases.Length - 1] = command.Name;
            for (var i = 0; i < aliases.Length; i++)
                aliases[i] = aliases[i].ToLower();
            CommandMap.Add(aliases, command);
        }

        public Command GetCommand(string label)
        {
            label = label.ToLower();
            return CommandMap.FirstOrDefault(s => s.Key.Contains(label)).Value;
        }
    }
}