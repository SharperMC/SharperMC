using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using SharperMC.Core.Config;
using SharperMC.Core.Networking.Packets.Login.Client;
using SharperMC.Core.Networking.Packets.Play;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Packets;

namespace SharperMC.Core.Networking
{
	internal class ClientManager
	{
		private int CurrentIdentifier { get; set; }
		private Timer Ticks { get; set; }
		private Dictionary<int, ClientWrapper> Clients { get; set; }
		private Dictionary<int, long> ClientPing { get; set; } 
		private Dictionary<int, int> PacketErrors { get; set; } 
		internal ClientManager()
		{
			CurrentIdentifier = 0;
			Clients = new Dictionary<int, ClientWrapper>();
			PacketErrors = new Dictionary<int, int>();
			ClientPing = new Dictionary<int, long>();
			Ticks = new Timer();
			Ticks.Elapsed += DoServerTick;
			Ticks.Interval = 5000;
			Ticks.Start();
		}

		internal void AddClient(ref ClientWrapper client)
		{
			if (client.ClientIdentifier == -1)
			{
				CurrentIdentifier++;
				client.ClientIdentifier = CurrentIdentifier;
				Clients.Add(CurrentIdentifier, client);
				PacketErrors.Add(CurrentIdentifier, 0);
				ClientPing.Add(CurrentIdentifier, UnixTimeNow());
			}
		}

		internal void RemoveClient(ClientWrapper client)
		{
			if (Clients.ContainsKey(client.ClientIdentifier))
			{
				Clients.Remove(client.ClientIdentifier);
				PacketErrors.Remove(client.ClientIdentifier);
				GC.Collect();
			}
		}

		public void ReportPing(ClientWrapper client)
		{
			if (ClientPing.ContainsKey(client.ClientIdentifier))
			{
				ClientPing[client.ClientIdentifier] = UnixTimeNow();
			}
		}
		
		private void DoServerTick(object obj, ElapsedEventArgs eventargs)
		{
			foreach (var c in Clients.Values.ToArray())
			{
				if (c != null)
				{
					new KeepAlive(c).Write();
					if (ClientPing.ContainsKey(c.ClientIdentifier))
					{
						if ((UnixTimeNow() - ClientPing[c.ClientIdentifier]) > 2000)
						{
							Globals.DisconnectClient(c, "Ping timeout");
						}
					}
				}
			}
		}

		public void PacketError(ClientWrapper client, Exception exception)
		{
			if (PacketErrors.ContainsKey(client.ClientIdentifier))
			{
				int errors = PacketErrors[client.ClientIdentifier];
				PacketErrors[client.ClientIdentifier] = errors + 1;
		
				if (ServerSettings.DisplayPacketErrors)
				{
					ConsoleFunctions.WriteWarningLine("Packet error for player: \"" + client.Player.Username + "\" Packet errors: " +
					                                  PacketErrors[client.ClientIdentifier] + "\nError:\n" + exception.Message);
				}
		
				if (PacketErrors[client.ClientIdentifier] >= 3)
				{
					if (ServerSettings.ReportExceptionsToClient)
					{
						new Disconnect(client) {Reason = new McChatMessage("You were kicked from the server!\n" + exception.Message)}.Write();
					}
					else
					{
						new Disconnect(client) { Reason = new McChatMessage("You were kicked from the server!") }.Write();
					}
					Globals.DisconnectClient(client);
				}
			}
		}

		public void CleanErrors(ClientWrapper client)
		{
			if (PacketErrors.ContainsKey(client.ClientIdentifier))
			{
				PacketErrors[client.ClientIdentifier] = 0;
			}
		}

		private long UnixTimeNow()
		{
			var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
			return (long)timeSpan.TotalSeconds;
		}
	}
}
