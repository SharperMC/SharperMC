using System;
using SharperMC.Core.Networking.Packets.Versions._47.Login.Client;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Types;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Entities.Player
{
    public class Player : Entity
    {
        public readonly ClientWrapper ClientWrapper;
        
        public String Username;
        public String DisplayName;

        public Guid Uuid;

        public Player(ClientWrapper clientWrapper)
        {
            ClientWrapper = clientWrapper;
        }

        public Gamemode Gamemode { get; set; } = SharperMC.Instance.Server.ServerSettings.DefaultGamemode;

        public void Kick(String reason = "You were kicked from the server.")
        {
            switch (ClientWrapper.Protocol)
            {
                case 47:
                    new Disconnect_47(ClientWrapper, new ChatText(reason)).Write();
                    break;
            }
            ClientWrapper.Connected = false;
        }
    }
}