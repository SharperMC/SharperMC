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

using SharperMC.Core.Blocks;
using SharperMC.Core.Worlds.Standard.Decorators;
using SharperMC.Core.Worlds.Standard.Structures;

namespace SharperMC.Core.Worlds.Standard.BiomeSystem
{
	public interface IBiome
	{
		int Id { get; }
		byte MinecraftBiomeId { get; }
		int MaxTrees { get; }
		int MinTrees { get; }
		Structure[] TreeStructures { get; }
		ChunkDecorator[] Decorators { get; }
		float Temperature { get; }
		Block TopBlock { get; }
		Block Filling { get; }
	}
}