﻿using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Misc;

namespace SharperMC.Core.Networking.Packets.Play.Client
{
	public class EntityHeadLook : Package<EntityHeadLook>
	{
		public int EntityId;
		public double HeadYaw;
		public EntityHeadLook(ClientWrapper client) : base(client)
		{
			SendId = 0x19;
		}

		public EntityHeadLook(ClientWrapper client, DataBuffer buffer) : base(client, buffer)
		{
			SendId = 0x19;
		}

		public override void Write()
		{
			if (Client != null)
			{
				Buffer.WriteVarInt(SendId);
				Buffer.WriteVarInt(EntityId);
				Buffer.WriteByte((byte)((HeadYaw / 360) * 256));
				Buffer.FlushData();
			}
		}
	}
}
