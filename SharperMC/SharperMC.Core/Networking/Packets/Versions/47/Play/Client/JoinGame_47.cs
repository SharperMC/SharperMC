using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Play.Client
{
    public class JoinGame_47 : SendablePacket
    {
        public JoinGame_47(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.JoinGame;
        }

        public override void Write()
        {
            DataBuffer.WriteVarInt(PacketId);
            DataBuffer.WriteInt(ClientWrapper.Player.Id);
            DataBuffer.WriteByte((byte) ClientWrapper.Player.Gamemode);
            DataBuffer.WriteByte(ClientWrapper.Player.Dimension);
            DataBuffer.WriteByte((byte) 1);
            DataBuffer.WriteByte((byte) SharperMC.Instance.Server.ServerSettings.MaxPlayers);
            DataBuffer.WriteString("default");
            DataBuffer.WriteBool(false);
            
            base.Write();
        }
    }
}