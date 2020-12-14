using SharperMC.Core.Utils;

namespace SharperMC.Core.Networking.Packets.Play.Client
{
	public class OpenSignEditor : Package<OpenSignEditor>
	{
		public Vector3 Coordinates;
		public OpenSignEditor(ClientWrapper client) : base(client)
		{
			SendId = 0x36;
		}

		public OpenSignEditor(ClientWrapper client, DataBuffer buffer) : base(client, buffer)
		{
			SendId = 0x36;
		}

		public override void Write()
		{
			if (Buffer != null)
			{
				Buffer.WriteVarInt(SendId);
				Buffer.WritePosition(Coordinates);
				Buffer.FlushData();
			}
		}
	}
}
