using System;
using SharperMC.Core.Plugins;
using SharperMC.Core.Utils.Console;

namespace ExamplePlugin
{
    /*
     * Be sure to have 
    
[assembly: PluginAttributes("ExamplePlugin.dll", "ExamplePlugin.PluginMain", "ExamplePlugin", "1.0", 
    "The SharperMC Team", "An example plugin for SharperMC.")]
    
     * in the AssemblyInfo. For some reason, it's staying in the obj.Debug idk why.
     * Todo: I'll have fix this.
     */
    public class PluginMain : SharperPlugin
    {
        private IPlugin _plugin;
        public override void Load()
        {
            ConsoleFunctions.WriteInfoLine("Loading!!!!!!");
            PluginManager.AddPlugin(_plugin = new FailedPlugin()
            {
                Name = "FailedPlugin",
                Version = "0.1",
                Author = "Sms_Gamer_3808",
                Description = "See what happens when a plugin fails to load..."
            });
        }

        public override void Enable()
        {
            ConsoleFunctions.WriteInfoLine("Enabling!!!!!!");
            PluginManager.Enable(_plugin);
        }

        public override void Disable()
        {
            ConsoleFunctions.WriteInfoLine("Disabling!!!!!!");
            PluginManager.Disable(_plugin);
        }
    }
}