﻿// Distrubuted under the MIT license
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using SharperMC.Core.Utils;
using SharperMC.Core.Blocks;
using SharperMC.Core.Entity;
using SharperMC.Core.Utils.Misc;
using SharperMC.Core.Utils.Vectors;

namespace SharperMC.Core.Worlds.Better
{
	internal class BetterWorldProvider : WorldProvider
	{
		private static readonly Random Getrandom = new Random();
		private static readonly object SyncLock = new object();
		private readonly string _folder;
		public Dictionary<Tuple<int, int>, ChunkColumn> ChunkCache = new Dictionary<Tuple<int, int>, ChunkColumn>();

		public BetterWorldProvider(string folder)
		{
			_folder = folder;
			IsCaching = true;
		}

		public override sealed bool IsCaching { get; set; }

		public override ChunkColumn LoadChunk(int x, int z)
		{
			var u = FileCompression.Decompress(File.ReadAllBytes(_folder + "/" + x + "." + z + ".cfile"));
			var reader = new DataBuffer(u);

			var blockLength = reader.ReadInt();
			var block = reader.ReadUShortLocal(blockLength);

			var metalength = reader.ReadInt();
			var blockmeta = reader.ReadUShortLocal(metalength);


			var skyLength = reader.ReadInt();
			var skylight = reader.Read(skyLength);

			var lightLength = reader.ReadInt();
			var blocklight = reader.Read(lightLength);

			var biomeIdLength = reader.ReadInt();
			var biomeId = reader.Read(biomeIdLength);

			var cc = new ChunkColumn
			{
				Blocks = block,
				Metadata = blockmeta,
				Blocklight = {Data = blocklight},
				Skylight = {Data = skylight},
				BiomeId = biomeId,
				X = x,
				Z = z
			};
			Debug.WriteLine("We should have loaded " + x + ", " + z);
			return cc;
		}

		public override void SaveChunks(string folder)
		{
			lock (ChunkCache)
			{
				foreach (var i in ChunkCache.Values.ToArray())
				{
					File.WriteAllBytes(_folder + "/" + i.X + "." + i.Z + ".cfile", FileCompression.Compress(i.Export()));
				}
			}
		}

		public override ChunkColumn GenerateChunkColumn(Vector2 chunkCoordinates)
		{
			ChunkColumn c;
			if (ChunkCache.TryGetValue(new Tuple<int, int>(chunkCoordinates.X, chunkCoordinates.Z), out c)) return c;

			if (File.Exists((_folder + "/" + chunkCoordinates.X + "." + chunkCoordinates.Z + ".cfile")))
			{
				var cd = LoadChunk(chunkCoordinates.X, chunkCoordinates.Z);
				lock (ChunkCache)
				{
					if (!ChunkCache.ContainsKey(new Tuple<int, int>(cd.X, cd.Z)))
						ChunkCache.Add(new Tuple<int, int>(cd.X, cd.Z), cd);
				}
				return cd;
			}

			var chunk = new ChunkColumn {X = chunkCoordinates.X, Z = chunkCoordinates.Z};
			PopulateChunk(chunk);

			if (!ChunkCache.ContainsKey(new Tuple<int, int>(chunkCoordinates.X, chunkCoordinates.Z)))
				ChunkCache.Add(new Tuple<int, int>(chunkCoordinates.X, chunkCoordinates.Z), chunk);

			return chunk;
		}

		public static int GetRandomNumber(int min, int max)
		{
			lock (SyncLock)
			{
				return Getrandom.Next(min, max);
			}
		}

		private void PopulateChunk(ChunkColumn chunk)
		{
		}

		/*public override void SetBlock(Block block, Level level, bool broadcast)
		{
			ChunkColumn c;
			lock (ChunkCache)
			{
				if (
					!ChunkCache.TryGetValue(new Tuple<int, int>((int) block.Coordinates.X >> 4, (int) block.Coordinates.Z >> 4), out c))
					throw new Exception("No chunk found!");
			}

			c.SetBlock(((int) block.Coordinates.X & 0x0f), ((int) block.Coordinates.Y & 0x7f), ((int) block.Coordinates.Z & 0x0f),
				block);
			if (!broadcast) return;

			BlockChange.Broadcast(block, level);
		}*/

		public override Vector3 GetSpawnPoint()
		{
			return new Vector3(0, 82, 0);
		}

		public override void ClearCache()
		{
			lock (ChunkCache)
			{
				ChunkCache.Clear();
			}
		}
	}
}