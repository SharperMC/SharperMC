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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zlib;
using SharperMC.Core.Config;
using SharperMC.Core.Networking.Packets;
using SharperMC.Core.Networking.Packets.Login.Client;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Misc;
using SharperMC.Core.Utils.Networking;
using SharperMC.Core.Utils.Packets;

namespace SharperMC.Core.Networking
{
	public class BasicListener
	{
		private TcpListener _serverListener;

		public void StartListening()
		{
			var port = 25515;//Config.GetProperty("port", 25565);
			if (port != 25565)
			{
				if (!NetUtils.PortAvailability(port))
				{
					ConsoleFunctions.WriteErrorLine("Port already in use... Shutting down server... [{0}]", true, port);
					Globals.StopServer();
					return;
				}
				ConsoleFunctions.WriteInfoLine("Starting server on port... {0}", true, port);
				_serverListener = new TcpListener(IPAddress.Any, port);
			}
			if (_serverListener == null)
			{
				ConsoleFunctions.WriteErrorLine("An error occured when starting the client listener.. Null TCPListener..");
				return;
			}
			_serverListener.Start();
			ConsoleFunctions.WriteInfoLine("Ready & looking for client connections... ");
			ConsoleFunctions.WriteInfoLine("To shutdown the server safely press CTRL+C or use stop/shutdown!");
			while (_serverListener.Server.IsBound)
			{
				TcpClient client = _serverListener.AcceptTcpClient();
				ConsoleFunctions.WriteDebugLine("A new client has been accepted.");
				new Task(() =>
				{
					HandleClientConnection(client);
				}).Start();
			}
		}

		public void StopListenening()
		{
			if(_serverListener != null && _serverListener.Server.IsBound)
				_serverListener.Stop();
		}

		#region ReadUncompressed
		
		private bool ReadUncompressed(ClientWrapper client, NetworkStream clientStream, int dlength)
		{
			var buffie = new byte[dlength];
			int receivedData;
			receivedData = clientStream.Read(buffie, 0, buffie.Length);
			if (receivedData > 0)
			{
				var buf = new DataBuffer(client);

				if (client.Decryptor != null)
				{
					var date = new byte[4096];
					client.Decryptor.TransformBlock(buffie, 0, buffie.Length, date, 0);
					buf.BufferedData = date;
				}
				else
				{
					buf.BufferedData = buffie;
				}

				buf.BufferedData = buffie;

				buf.Size = dlength;
				var packid = buf.ReadVarInt();

				if (!new PackageFactory(client, buf).Handle(packid))
				{
					ConsoleFunctions.WriteWarningLine("Unknown packet received! \"0x" + packid.ToString("X2") + "\"");
				}

				buf.Dispose();
				return true;
			}
			return false;
		}
		
		#endregion
		#region ReadCompressed

		private bool ReadCompressed(ClientWrapper client, NetworkStream clientStream, int dlength)
		{
			var buffie = new byte[dlength];
			int receivedData;
			receivedData = clientStream.Read(buffie, 0, buffie.Length);
			buffie = ZlibStream.UncompressBuffer(buffie);

			if (receivedData > 0)
			{
				var buf = new DataBuffer(client);

				if (client.Decryptor != null)
				{
					var date = new byte[4096];
					client.Decryptor.TransformBlock(buffie, 0, buffie.Length, date, 0);
					buf.BufferedData = date;
				}
				else
				{
					buf.BufferedData = buffie;
				}

				buf.BufferedData = buffie;

				buf.Size = dlength;
				var packid = buf.ReadVarInt();

				if (!new PackageFactory(client, buf).Handle(packid))
				{
					ConsoleFunctions.WriteWarningLine("Unknown packet received! \"0x" + packid.ToString("X2") + "\"");
				}

				buf.Dispose();
				return true;
			}
			return false;
		}

		#endregion
		
		/*
		 * TODO: Fix data compression
		 * TODO: Look into: Internal Exception: io.netty.handler.codec.DecoderException: java.lang.IndexOutOfBoundsException: readerIndex(2) + length(72) exceeds writerIndex(2): UnpooledHeapByteBuf(ridx: 2, widx: 2, cap: 2)
		 */
		private void HandleClientConnection(TcpClient client)
		{
			NetworkStream clientStream = client.GetStream();
			ClientWrapper WrappedClient = new ClientWrapper(client);
			
			Globals.ClientManager.AddClient(ref WrappedClient);
			while (true)
			{
				try
				{
					if (ServerSettings.UseCompression && WrappedClient.PacketMode == PacketMode.Play)
					{
						int packetLength = NetUtils.ReadVarInt(clientStream);
						int dataLength = NetUtils.ReadVarInt(clientStream);
						int actualDataLength = packetLength - NetUtils.GetVarIntBytes(dataLength).Length;
						ConsoleFunctions.WriteInfoLine("PacketLength: {0} \n DataLength: {1} \n ActualDataLength: {2}", true, packetLength, dataLength, actualDataLength);
						if (dataLength == 0)
						{
							if (!ReadCompressed(WrappedClient, clientStream, actualDataLength))
								break;
						}
						else
						{
							if (!ReadUncompressed(WrappedClient, clientStream, dataLength))
								break;
						}
					}
					else
					{
						if (!ReadUncompressed(WrappedClient, clientStream, NetUtils.ReadVarInt(clientStream)))
							break;
					}
				}
				catch (Exception ex)
				{
					ConsoleFunctions.WriteDebugLine("Error: \n" + ex);
					if (ServerSettings.ReportExceptionsToClient)
					{
						new Disconnect(WrappedClient)
						{
							Reason = new McChatMessage("§fServer threw an exception!\n\nException: \n" + ex.Message)
						}.Write();
					}
					else
					{
						new Disconnect(WrappedClient)
						{
							Reason = new McChatMessage("§fYou were kicked because of an internal problem!")
						}.Write();
					}
					break;
				}
			}
			Globals.DisconnectClient(WrappedClient);
			Thread.CurrentThread.Abort();
		}
	}
}