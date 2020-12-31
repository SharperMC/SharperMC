using System;
using SharperMC.Core.Plugins;

namespace ExamplePlugin
{
    public class FailedPlugin : SharperPlugin
    {
        public override void Load()
        {
            throw new Exception("Plugin load");
        }

        public override void Enable()
        {
            throw new Exception("Plugin enable");
        }

        public override void Disable()
        {
            throw new Exception("Plugin disable");
        }
    }
}