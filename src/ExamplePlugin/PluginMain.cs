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
        public override void Load()
        {
            ConsoleFunctions.WriteInfoLine("Loading!!!!!!");
        }

        public override void Enable()
        {
            ConsoleFunctions.WriteInfoLine("Enabling!!!!!!");
        }

        public override void Disable()
        {
            ConsoleFunctions.WriteInfoLine("Disabling!!!!!!");
        }
    }
}