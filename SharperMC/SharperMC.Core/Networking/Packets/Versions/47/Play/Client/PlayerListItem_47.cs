using System;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Play.Client
{
    public class PlayerListItem_47 : SendablePacket
    {
        private readonly int _action;
        private readonly Gamemode _gamemode;
        private readonly string _username;
        private readonly Guid _uuid;
        private readonly int _latency;
        
        public PlayerListItem_47(ClientWrapper clientWrapper, int action, Gamemode gamemode, string username, Guid uuid) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.PlayerListItem;
            _action = action;
            _gamemode = gamemode;
            _username = username;
            _uuid = uuid;

            //TODO:
            _latency = 0;
        }

        public override void Write()
        {
            DataBuffer.WriteVarInt(PacketId);
            DataBuffer.WriteVarInt(_action);
            DataBuffer.WriteVarInt(1);
            DataBuffer.WriteUuid(_uuid);
            switch (_action)
            {
                case 0:
                    DataBuffer.WriteString(_username);
                    DataBuffer.WriteVarInt(_latency);
                    DataBuffer.WriteVarInt((byte) _gamemode);
                    DataBuffer.WriteVarInt(0);
                    DataBuffer.WriteBool(false);
                    break;
                case 1:
                    DataBuffer.WriteVarInt((byte)_gamemode);
                    break;
                case 2:
                    DataBuffer.WriteVarInt(_latency);
                    break;
                case 4:
                    break;
            }
            base.Write();
        }
    }
}