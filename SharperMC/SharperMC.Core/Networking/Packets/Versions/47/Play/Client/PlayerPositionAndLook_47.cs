using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets.Versions._47.Play.Client
{
    public class PlayerPositionAndLook_47 : SendablePacket
    {
        public PlayerPositionAndLook_47(ClientWrapper clientWrapper) : base(clientWrapper)
        {
            Protocol = 47;
            PacketId = (int) Protocol47.PlayerListItem;
        }
    }
}