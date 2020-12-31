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
using System.Net.Sockets;
using SharperMC.Core.Entity;
using SharperMC.Core.Networking.Packets.Login.Client;
using SharperMC.Core.Networking.Packets.Play.Client;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Client;
using SharperMC.Core.Utils.Console;
using SharperMC.Core.Utils.Misc;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Networking
{
	public abstract class Package
	{
		protected Package(ClientWrapper client)
		{
			if (client == null) return;
			if (!client.TcpClient.Connected) return;
			Client = client;
			Stream = client.TcpClient.GetStream();
			Buffer = new DataBuffer(client);
		}

		protected Package(ClientWrapper client, DataBuffer buffer)
		{
			if (client == null) return;
			if (!client.TcpClient.Connected) return;
			Client = client;
			Stream = client.TcpClient.GetStream();
			Buffer = buffer;
		}

		public NetworkStream Stream { get; private set; }
		public DataBuffer Buffer { get; private set; }
		public ClientWrapper Client { get; private set; }
		public int ReadId { get; set; }
		public int SendId { get; set; }

		internal void SetTarget(ClientWrapper client)
		{
			Client = client;
			if (client.TcpClient != null && client.TcpClient.Connected)
			{
				Stream = client.TcpClient.GetStream();
			}
			else
			{
				new Disconnect(client).Write();
			}
			Buffer = new DataBuffer(client);
		}

		public virtual void Read()
		{
		}

		public virtual void Write()
		{
		}

		/*
		 * TODO: Issues with this... Figure out whats causing it. 
		 */
		public void Broadcast(Level level, bool self = true, Player source = null)
		{
			foreach (var player in level.GetOnlinePlayers)
			{
				try
				{
					if (player != null && player.Wrapper != null && player.Wrapper.TcpClient != null)
					{
						if (!self && player == source)
						{
							continue;
						}

						if (player.Wrapper.TcpClient.Connected)
						{
							Client = player.Wrapper;
							Buffer = new DataBuffer(player.Wrapper);
							Stream = player.Wrapper.TcpClient.GetStream(); //Exception here. (sometimes)
							Write();
						}
					}
					else if(player != null)
					{
						player.Kick();
					}
				}
				catch (Exception e)
				{
					ConsoleFunctions.WriteErrorLine("Exception thrown in Package.cs, Broadcast Function... " + e.StackTrace + " " + e.Message);
					//Catch any exception just to be sure the broadcast works.
					//TODO: Fix the exception.
				}
			}
		}
	}

	public abstract class Package<T> : Package where T : Package<T>
	{
		protected Package(ClientWrapper client) : base(client)
		{
		}

		protected Package(ClientWrapper client, DataBuffer buffer) : base(client, buffer)
		{
		}
	}
}