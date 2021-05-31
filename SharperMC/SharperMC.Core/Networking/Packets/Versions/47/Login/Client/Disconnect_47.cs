using System;
using Newtonsoft.Json;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Types;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Login.Client
{
    public class Disconnect_47 : SendablePacket
    {
        private readonly ChatText chatText;
        
        public Disconnect_47(ClientWrapper clientWrapper, ChatText reason) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.Disconnect;
            chatText = reason;
        }

        public override void Write()
        {
            DataBuffer.WriteVarInt(ClientWrapper.Status == PacketStatus.Login ? (int) Protocol47.Disconnect : 0x40);
            DataBuffer.WriteString(JsonConvert.SerializeObject(chatText));
            base.Write();
        }
    }
}