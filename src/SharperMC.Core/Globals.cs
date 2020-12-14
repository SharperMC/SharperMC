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
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using SharperMC.Core.Chat;
using SharperMC.Core.Commands;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Networking;
using SharperMC.Core.Networking.Packets.Login.Client;
using SharperMC.Core.PluginChannel;
using SharperMC.Core.Utils;

namespace SharperMC.Core
{
	/* Notes:
	 * Currently online-mode and compression are not working yet.
	 * Will look into it soon.
	*/
	public class Globals
	{
		/*
		 * TODO: Update to 1.8.9 Protocol 47...
		 */
		internal static int ProtocolVersion = 47; //56 
		internal static string ProtocolName = "SharperMC 1.8.x";
		internal static string OfficialProtocolName = "Minecraft 1.8.x";

		internal static BasicListener ServerListener;
		internal static LevelManager LevelManager;
		internal static ChatManager ChatManager;

		internal static RSAParameters ServerKey;

		internal static ClientManager ClientManager;
		internal static MessageFactory MessageFactory;
		
		internal static Random Rand;
		public static ConsoleSender ConsoleSender = new ConsoleSender();

		public static void BroadcastChat(string message, Player sender = null)
		{
			BroadcastChat(new McChatMessage(message), ChatMessageType.ChatBox, sender);
		}
		public static void BroadcastChat(McChatMessage message, ChatMessageType chattype, Player sender)
		{
			foreach (var lvl in LevelManager.GetLevels())
			{
				lvl.BroadcastChat(message, chattype, sender);
			}
			LevelManager.MainLevel.BroadcastChat(message, chattype, sender);
		}

		public static int GetOnlinePlayerCount()
		{
			var count = 0;
			foreach (var lvl in LevelManager.GetLevels())
			{
				count += lvl.OnlinePlayers.Count;
			}
			count += LevelManager.MainLevel.OnlinePlayers.Count;
			return count;
		}

		public static byte[] Compress(byte[] input)
		{
			using (var output = new MemoryStream())
			{
				using (var zip = new GZipStream(output, CompressionMode.Compress))
				{
					zip.Write(input, 0, input.Length);
				}
				return output.ToArray();
			}
		}

		public static byte[] Decompress(byte[] input)
		{
			using (var output = new MemoryStream(input))
			{
				using (var zip = new GZipStream(output, CompressionMode.Decompress))
				{
					var bytes = new List<byte>();
					var b = zip.ReadByte();
					while (b != -1)
					{
						bytes.Add((byte) b);
						b = zip.ReadByte();
					}
					return bytes.ToArray();
				}
			}
		}

		/*
		 * TODO: Implement actual fix for double Player saving when shutting the server down.
		 */
        public static void StopServer(string stopMsg = "Shutting down server...")
        {
            ConsoleFunctions.WriteInfoLine("Shutting down...");
			Disconnect d = new Disconnect(null);
			d.Reason = new McChatMessage("§f" + stopMsg);
			BroadcastPacket(d);
	        ConsoleFunctions.WriteInfoLine("Saving all player data...");
	        foreach (var player in LevelManager.GetAllPlayers())
	        {
		        player.SavePlayer();
	        }
			ConsoleFunctions.WriteInfoLine("Saving config file...");
			Config.SaveConfig();
            ConsoleFunctions.WriteInfoLine("Saving chunks...");
	        LevelManager.SaveAllChunks();
	        ServerListener.StopListenening();
	        Environment.Exit(0);
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
				client.Disconnected = true;
				if(reason == null)
					ConsoleFunctions.WriteInfoLine(client.Player.Username + " disconnected.");
				else
					ConsoleFunctions.WriteInfoLine(client.Player.Username + " disconnected (Reason: {0}).", reason);
				client.ThreadPool.KillAllThreads();
				if (client.Player != null)
				{
					client.Player.SavePlayer();
					client.Player.Level.RemovePlayer(client.Player.EntityId);
					client.Player.Level.BroadcastPlayerRemoval(client);
				}
				client.TcpClient.Close();
				ClientManager.RemoveClient(client);
			}
			else
			{
				ConsoleFunctions.WriteFatalErrorLine("Cannot disconnect a client from the server! (Save/Restart)");
			}
		}
	}
}