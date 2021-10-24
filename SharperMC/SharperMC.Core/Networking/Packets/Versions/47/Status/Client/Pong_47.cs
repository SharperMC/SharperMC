using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Status.Client
{
    public class Pong_47 : SendablePacket
    {
        private readonly long _time;
        
        public Pong_47(ClientWrapper clientWrapper, long time) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.Pong;
            _time = time;
        }

        public override void Write()
        {
            DataBuffer.WriteVarInt(PacketId);
            DataBuffer.WriteLong(_time);
            base.Write();
        }
    }
}