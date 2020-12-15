using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Misc;

namespace SharperMC.Core.PluginChannel
{
	public class PluginMessage
	{
		public string Command { get; private set; }
		public PluginMessage(string command)
		{
			Command = command;
		}

		public virtual void HandleData(ClientWrapper client, DataBuffer buffer)
		{
			
		}
	}
}
