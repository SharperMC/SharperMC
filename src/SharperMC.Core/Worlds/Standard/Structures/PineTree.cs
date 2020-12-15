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
using SharperMC.Core.Blocks;
using SharperMC.Core.Utils;
using SharperMC.Core.Utils.Vectors;

namespace SharperMC.Core.Worlds.Standard.Structures
{
	internal class PineTree : Structure
	{
		private readonly int _leafRadius = 2;

		public override string Name
		{
			get { return "PineTree"; }
		}

		public override int Height
		{
			get { return 10; }
		}

		public bool ValidLocation(Vector3 location)
		{
			if (location.X - _leafRadius < 0 || location.X + _leafRadius >= 16 || location.Z - _leafRadius < 0 ||
			    location.Z + _leafRadius >= 256)
				return false;
			return true;
		}

		public override void Create(ChunkColumn chunk, int x, int y, int z)
		{
			var location = new Vector3(x, y, z);
			if (!ValidLocation(new Vector3(x, y, z))) return;

			var r = new Random();
			var height = r.Next(7, 8);
			GenerateColumn(chunk, location, height, new Block(17) {Metadata = 1});
			for (var Y = 1; Y < height; Y++)
			{
				if (Y%2 == 0)
				{
					GenerateVanillaCircle(chunk, location + new Vector3(0, Y + 1, 0), _leafRadius - 1, new Block(18) {Metadata = 1});
					continue;
				}
				GenerateVanillaCircle(chunk, location + new Vector3(0, Y + 1, 0), _leafRadius, new Block(18) {Metadata = 1});
			}

			GenerateTopper(chunk, location + new Vector3(0, height, 0), 0x1);
		}

		protected void GenerateTopper(ChunkColumn chunk, Vector3 location, byte type = 0x0)
		{
			var sectionRadius = 1;
			GenerateCircle(chunk, location, sectionRadius, new Block(18) {Metadata = 1});
			var top = location + new Vector3(0, 1, 0);
			var x = (int) location.X;
			var y = (int) location.Y + 1;
			var z = (int) location.Z;
			chunk.SetBlock(x, y, z, new Block(18) {Metadata = 1});
			if (type == 0x1 && y < 256)
				GenerateVanillaCircle(chunk, new Vector3(x, y, z), sectionRadius, new Block(18) {Metadata = 1});
		}
	}
}