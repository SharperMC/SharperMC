using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Data;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Type
{
    public class ReadablePacket
    {
        public ClientWrapper ClientWrapper;

        public int Protocol;
        public int PacketId;
        public PacketStatus Status;
        
        private DataBuffer _dataBuffer;

        public ReadablePacket(ClientWrapper clientWrapper)
        {
            ClientWrapper = clientWrapper;
        }
        
        public virtual void Read(DataBuffer dataBuffer)
        {
            _dataBuffer = dataBuffer;
        }
    }
    
    public abstract class ReadablePacket<T> : ReadablePacket where T : ReadablePacket<T>
    {
        protected ReadablePacket(ClientWrapper client) : base(client)
        {
        }
    }
}