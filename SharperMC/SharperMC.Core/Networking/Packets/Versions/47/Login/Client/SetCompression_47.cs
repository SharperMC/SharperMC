using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Login.Client
{
    public class SetCompression_47 : SendablePacket
    {
        private readonly int _threshold;
        
        public SetCompression_47(ClientWrapper clientWrapper, int threshold) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.SetCompression;
            _threshold = threshold;
        }

        public override void Write()
        {
            DataBuffer.WriteVarInt(PacketId);
            DataBuffer.WriteVarInt(_threshold);
            base.Write();
        }
    }
}