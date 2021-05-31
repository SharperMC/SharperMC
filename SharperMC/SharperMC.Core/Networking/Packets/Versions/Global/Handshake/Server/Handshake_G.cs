using System;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Networking.Packets.Versions._47;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions.Global.Handshake.Server
{
    public class Handshake_G : ReadablePacket
    {
        public Handshake_G(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = -1;
            PacketId = (int) Global.Handshake;
            Status = PacketStatus.Handshake;
        }
        
        public override void Read(DataBuffer dataBuffer)
        {
            base.Read(dataBuffer);
            var protocol = dataBuffer.ReadVarInt();
            var host = dataBuffer.ReadString();
            var port = dataBuffer.ReadShort();
            var state = dataBuffer.ReadVarInt();

            ClientWrapper.Protocol = protocol;
            
            Console.WriteLine("Protocol: {0} Host: {1} Port: {2} State: {3}", protocol, host, port, state);

            switch (state)
            {
                case 1:
                    ClientWrapper.Status = PacketStatus.Status;
                    break;
                case 2:
                    ClientWrapper.Status = PacketStatus.Login;
                    break;
            }
        }
    }
}