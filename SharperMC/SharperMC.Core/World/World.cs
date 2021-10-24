using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SharperMC.Core.Entities;
using SharperMC.Core.Entities.Player;
using SharperMC.Core.Networking.Packets.Type;
using SharperMC.Core.Networking.Packets.Versions._47.Play.Client;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core
{
    public class World
    {
        public World()
        {
            Day = 0;
            Time = 1200;
            Dimension = 0;
            DefaultGamemode = SharperMC.Instance.Server.ServerSettings.DefaultGamemode;
        }

        private int Dimension { get; set; }
        
        public string Name { get; set; }
        public int Difficulty { get; set; }
        private Gamemode DefaultGamemode { get; set; }

        private int Time { get; }
        private int Day { get; }
        private bool Rain { get; }

        private Dictionary<int, Player> Players { get; } = new();
        private List<Entity> Entities { get; } = new();

        public Player[] getPlayers()
        {
            return Players.Values.ToArray();
        }
        
        public void RemovePlayer(Player player)
        {
            RemovePlayer(player.Id);
        }

        private void RemovePlayer(int entityId)
        {
            lock (Players)
            {
                if (Players.ContainsKey(entityId))
                {
                    Players.Remove(entityId);
                }
            }
        }
        
        public Player GetPlayer(int entityId)
        {
            if (Players.ContainsKey(entityId))
            {
                return Players[entityId];
            }
            return null;
        }
        
        public void AddPlayer(Player player)
        {
            Players.Add(player.Id, player);
            switch (player.ClientWrapper.Protocol)
            {
                case 47:
                    new PlayerListItem_47(player.ClientWrapper, 0, player.Gamemode, player.Username, player.Uuid).Broadcast(this);
                    break;
            }

            SpawnPlayer(player.ClientWrapper);
        }
        
        private void SpawnPlayer(ClientWrapper caller)
        {
            foreach (var player in Players.Values.Where(i => i.ClientWrapper != caller))
            {
                switch (player.ClientWrapper.Protocol)
                {
                    case 47:
                        new PlayerListItem_47(player.ClientWrapper, 0, player.Gamemode, player.Username, player.Uuid).Write();
                        new SpawnPlayer_47(caller, player).Write();
                        new SpawnPlayer_47(player.ClientWrapper, caller.Player).Write();
                        break;
                }
                // i.BroadcastEquipment();
            }
        }
    }
}