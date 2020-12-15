using Newtonsoft.Json;
using SharperMC.Core.Config;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Misc;
using SharperMC.Core.Utils.Packets;

namespace SharperMC.Core.Networking.Packets.Status
{
	internal class Request : Package<Request>
	{
		public Request(ClientWrapper client) : base(client)
		{
			ReadId = 0x00;
			SendId = 0x00;
		}

		public Request(ClientWrapper client, DataBuffer buffer) : base(client, buffer)
		{
			ReadId = 0x00;
			SendId = 0x00;
		}

		public override void Read()
		{
			if (Buffer != null)
			{
				string serializeObject = JsonConvert.SerializeObject(new StatusRequestMessage(Globals.ProtocolName, Globals.ProtocolVersion, ServerSettings.MaxPlayers, Globals.GetOnlinePlayerCount(), ServerSettings.Motd));
				Buffer.WriteVarInt(SendId);
				Buffer.WriteString(serializeObject);
				Buffer.FlushData();
			}
		}
	}
}
