using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Misc;

namespace SharperMC.Core.PluginChannel
{
	public class BrandMessage : PluginMessage
	{
		public BrandMessage() : base("Brand")
		{
			
		}

		public override void HandleData(ClientWrapper client, DataBuffer buffer)
		{
			string c = buffer.ReadString();
			ConsoleFunctions.WriteInfoLine(client.Player.Username + "'s client: " + c);
		}
	}
}
