using System;
using System.Collections.Generic;
using System.Threading;
using SharperMC.Core.Entities;
using SharperMC.Core.Entities.Player;
using SharperMC.Core.Networking;
using SharperMC.Core.Utils.Management;

namespace SharperMC.Core
{
	public class Server
	{
		public readonly ServerSettings ServerSettings;

		private ClientListener ServerListener { get; }
		
		public ClientHandler ClientHandler { get; }
		
		public EntityHandler EntityHandler { get; }

		public Server()
		{
			ServerSettings = new();
			ServerListener = new();
			ClientHandler = new();
			EntityHandler = new();
		}
		
		public void StartServer()
		{
			try
			{
				if (ServerSettings.ServerThreading)
				{
					new Thread(ServerListener.StartListening).Start();
				}
				else
				{
					ServerListener.StartListening();
				}
			}
			catch (Exception ex)
			{
				SharperMC.UnhandledException(this, new UnhandledExceptionEventArgs(ex, false));
			}
		}
        
		public void StopServer(string message = "Server shutting down...")
		{
			Console.WriteLine(message);
			ServerListener.StopListening();
			Environment.Exit(0);
		}

		public List<Player> GetPlayers()
		{
			List<Player> players = new List<Player>();
			foreach (var clientWrapper in ClientHandler.ClientWrappers)
			{
				if(clientWrapper.Player != null)
					players.Add(clientWrapper.Player);
			}
			return players;
		}
	}
}