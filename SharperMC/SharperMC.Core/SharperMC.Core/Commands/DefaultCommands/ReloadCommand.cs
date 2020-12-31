using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Commands.DefaultCommands
{
     public class ReloadCommand : Command
     {
        public  ReloadCommand() : base("reload", "reload", "Reloads everything (plugins, server config, etc...)")
        {
        }

        public override void Execute(ICommandSender sender, string label, string[] args)
        {
            if (args.Length == 0)
            {
                // Also reload plugins here
                Server.LoadServerSettingsFromFile();
                sender.SendChat("Server fully reloaded!");
            }
            else if(args.Length >= 1)
            {
                switch (args[0])
                {
                    case "plugins":
                        //TODO: reload plugins
                        break;
                    case "config":
                        Server.LoadServerSettingsFromFile();
                        sender.SendChat("Server config has been reloaded from file. ", TextColor.Green);
                        break;
                    default:
                        // Also reload plugins here
                        Server.LoadServerSettingsFromFile();
                        sender.SendChat("Server fully reloaded!");
                        break;
                }
            }
        }

        public override string[] TabComplete(ICommandSender sender, string label, string[] args)
        {
            return args.Length == 0 ? new[] {"plugins", "config", "all"} : new string[0];
        }
    }
    
}