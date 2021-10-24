using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using fNbt;
using SharperMC.Core.Biomes;
using SharperMC.Core.Blocks;
using SharperMC.Core.Utils.Data;
using SharperMC.Core.Utils.World.Vectors;

namespace SharperMC.Core
{
    public class Chunk : IDisposable
    {
        public BiomeBase Biome;
		public int[] BiomeColor = ArrayOf<int>.Create(256, 1);
		public byte[] BiomeId = ArrayOf<byte>.Create(256, 1);
		public NibbleArray Blocklight = new NibbleArray(16*16*256);
		public ushort[] Blocks = new ushort[16*16*256];
		public bool IsDirty = false;
		public ushort[] Metadata = new ushort[16*16*256];
		public NibbleArray Skylight = new NibbleArray(16*16*256);
		public IDictionary<Vector3D, NbtCompound> TileEntities = new Dictionary<Vector3D, NbtCompound>();

		public Chunk()
		{
			for (var i = 0; i < Skylight.Length; i ++)
				Skylight[i] = 0xff;
			for (var i = 0; i < BiomeColor.Length; i++)
				BiomeColor[i] = 8761930;
			for (var i = 0; i < Metadata.Length; i++)
				Metadata[i] = 0;
		}

		public World World { get; set; }
		public int X { get; set; }
		public int Z { get; set; }

		public ushort GetBlock(int x, int y, int z)
		{
			var index = x + 16*z + 16*16*y;
			if (index >= 0 && index < Blocks.Length)
			{
				return (Blocks[index]);
			}
			return 0x0;
		}

		public byte GetMetadata(int x, int y, int z)
		{
			var index = x + 16*z + 16*16*y;
			if (index >= 0 && index < Metadata.Length)
			{
				return (byte) (Metadata[index]);
			}
			return 0x0;
		}

		public void SetMetadata(int x, int y, int z, byte metadata)
		{
			var index = x + 16*z + 16*16*y;
			if (index >= 0 && index < Metadata.Length)
			{
				Metadata[index] = metadata;
			}
		}

		public void SetBlock(int x, int y, int z, Block block)
		{
			var index = x + 16*z + 16*16*y;
			if (index >= 0 && index < Blocks.Length)
			{
				Blocks[index] = block.Id;
				Metadata[index] = block.Metadata;
			}
		}

		public void SetBlocklight(int x, int y, int z, byte data)
		{
			Blocklight[(x*2048) + (z*256) + y] = data;
		}

		public byte GetBlocklight(int x, int y, int z)
		{
			return Blocklight[(x*2048) + (z*256) + y];
		}

		public byte GetSkylight(int x, int y, int z)
		{
			return Skylight[(x*2048) + (z*256) + y];
		}

		public void SetSkylight(int x, int y, int z, byte data)
		{
			Skylight[(x*2048) + (z*256) + y] = data;
		}

		public NbtCompound GetBlockEntity(Vector3D coordinates)
		{
			NbtCompound nbt;
			TileEntities.TryGetValue(coordinates, out nbt);
			return nbt;
		}

		public void SetBlockEntity(Vector3D coordinates, NbtCompound nbt)
		{
			IsDirty = true;
			TileEntities[coordinates] = nbt;
		}

		public void RemoveBlockEntity(Vector3D coordinates)
		{
			IsDirty = true;
			TileEntities.Remove(coordinates);
		}

		public byte[] GetMeta()
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = new NbtBinaryWriter(stream, true))
				{
					writer.Write(IPAddress.HostToNetworkOrder(X));
					writer.Write(IPAddress.HostToNetworkOrder(Z));
					writer.Write((ushort) 0xffff); // bitmap

					writer.Flush();
					writer.Close();
				}
				return stream.ToArray();
			}
		}

		public byte[] GetChunkData()
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = new NbtBinaryWriter(stream, true))
				{
					writer.WriteVarInt((Blocks.Length*2) + Skylight.Data.Length + Blocklight.Data.Length + BiomeId.Length);

					for (var i = 0; i < Blocks.Length; i++)
						writer.Write((ushort) ((Blocks[i] << 4) | Metadata[i]));

					writer.Write(Blocklight.Data);
					writer.Write(Skylight.Data);

					writer.Write(BiomeId);

					writer.Flush();
					writer.Close();
				}
				return stream.ToArray();
			}
		}

		public byte[] GetBytes(bool unloader = false)
		{
			var writer = new DataBuffer(new byte[0]);
			if (!unloader)
			{
				writer.WriteInt(X);
				writer.WriteInt(Z);
				writer.WriteBool(true);
				writer.WriteUShort(0xffff); // bitmap
				writer.WriteVarInt((Blocks.Length*2) + Skylight.Data.Length + Blocklight.Data.Length + BiomeId.Length);

				for (var i = 0; i < Blocks.Length; i++)
				{
					writer.WriteUShort((ushort) ((Blocks[i] << 4) | Metadata[i]));
				}

				writer.Write(Blocklight.Data);
				writer.Write(Skylight.Data);

				writer.Write(BiomeId);
			}
			else
			{
				writer.WriteInt(X);
				writer.WriteInt(Z);
				writer.WriteBool(true);
				writer.WriteUShort(0);
				writer.WriteVarInt(0);
			}
			return writer.ExportWriter;
		}

		public byte[] Export()
		{
			var buffer = new DataBuffer(new byte[0]);

			buffer.WriteInt(Blocks.Length);

			for (var i = 0; i < Blocks.Length; i++)
				buffer.WriteUShort(Blocks[i]);

			buffer.WriteInt(Blocks.Length);
			for (var i = 0; i < Blocks.Length; i++)
				buffer.WriteUShort((ushort) Metadata[i]);

			buffer.WriteInt(Blocklight.Data.Length);
			buffer.Write(Blocklight.Data);

			buffer.WriteInt(Skylight.Data.Length);
			buffer.Write(Skylight.Data);

			buffer.WriteInt(BiomeId.Length);
			buffer.Write(BiomeId);

			return buffer.ExportWriter;
		}

		private bool disposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					Blocks = null;
					Metadata = null;
					Blocklight.Data = null;
					Blocklight = null;
					BiomeId = null;
					BiomeColor = null;
					Skylight.Data = null;
					Skylight = null;
				}

				disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
    }
}