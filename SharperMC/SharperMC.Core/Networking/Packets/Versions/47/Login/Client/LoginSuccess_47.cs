using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Login.Client
{
    public class LoginSuccess_47 : SendablePacket
    {
        private readonly string _username;
        private readonly string _uuid;
        
        public LoginSuccess_47(ClientWrapper clientWrapper, string username, string uuid) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.LoginSuccess;
            _username = username;
            _uuid = uuid;
        }

        public override void Write()
        {
            DataBuffer.WriteVarInt(PacketId);
            DataBuffer.WriteString(_uuid);
            DataBuffer.WriteString(_username);
            base.Write();
        }
    }
}