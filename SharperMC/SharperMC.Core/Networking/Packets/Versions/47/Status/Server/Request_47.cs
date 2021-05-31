using System;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Networking.Packets.Versions._47.Status.Client;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Status.Server
{
    public class Request_47 : ReadablePacket
    {
        public Request_47(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.Request;
            Status = PacketStatus.Status;
        }

        public override void Read(DataBuffer dataBuffer)
        {
            base.Read(dataBuffer);
            new Response_47(ClientWrapper).Write();
        }
    }
}