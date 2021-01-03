using System.Collections.Generic;

namespace SharperMC.Core.Commands
{
    public interface ICommandSystem
    {
        public IEnumerable<string> GetPrefixes();

        // message never has prefix
        public void ParseCommand(ICommandSender sender, string message);
        public IEnumerable<string> ParseTab(ICommandSender sender, string message);
        
        public void AddCommand(Command command);
        public Command GetCommand(string commandName);
        public bool IsCommand(string commandName);
    }
}