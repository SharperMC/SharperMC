using System;
using System.Net.Sockets;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Networking.Packets
{
    public class PacketReader
    {
        public bool ReadUncompressed(ClientWrapper client, NetworkStream clientStream, int dataLength)
        {
            var buffer = new byte[dataLength];
            var receivedData = clientStream.Read(buffer, 0, buffer.Length);
            if (receivedData > 0)
            {
                var dataBuffer = new DataBuffer(client);
                
                if (client.Decryptor != null)
                {
                    var data = new byte[4096];
                    client.Decryptor.TransformBlock(buffer, 0, buffer.Length, data, 0);
                    dataBuffer.BufferedData = data;
                }
                else
                {
                    dataBuffer.BufferedData = buffer;
                }
                dataBuffer.BufferedData ??= buffer;
                dataBuffer.Size = dataLength;

                if (client.PacketHandler != null)
                    client.PacketHandler.HandlePacket(dataBuffer, dataBuffer.ReadVarInt());
                else
                    return false;
                dataBuffer.Dispose();
                return true;
            }
            return false;
        }
    }
}