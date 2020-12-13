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
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SharperMC.Core.Utils.Networking
{
    public class NetUtils
    {
        public static bool PortAvailability(int portId)
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
                    ConsoleFunctions.WriteDebugLine("VarInt size is longer than expected. (Size: {0})", size);
                    //throw new IOException("VarInt size is longer than expected. (Size: {0})", size);
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