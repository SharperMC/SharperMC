using System;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Play.Client
{
    public class MapChunkBulk_47 : SendablePacket
    {
        public MapChunkBulk_47(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.MapChunkBulk;
        }

        public override void Write()
        {
            //TODO: This
            base.Write();
        }
    }
}