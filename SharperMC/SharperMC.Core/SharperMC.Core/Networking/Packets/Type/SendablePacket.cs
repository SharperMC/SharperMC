using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Type
{
    public class SendablePacket
    {
        protected readonly ClientWrapper ClientWrapper;
        
        public int Protocol;
        public int PacketId;
        public PacketStatus Status;
        
        public DataBuffer DataBuffer { get; }
        
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