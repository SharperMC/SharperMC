using System.Collections.Generic;
using SharperMC.Core.Events;
using SharperMC.Core.Plugins;
using SharperMC.Core.Utils.Text;

namespace SharperMC.Core.Commands.DefaultCommands
{
    public class ReloadCommand : Command
    {
        public ReloadCommand() : base("reload", "reload", "Reloads everything (plugins, server config, etc...)")
        {
        }

        public override void Execute(ICommandSender sender, string label, string[] args, string origMessage)
        {
            if (args.Length == 0)
            {
                Server.LoadServerSettingsFromFile();
                sender.SendChat("Server fully reloaded!");
            }
            else if (args.Length >= 1)
            {
                switch (args[0])
                {
                    case "plugins":
                        ReloadPlugins();
                        sender.SendChat("Reloaded all plugins.", TextColor.Green);
                        break;
                    case "config":
                        ReloadConfig();
                        sender.SendChat("Server config has been reloaded from file.", TextColor.Green);
                        break;
                    default:
                        ReloadPlugins();
                        ReloadConfig();
                        sender.SendChat("Server fully reloaded!", TextColor.Green);
                        break;
                }
            }
        }

        private void ReloadPlugins()
        {
            EventManager.UnregisterAllListeners();
            PluginManager.DisableAllPlugins();
            PluginManager.EnableAllPlugins();
        }

        private void ReloadConfig()
        {
            Server.LoadServerSettingsFromFile();
        }

        public override IEnumerable<string> TabComplete(ICommandSender sender, string label, string[] args,
            string origMessage)
        {
            return args.Length == 0 ? new[] {"plugins", "config", "all"} : new string[0];
        }
    }
}