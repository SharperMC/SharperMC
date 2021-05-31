using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Type
{
    public class SendablePacket
    {
        protected readonly ClientWrapper ClientWrapper;

        protected int Protocol;
        protected int PacketId;

        protected DataBuffer DataBuffer { get; }

        public SendablePacket(ClientWrapper clientWrapper)
        {
            ClientWrapper = clientWrapper;
            DataBuffer = new DataBuffer(clientWrapper);
        }
        
        public virtual void Write()
        {
            DataBuffer.WriteData();
        }
    }
}