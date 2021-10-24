using SharperMC.Core.Entities.Player;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Play.Client
{
    public class SpawnPlayer_47 : SendablePacket
    {
        private readonly Player _player;
        
        public SpawnPlayer_47(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.SpawnPlayer;
            _player = clientWrapper.Player;
        }
        
        public SpawnPlayer_47(ClientWrapper clientWrapper, Player player) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.SpawnPlayer;
            _player = player;
        }

        public override void Write()
        {
            DataBuffer.WriteVarInt(PacketId);
            DataBuffer.WriteVarInt(_player.Id);
            DataBuffer.WriteUuid(_player.Uuid);
            DataBuffer.WriteInt((int) _player.Location.X*32);
            DataBuffer.WriteInt((int) _player.Location.Y*32);
            DataBuffer.WriteInt((int) _player.Location.Z*32);
            DataBuffer.WriteByte((byte) _player.Location.Yaw);
            DataBuffer.WriteByte((byte) _player.Location.Pitch);
            DataBuffer.WriteShort(0);
            DataBuffer.WriteByte(127);
            base.Write();
        }
    }
}