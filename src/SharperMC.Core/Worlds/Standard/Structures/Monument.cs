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

using SharperMC.Core.Blocks;
using SharperMC.Core.Utils;

namespace SharperMC.Core.Worlds.Standard.Structures
{
	//Hehe, not finished yet :P
	internal class Monument : Structure
	{
		public override string Name
		{
			get { return "Monument"; }
		}

		public override Block[] Blocks
		{
			get
			{
				return new[]
				{
					new Block(42) {Metadata = 1, Coordinates = new Vector3(0, 0, 0)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(0, 0, 0)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(1, 0, 0)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(2, 0, 0)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(3, 0, 0)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(0, 0, 1)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(1, 0, 1)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(2, 0, 1)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(3, 0, 1)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(0, 0, 2)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(1, 0, 2)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(2, 0, 2)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(3, 0, 2)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(0, 0, 3)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(1, 0, 3)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(2, 0, 3)},
					new Block(43) {Metadata = 1, Coordinates = new Vector3(3, 0, 3)}
				};
			}
		}
	}
}