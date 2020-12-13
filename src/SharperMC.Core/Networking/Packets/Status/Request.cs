using Newtonsoft.Json;
using SharperMC.Core.Utils;

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
				var status = new StatusRequestMessage(Globals.ProtocolName, Globals.ProtocolVersion, ServerSettings.MaxPlayers, Globals.GetOnlinePlayerCount(), ServerSettings.Motd);
				var statusstring = JsonConvert.SerializeObject(status);
				Buffer.WriteVarInt(SendId);
				Buffer.WriteString(statusstring);
				Buffer.FlushData();
			}
		}
	}
}
