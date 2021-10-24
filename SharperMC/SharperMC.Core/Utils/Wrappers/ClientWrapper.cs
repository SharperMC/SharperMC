using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using SharperMC.Core.Entities.Player;
using SharperMC.Core.Networking.Packets;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.Threading;

namespace SharperMC.Core.Utils.Wrappers
{
    public class ClientWrapper
    {
        public TcpClient TcpClient { get; }
        public PacketHandler PacketHandler { get; }
        
        public Player Player;
        
        public int Protocol;
        
        public PacketStatus Status = PacketStatus.Handshake;
        internal ICryptoTransform Encryptor { get; set; }
        internal ICryptoTransform Decryptor { get; set; }
        
        private readonly Queue<byte[]> _commands = new Queue<byte[]>();
        private readonly AutoResetEvent _resume = new AutoResetEvent(false);
        private readonly CThreadPool _threadPool;

        public bool Connected;
        public bool EncryptionEnabled;

        public byte[] EncryptionToken;

        public byte[] SharedKey;
        
        public ClientWrapper(TcpClient client)
        {
            if (client != null)
            {
                TcpClient = client;
                Connected = true;
                _threadPool = new CThreadPool();
                _threadPool.LaunchThread(ThreadRun);
                
                PacketHandler = new PacketHandler(this);
            }
        }

        public void Close()
        {
            TcpClient.Close();
            /*
             * TODO: Not closing _threadPool's threads could cause memory / cpu issues
             */
        }
        
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
        
        private void SendData(byte[] data)
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}