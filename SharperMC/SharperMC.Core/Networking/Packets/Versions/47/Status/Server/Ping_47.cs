using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Networking.Packets.Versions._47.Status.Client;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Data;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Status.Server
{
    public class Ping_47 : ReadablePacket
    {
        public Ping_47(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.Ping;
            Status = PacketStatus.Status;
        }


        public override void Read(DataBuffer dataBuffer)
        {
            base.Read(dataBuffer);
            ClientWrapper.Player?.UpdatePing();
            new Pong_47(ClientWrapper, dataBuffer.ReadLong()).Write();
        }
    }
}