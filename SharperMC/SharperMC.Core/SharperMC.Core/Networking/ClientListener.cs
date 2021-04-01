using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SharperMC.Core.Networking.Packets;
using SharperMC.Core.Utils.Networking;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking
{
	public class ClientListener
	{
		private TcpListener _serverListener;

		private PacketReader _packetReader = new PacketReader();
		private bool _disposed;

		public void StartListening()
		{
			int port = 25550;
			if (!NetUtils.PortAvailable(port))
			{
				Console.WriteLine("Port already in use ({0})", port);
				SharperMC.Instance.Server.StopServer();
				return;
			}
			
			Console.WriteLine("Starting server on port... {0}", port);
			(_serverListener = new TcpListener(IPAddress.Any, port)).Start();
			
			Console.WriteLine("Ready & looking for client connections...");
			Console.WriteLine("To shutdown the server safely press CTRL+C!");

			while (_serverListener.Server.IsBound)
			{
				try
				{
					if (_disposed)
					{
						Console.WriteLine("Disconnecting from ServerListener.");
						break;
					}
					if (!_serverListener.Pending()) continue;
					TcpClient client = _serverListener.AcceptTcpClient();
					Console.WriteLine("[ClientListener] ACCEPTED NEW CLIENT");
					new Task(() => { HandleClient(client); }).Start();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		public void StopListening()
		{
			_disposed = true;
			if(_serverListener != null && _serverListener.Server.IsBound)
				_serverListener.Stop();
		}

		private void HandleClient(TcpClient client)
		{
			NetworkStream clientStream = client.GetStream();
			ClientWrapper clientWrapper = new ClientWrapper(client);

			SharperMC.Instance.Server.ClientHandler.AddClient(ref clientWrapper);
			while (true)
			{
				try
				{
					if (!_packetReader.ReadUncompressed(clientWrapper, clientStream, NetUtils.ReadVarInt(clientStream)))
						break;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					break;
				}
			}
			SharperMC.Instance.Server.ClientHandler.DisconnectClient(clientWrapper);
			Thread.CurrentThread.Join();
		}
	}
}