// Distrubuted under the MIT license
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

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using SharperMC.Core.Chat;
using SharperMC.Core.Commands;
using SharperMC.Core.Config;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Networking;
using SharperMC.Core.Networking.Packets.Login.Client;
using SharperMC.Core.PluginChannel;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Packets;
using SharperMC.Core.Worlds;

namespace SharperMC.Core
{
	public class Globals
	{
		public static readonly int ProtocolVersion = 47;
		public static readonly string ProtocolName = "SharperMC 1.8.x";
		public static readonly string OfficialProtocolName = "Minecraft 1.8.x";
		
		internal static ClientListener ServerListener;
		internal static LevelManager LevelManager;
		internal static ChatManager ChatManager;
		
		internal static RSAParameters ServerKey;
		
		internal static ClientManager ClientManager;
		internal static MessageFactory MessageFactory;
		
		public static readonly ConsoleSender ConsoleSender = new ConsoleSender();

		public static readonly Random Random = new Random(Environment.TickCount);

		public static int GetOnlinePlayerCount()
		{
			var count = LevelManager.GetLevels().Sum(lvl => lvl.OnlinePlayers.Count);
			count += LevelManager.MainLevel.OnlinePlayers.Count;
			return count;
		}
		
		public static void BroadcastPacket(Package packet)
		{
			foreach (var lvl in LevelManager.GetLevels())
			{
				lvl.BroadcastPacket(packet);
			}
			LevelManager.MainLevel.BroadcastPacket(packet);
		}
		
		public static void DisconnectClient(ClientWrapper client, string reason = null)
		{
			if (client != null)
			{
				if (client.Disconnected)
					return;
				if (client.Player != null)
				{
					ConsoleFunctions.WriteInfoLine(client.Player.Username + " disconnected" + (reason == null ? "." : " (Reason: {0})."), reason);
					client.Player.SavePlayer();
					client.Player.Level.RemovePlayer(client.Player.EntityId);
					client.Player.Level.BroadcastPlayerRemoval(client);
				}
				client.ThreadPool.KillAllThreads();
				client.TcpClient.Close();
				ClientManager.RemoveClient(client);
				client.Disconnected = true;
			}
			else
				ConsoleFunctions.WriteFatalErrorLine("Cannot disconnect a client from the server! (Save/Restart/Panic)");
		}
	}
}