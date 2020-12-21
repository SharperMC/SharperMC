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
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using SharperMC.Core.Entity;
using SharperMC.Core.Networking.Packets.Play.Client;
using SharperMC.Core.Utils.Misc;

namespace SharperMC.Core.Utils.Client
{
    public enum PacketMode
    {
        Ping,
        Status,
        Login,
        Play
    }

    public class ClientWrapper
    {
        private readonly Queue<byte[]> _commands = new Queue<byte[]>();
        private readonly AutoResetEvent _resume = new AutoResetEvent(false);
        internal bool EncryptionEnabled = false;
        public PacketMode PacketMode = PacketMode.Ping;
        public Player Player;
        public readonly TcpClient TcpClient;
        public readonly MyThreadPool ThreadPool;
        internal int Protocol = 0;
        internal int ClientIdentifier = -1;
        public bool SetCompressionSend = false;

        private long _lastPing = 0;
        public bool Disconnected = false;

        public ClientWrapper(TcpClient client)
        {
            if (client != null)
            {
                TcpClient = client;
                ThreadPool = new MyThreadPool();
                ThreadPool.LaunchThread(ThreadRun);

                var bytes = new byte[8];
                Globals.Random.NextBytes(bytes);
                ConnectionId = Encoding.ASCII.GetString(bytes).Replace("-", "");
            }
        }

        internal byte[] SharedKey { get; set; }
        internal ICryptoTransform Encryptor { get; set; }
        internal ICryptoTransform Decryptor { get; set; }
        internal string ConnectionId { get; set; }
        internal string Username { get; set; }

        public void AddToQueue(byte[] data, bool queue = false)
        {
            if (TcpClient == null) return;
            if (queue)
            {
                lock (_commands)
                {
                    _commands.Enqueue(data);
                }

                _resume.Set();
            }
            else
            {
                SendData(data);
            }
        }

        private void ThreadRun()
        {
            while (_resume.WaitOne())
            {
                byte[] command;
                lock (_commands)
                {
                    command = _commands.Dequeue();
                }

                SendData(command);
            }
        }

        public void SendData(byte[] data)
        {
            if (TcpClient != null)
            {
                try
                {
                    if (Encryptor != null)
                    {
                        var toEncrypt = data;
                        data = new byte[toEncrypt.Length];
                        Encryptor.TransformBlock(toEncrypt, 0, toEncrypt.Length, data, 0);

                        var a = TcpClient.GetStream();
                        a.Write(data, 0, data.Length);
                        a.Flush();
                    }
                    else
                    {
                        TcpClient.Client.Send(data);
                    }

                    Globals.ClientManager.CleanErrors(this);
                }
                catch (Exception ex)
                {
                    Globals.ClientManager.PacketError(this, ex);
                }
            }
        }

        public void UpdatePing()
        {
            var time = UnixTimeNow();
            var ping = time - _lastPing;
            _lastPing = time;
            Globals.ClientManager.ReportPing(this);

            var packet = new PlayerListItem(null)
            {
                Action = 2,
                Latency = (int) ping,
                Uuid = Player.Uuid
            };
            Globals.BroadcastPacket(packet);
        }

        private static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long) timeSpan.TotalSeconds;
        }
    }
}