using System;
using System.Threading;
using SharperMC.Core.Networking;

namespace SharperMC.Core
{
	public class Server
	{
		public readonly int ProtocolVersion = 47;
		public readonly string ProtocolName = "SharperMC 1.8.x";
		public readonly bool ProtocolOnline = true;

		public ClientListener ServerListener { get; }
		
		public ClientHandler ClientHandler { get; }
		
		public Random Random { get; }

		public Server()
		{
			ServerListener = new ClientListener();
			ClientHandler = new ClientHandler();
			Random = new Random(Environment.TickCount);
		}
		
		public void StartServer()
		{
			try
			{
				Console.WriteLine("Trying to start client listener");
				new Thread(ServerListener.StartListening).Start();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Start server exception");
				SharperMC.UnhandledException(this, new UnhandledExceptionEventArgs(ex, false));
			}
		}
        
		public void StopServer(string message = "Server shutting down...")
		{
			Console.WriteLine(message);
			ServerListener.StopListening();
			Environment.Exit(0);
		}
	}
}