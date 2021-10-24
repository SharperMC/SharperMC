using System;
using SharperMC.Core.Entities.Player;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Data;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Type
{
    public class SendablePacket
    {
        protected ClientWrapper ClientWrapper;

        protected int Protocol;
        protected int PacketId;

        protected DataBuffer DataBuffer { get; set; }

        public SendablePacket(ClientWrapper clientWrapper)
        {
            ClientWrapper = clientWrapper;
            DataBuffer = new DataBuffer(clientWrapper);
        }
        
        public virtual void Write()
        {
            DataBuffer.WriteData();
        }
        
        public void Broadcast(World world, bool self = true, Player source = null)
        {
            foreach (var player in world.getPlayers())
            {
                if (!self && player == source)
                {
                    continue;
                }
                ClientWrapper = player.ClientWrapper;
                DataBuffer = new DataBuffer(player.ClientWrapper);
                Write();
            }
        }
    }
}