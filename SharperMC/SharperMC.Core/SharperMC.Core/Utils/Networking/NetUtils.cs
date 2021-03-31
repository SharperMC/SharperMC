using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SharperMC.Core.Utils.Networking
{
    public static class NetUtils
    {
        public static bool PortAvailable(int portId)
        {
            return IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections()
                .All(tcpInfo => !tcpInfo.LocalEndPoint.Port.Equals(portId));
        }
        
        public static int ReadVarInt(NetworkStream stream)
        {
            var value = 0;
            var size = 0;
            int b;

            while (((b = stream.ReadByte()) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException($"VarInt size is longer than expected. (Size: {size})");
                }
            }
            return value | ((b & 0x7F) << (size * 7));
        }
        public static byte[] GetVarIntBytes(int integer)
        {
            var bytes = new List<byte>();
            while ((integer & -128) != 0)
            {
                bytes.Add((byte)(integer & 127 | 128));
                integer = (int)(((uint)integer) >> 7);
            }
            bytes.Add((byte)integer);
            return bytes.ToArray();
        }
    }
}