using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Numerics;
using System.Text;
using SharperMC.Core.Utils.Wrappers;

namespace SharperMC.Core.Utils
{
    public class DataBuffer
    {
        private readonly ClientWrapper _client;
		public byte[] BufferedData = new byte[4096];
		private int _lastByte;
		public int Size = 0;

		public DataBuffer(ClientWrapper client)
		{
			_client = client;
		}

		public DataBuffer(byte[] data)
		{
			BufferedData = data;
		}

		public void SetDataSize(int size)
		{
			Array.Resize(ref BufferedData, size);
			Size = size;
		}

		public void Dispose()
		{
			BufferedData = null;
			_lastByte = 0;
		}

		#region Reader

		public int ReadByte()
		{
			var returnData = BufferedData[_lastByte];
			_lastByte++;
			return returnData;
		}

		public byte[] Read(int length)
		{
			var buffered = new byte[length];
			Array.Copy(BufferedData, _lastByte, buffered, 0, length);
			_lastByte += length;
			return buffered;
		}


		public int ReadInt()
		{
			var dat = Read(4);
			var value = BitConverter.ToInt32(dat, 0);
			return IPAddress.NetworkToHostOrder(value);
		}

		public float ReadFloat()
		{
			var almost = Read(4);
			var f = BitConverter.ToSingle(almost, 0);
			return NetworkToHostOrder(f);
		}

		public bool ReadBool()
		{
			var answer = ReadByte();
			return answer == 1;
		}

		public double ReadDouble()
		{
			var almostValue = Read(8);
			return NetworkToHostOrder(almostValue);
		}

		public int ReadVarInt()
		{
			var value = 0;
			var size = 0;
			int b;
			while (((b = ReadByte()) & 0x80) == 0x80)
			{
				value |= (b & 0x7F) << (size++*7);
				if (size > 5)
				{
					throw new IOException("VarInt too long. Hehe, that's funny.");
				}
			}
			return value | ((b & 0x7F) << (size*7));
		}

		public long ReadVarLong()
		{
			var value = 0;
			var size = 0;
			int b;
			while (((b = ReadByte()) & 0x80) == 0x80)
			{
				value |= (b & 0x7F) << (size++*7);
				if (size > 10)
				{
					throw new IOException("VarLong too long. That's what she said.");
				}
			}
			return value | ((b & 0x7F) << (size*7));
		}

		public short ReadShort()
		{
			var da = Read(2);
			var d = BitConverter.ToInt16(da, 0);
			return IPAddress.NetworkToHostOrder(d);
		}

		public ushort ReadUShort()
		{
			var da = Read(2);
			return NetworkToHostOrder(BitConverter.ToUInt16(da, 0));
		}

		public ushort[] ReadUShort(int count)
		{
			var us = new ushort[count];
			for (var i = 0; i < us.Length; i++)
			{
				var da = Read(2);
				var d = BitConverter.ToUInt16(da, 0);
				us[i] = d;
			}
			return NetworkToHostOrder(us);
		}

		public ushort[] ReadUShortLocal(int count)
		{
			var us = new ushort[count];
			for (var i = 0; i < us.Length; i++)
			{
				var da = Read(2);
				var d = BitConverter.ToUInt16(da, 0);
				us[i] = d;
			}
			return us;
		}

		public short[] ReadShortLocal(int count)
		{
			var us = new short[count];
			for (var i = 0; i < us.Length; i++)
			{
				var da = Read(2);
				var d = BitConverter.ToInt16(da, 0);
				us[i] = d;
			}
			return us;
		}

		public string ReadString()
		{
			var length = ReadVarInt();
			var stringValue = Read(length);

			return Encoding.UTF8.GetString(stringValue);
		}

		public long ReadLong()
		{
			var l = Read(8);
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(l, 0));
		}

		private double NetworkToHostOrder(byte[] data)
		{
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(data);
			}
			return BitConverter.ToDouble(data, 0);
		}

		private float NetworkToHostOrder(float network)
		{
			var bytes = BitConverter.GetBytes(network);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			return BitConverter.ToSingle(bytes, 0);
		}

		private ushort[] NetworkToHostOrder(ushort[] network)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(network);
			return network;
		}

		private ushort NetworkToHostOrder(ushort network)
		{
			var net = BitConverter.GetBytes(network);
			if (BitConverter.IsLittleEndian)
				Array.Reverse(net);
			return BitConverter.ToUInt16(net, 0);
		}

		#endregion

		public byte[] ExportWriter => _bffr.ToArray();

		private readonly List<byte> _bffr = new List<byte>();

		public void Write(byte[] data, int offset, int length)
		{
			for (var i = 0; i < length; i++)
			{
				_bffr.Add(data[i + offset]);
			}
		}

		public void Write(byte[] data)
		{
			foreach (var i in data)
			{
				_bffr.Add(i);
			}
		}

		public void WriteVarInt(int integer)
		{
			while ((integer & -128) != 0)
			{
				_bffr.Add((byte) (integer & 127 | 128));
				integer = (int) (((uint) integer) >> 7);
			}
			_bffr.Add((byte) integer);
		}

		public void WriteVarLong(long i)
		{
			var fuck = i;
			while ((fuck & ~0x7F) != 0)
			{
				_bffr.Add((byte) ((fuck & 0x7F) | 0x80));
				fuck >>= 7;
			}
			_bffr.Add((byte) fuck);
		}

		public void WriteInt(int data)
		{
			var buffer = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data));
			Write(buffer);
		}

		public void WriteString(string data)
		{
			var stringData = Encoding.UTF8.GetBytes(data);
			WriteVarInt(stringData.Length);
			Write(stringData);
		}

		public void WriteShort(short data)
		{
			var shortData = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data));
			Write(shortData);
		}

		public void WriteUShort(ushort data)
		{
			var uShortData = BitConverter.GetBytes(data);
			Write(uShortData);
		}

		public void WriteByte(byte data)
		{
			_bffr.Add(data);
		}

		public void WriteBool(bool data)
		{
			Write(BitConverter.GetBytes(data));
		}

		public void WriteDouble(double data)
		{
			Write(HostToNetworkOrder(data));
		}

		public void WriteFloat(float data)
		{
			Write(HostToNetworkOrder(data));
		}

		public void WriteLong(long data)
		{
			Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data)));
		}

		public void WriteUuid(Guid uuid)
		{
			var guid = uuid.ToByteArray();
			var long1 = new byte[8];
			var long2 = new byte[8];
			Array.Copy(guid, 0, long1, 0, 8);
			Array.Copy(guid, 8, long2, 0, 8);
			Write(long1);
			Write(long2);
		}

		private static byte[] GetVarIntBytes(int integer)
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

		public void WriteData(bool queuing = false)
		{
			try
			{
				var allData = _bffr.ToArray();
				_bffr.Clear();

				WriteVarInt(allData.Length);
				var buffer = _bffr.ToArray();

				var data = new List<byte>();
				foreach (var i in buffer)
				{
					data.Add(i);
				}
				foreach (var i in allData)
				{
					data.Add(i);
				}
				_client.AddToQueue(data.ToArray(), queuing);
				
				_bffr.Clear();
			}
			catch (Exception ex)
			{
				
			}
		}
		
		private static byte[] HostToNetworkOrder(double d)
		{
			var data = BitConverter.GetBytes(d);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(data);

			return data;
		}

		private static byte[] HostToNetworkOrder(float host)
		{
			var bytes = BitConverter.GetBytes(host);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			return bytes;
		}
    }
}