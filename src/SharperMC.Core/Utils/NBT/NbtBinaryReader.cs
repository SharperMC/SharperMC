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
using System.IO;
using System.Text;
using fNbt;

namespace SharperMC.Core.Utils.NBT
{
	/// <summary>
	///     BinaryReader wrapper that takes care of reading primitives from an NBT stream,
	///     while taking care of endianness, string encoding, and skipping.
	/// </summary>
	public class NbtBinaryReader : BinaryReader
	{
		private const int SeekBufferSize = 64*1024;
		private readonly bool _bigEndian;

		private readonly byte[] _floatBuffer = new byte[sizeof (float)],
			_doubleBuffer = new byte[sizeof (double)];

		private byte[] _seekBuffer;

		public NbtBinaryReader(Stream input, bool bigEndian)
			: base(input)
		{
			this._bigEndian = bigEndian;
		}

		public TagSelector Selector { get; set; }

		public NbtTagType ReadTagType()
		{
			var type = (NbtTagType) ReadByte();
			if (type < NbtTagType.End || type > NbtTagType.IntArray)
			{
				throw new NbtFormatException("NBT tag type out of range: " + (int) type);
			}
			return type;
		}

		public override short ReadInt16()
		{
			return BitConverter.IsLittleEndian == _bigEndian ? NbtBinaryWriter.Swap(base.ReadInt16()) : base.ReadInt16();
		}

		public override int ReadInt32()
		{
			return BitConverter.IsLittleEndian == _bigEndian ? NbtBinaryWriter.Swap(base.ReadInt32()) : base.ReadInt32();
		}

		public override long ReadInt64()
		{
			return BitConverter.IsLittleEndian == _bigEndian ? NbtBinaryWriter.Swap(base.ReadInt64()) : base.ReadInt64();
		}

		public override float ReadSingle()
		{
			if (BitConverter.IsLittleEndian == _bigEndian)
			{
				BaseStream.Read(_floatBuffer, 0, sizeof (float));
				Array.Reverse(_floatBuffer);
				return BitConverter.ToSingle(_floatBuffer, 0);
			}
			return base.ReadSingle();
		}

		public override double ReadDouble()
		{
			if (BitConverter.IsLittleEndian == _bigEndian)
			{
				BaseStream.Read(_doubleBuffer, 0, sizeof (double));
				Array.Reverse(_doubleBuffer);
				return BitConverter.ToDouble(_doubleBuffer, 0);
			}
			return base.ReadDouble();
		}

		public override string ReadString()
		{
			var length = ReadInt16();
			if (length < 0)
			{
				throw new NbtFormatException("Negative string length given!");
			}
			var stringData = ReadBytes(length);
			return Encoding.UTF8.GetString(stringData);
		}

		public void Skip(int bytesToSkip)
		{
			if (bytesToSkip < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(bytesToSkip));
			}
			if (BaseStream.CanSeek)
			{
				BaseStream.Position += bytesToSkip;
			}
			else if (bytesToSkip != 0)
			{
				if (_seekBuffer == null)
					_seekBuffer = new byte[SeekBufferSize];
				var bytesDone = 0;
				while (bytesDone < bytesToSkip)
				{
					var readThisTime = BaseStream.Read(_seekBuffer, bytesDone, bytesToSkip - bytesDone);
					if (readThisTime == 0)
					{
						throw new EndOfStreamException();
					}
					bytesDone += readThisTime;
				}
			}
		}

		public void SkipString()
		{
			var length = ReadInt16();
			if (length < 0)
			{
				throw new NbtFormatException("Negative string length given!");
			}
			Skip(length);
		}
	}
}