﻿// Distrubuted under the MIT license
// ===================================================
// SharperMC uses the permissive MIT license.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the “Software”), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ©Copyright SharperMC - 2020

using System.Collections.Generic;
using System.Linq;
using SharperMC.Core.Entity;
using SharperMC.Core.Networking.Packets.Play.Client;
using SharperMC.Core.Utils;
using SharperMC.Core.Worlds;

namespace SharperMC.Core
{
	public class LevelManager
	{
		public LevelManager(Level mainLevel)
		{
			MainLevel = mainLevel;
			SubLevels = new Dictionary<string, Level>();
		}

		private Dictionary<string, Level> SubLevels { get; set; }
		public Level MainLevel { get; private set; }

		public Level[] GetLevels()
		{
			return SubLevels.Values.ToArray();
		}

		public void AddLevel(string name, Level lvl)
		{
			ConsoleFunctions.WriteInfoLine("Initiating level: " + name);
			SubLevels.Add(name, lvl);
		}

		private Level GetLevel(string name)
		{
			var d = (from lvl in SubLevels where lvl.Key == name select lvl.Value).FirstOrDefault();
			if (d != null) return d;
			return MainLevel;
		}

		public void TeleportToLevel(Player player, string level)
		{
			var lvl = GetLevel(level);

			player.Level.RemovePlayer(player);
			player.Level.BroadcastPlayerRemoval(player.Wrapper);

			player.Level = lvl;

			new Respawn(player.Wrapper)
			{
				Dimension = lvl.Dimension,
				Difficulty = (byte) lvl.Difficulty,
				GameMode = (byte) lvl.DefaultGamemode
			}.Write();

			player.IsSpawned = false;
			player.KnownPosition = lvl.GetSpawnPoint();
			player.SendChunksForKnownPosition(true);
		}

		public void TeleportToMain(Player player)
		{
			player.Level.RemovePlayer(player);
			player.Level.BroadcastPlayerRemoval(player.Wrapper);

			player.Level = MainLevel;

			new Respawn(player.Wrapper)
			{
				Dimension = 0,
				Difficulty = (byte) MainLevel.Difficulty,
				GameMode = (byte) MainLevel.DefaultGamemode
			}.Write();

			player.IsSpawned = false;
			player.KnownPosition = MainLevel.GetSpawnPoint();
			player.SendChunksForKnownPosition(true);
		}

		public void SaveAllChunks()
		{
			foreach (Level lvl in GetLevels())
			{
				lvl.SaveChunks();
			}
			MainLevel.SaveChunks();
		}

		public Player[] GetAllPlayers()
		{
			List<Player> players = new List<Player>();
			foreach (Level lvl in GetLevels())
			{
				players.AddRange(lvl.GetOnlinePlayers);
			}
			players.AddRange(MainLevel.GetOnlinePlayers);
			return players.ToArray();
		}
	}
}