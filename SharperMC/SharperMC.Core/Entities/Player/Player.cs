using System;
using SharperMC.Core.Networking.Packets.Versions._47.Login.Client;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Types;
using SharperMC.Core.Utils.World;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Entities.Player
{
    public class Player : Entity
    {
        public readonly ClientWrapper ClientWrapper;
        
        public String Username;
        
        public Gamemode Gamemode { get; set; } = SharperMC.Instance.Server.ServerSettings.DefaultGamemode;
        
        public Guid Uuid;

        public Player(ClientWrapper clientWrapper)
        {
            ClientWrapper = clientWrapper;
        }

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

        public override void Spawn()
        {
            // var chunks = Level.Generator.GenerateChunks((ViewDistance * 21), _chunksUsed, this);
            // new MapChunkBulk(Wrapper) {Chunks = chunks.ToArray()}.Write();

            // new PlayerPositionAndLook(Wrapper)
            // {
            //                 X = KnownPosition.X,
            //                 Y = KnownPosition.Y,
            //                 Z = KnownPosition.Z,
            //                 Yaw = KnownPosition.Yaw,
            //                 Pitch = KnownPosition.Pitch,
            //                 OnGround = KnownPosition.OnGround
            // }.Write();

            // IsSpawned = true;
            // Level.AddPlayer(this);
            // Wrapper.Player.Inventory.SendToPlayer();
            // BroadcastEquipment();
            // SetGamemode(Gamemode, true);
        }
        
        // TODO: Complete player ping update method. 
        public void UpdatePing()
        {
        }
    }
}