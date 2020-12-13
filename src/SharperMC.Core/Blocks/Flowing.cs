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
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Blocks
{
	public abstract class Flowing : Block
	{
		private readonly int[] _flowCost = new int[4];
		private readonly bool[] _optimalFlowDirections = new bool[4];
		private int _adjacentSources;

		protected Flowing(ushort id) : base(id)
		{
			IsSolid = false;
			IsReplacible = true;
		}

		public override bool PlaceBlock(Level world, Player player, Vector3 blockCoordinates, BlockFace face, Vector3 mouseLocation)
		{
			CheckForHarden(world, (int) blockCoordinates.X, (int) blockCoordinates.Y, (int) blockCoordinates.Z);
			world.ScheduleBlockTick(this, TickRate());
			return false;
		}

		public override void DoPhysics(Level level)
		{
			CheckForHarden(level, (int) Coordinates.X, (int) Coordinates.Y, (int) Coordinates.Z);
			level.ScheduleBlockTick(this, TickRate());
		}

		public override void OnTick(Level level)
		{
			var world = level;
			var random = new Random();

			var x = (int) Coordinates.X;
			var y = (int) Coordinates.Y;
			var z = (int) Coordinates.Z;

			var currentDecay = GetFlowDecay(world, x, y, z);
			byte multiplier = 1;

			if (this is BlockFlowingLava)
			{
				multiplier = 2;
			}

			var flag = true;
			var tickRate = TickRate();

			if (currentDecay > 0)
			{
				var smallestFlowDecay = -100;
				_adjacentSources = 0;
				smallestFlowDecay = GetSmallestFlowDecay(world, x - 1, y, z, smallestFlowDecay);
				smallestFlowDecay = GetSmallestFlowDecay(world, x + 1, y, z, smallestFlowDecay);
				smallestFlowDecay = GetSmallestFlowDecay(world, x, y, z - 1, smallestFlowDecay);
				smallestFlowDecay = GetSmallestFlowDecay(world, x, y, z + 1, smallestFlowDecay);
				var newDecay = smallestFlowDecay + multiplier;
				if (newDecay >= 8 || smallestFlowDecay < 0)
				{
					newDecay = -1;
				}

				if (GetFlowDecay(world, x, y + 1, z) >= 0)
				{
					var topFlowDecay = GetFlowDecay(world, x, y + 1, z);

					if (topFlowDecay >= 8)
					{
						newDecay = topFlowDecay;
					}
					else
					{
						newDecay = topFlowDecay + 8;
					}
				}

				if (_adjacentSources >= 2 && this is BlockFlowingWater)
				{
					if (world.GetBlock(new Vector3(x, y - 1, z)).IsSolid)
					{
						newDecay = 0;
					}
					else if (IsSameMaterial(world.GetBlock(new Vector3(x, y - 1, z))) &&
					         world.GetBlock(new Vector3(x, y - 1, z)).Metadata == 0)
					{
						newDecay = 0;
					}
				}

				if (this is BlockFlowingLava && currentDecay < 8 && newDecay < 8 && newDecay > currentDecay && random.Next(4) != 0)
				{
					tickRate *= 4;
				}

				if (newDecay == currentDecay)
				{
					if (flag)
					{
						SetToStill(world, x, y, z);
					}
				}
				else
				{
					currentDecay = newDecay;
					if (newDecay < 0)
					{
						world.SetBlock(new BlockAir {Coordinates = new Vector3(x, y, z)});
					}
					else
					{
						world.ApplyPhysics(x, y, z);
						world.ScheduleBlockTick(this, tickRate); // Schedule tick
					}
				}
			}
			else
			{
				SetToStill(world, x, y, z);
			}

			if (CanBeFlownInto(world, x, y - 1, z))
			{
				if (this is BlockFlowingLava &&
				    (world.GetBlock(new Vector3(x, y - 1, z)) is BlockFlowingWater ||
				     world.GetBlock(new Vector3(x, y - 1, z)) is BlockStationaryWater))
				{
					world.SetBlock(new BlockCobbleStone {Coordinates = new Vector3(x, y - 1, z)});
					return;
				}

				if (currentDecay >= 8)
				{
					Flow(world, x, y - 1, z, currentDecay);
				}
				else
				{
					Flow(world, x, y - 1, z, currentDecay + 8);
				}
			}
			else if (currentDecay >= 0 && (currentDecay == 0 || BlocksFluid(world, x, y - 1, z)))
			{
				var optimalFlowDirections = GetOptimalFlowDirections(world, x, y, z);

				var newDecay = currentDecay + multiplier;
				if (currentDecay >= 8)
				{
					newDecay = 1;
				}

				if (newDecay >= 8)
				{
					return;
				}

				if (optimalFlowDirections[0])
				{
					Flow(world, x - 1, y, z, newDecay);
				}

				if (optimalFlowDirections[1])
				{
					Flow(world, x + 1, y, z, newDecay);
				}

				if (optimalFlowDirections[2])
				{
					Flow(world, x, y, z - 1, newDecay);
				}

				if (optimalFlowDirections[3])
				{
					Flow(world, x, y, z + 1, newDecay);
				}
			}
		}

		private bool[] GetOptimalFlowDirections(Level world, int x, int y, int z)
		{
			int l;
			int x2;

			for (l = 0; l < 4; ++l)
			{
				_flowCost[l] = 1000;
				x2 = x;
				var z2 = z;

				if (l == 0)
				{
					x2 = x - 1;
				}

				if (l == 1)
				{
					++x2;
				}

				if (l == 2)
				{
					z2 = z - 1;
				}

				if (l == 3)
				{
					++z2;
				}

				if (!BlocksFluid(world, x2, y, z2) &&
				    (!IsSameMaterial(world.GetBlock(new Vector3(x2, y, z2))) || world.GetBlock(new Vector3(x2, y, z2)).Metadata != 0))
				{
					if (BlocksFluid(world, x2, y - 1, z2))
					{
						_flowCost[l] = CalculateFlowCost(world, x2, y, z2, 1, l);
					}
					else
					{
						_flowCost[l] = 0;
					}
				}
			}

			l = _flowCost[0];

			for (x2 = 1; x2 < 4; ++x2)
			{
				if (_flowCost[x2] < l)
				{
					l = _flowCost[x2];
				}
			}

			for (x2 = 0; x2 < 4; ++x2)
			{
				_optimalFlowDirections[x2] = _flowCost[x2] == l;
			}

			return _optimalFlowDirections;
		}

		private int CalculateFlowCost(Level world, int x, int y, int z, int accumulatedCost, int prevDirection)
		{
			var cost = 1000;

			for (var direction = 0; direction < 4; ++direction)
			{
				if ((direction != 0 || prevDirection != 1)
				    && (direction != 1 || prevDirection != 0)
				    && (direction != 2 || prevDirection != 3)
				    && (direction != 3 || prevDirection != 2))
				{
					var x2 = x;
					var z2 = z;

					if (direction == 0)
					{
						x2 = x - 1;
					}

					if (direction == 1)
					{
						++x2;
					}

					if (direction == 2)
					{
						z2 = z - 1;
					}

					if (direction == 3)
					{
						++z2;
					}

					if (!BlocksFluid(world, x2, y, z2) &&
					    (!IsSameMaterial(world.GetBlock(new Vector3(x2, y, z2))) ||
					     world.GetBlock(new Vector3(x2, y, z2)).Metadata != 0))
					{
						if (!BlocksFluid(world, x2, y - 1, z2))
						{
							return accumulatedCost;
						}

						if (accumulatedCost < 4)
						{
							var j2 = CalculateFlowCost(world, x2, y, z2, accumulatedCost + 1, direction);

							if (j2 < cost)
							{
								cost = j2;
							}
						}
					}
				}
			}

			return cost;
		}

		private void Flow(Level world, int x, int y, int z, int decay)
		{
			if (CanBeFlownInto(world, x, y, z))
			{
				var oldBlock = world.GetBlock(new Vector3(x, y, z));
				if (oldBlock is Flowing || oldBlock.Id == 0)
				{
				}
				else
				{
					oldBlock.DoDrop(world);
				}
				oldBlock.BreakBlock(world);

				var newBlock = BlockFactory.GetBlockById(Id);
				newBlock.Coordinates = new Vector3(x, y, z);
				newBlock.Metadata = (byte) decay;
				world.SetBlock(newBlock, applyPhysics: true);
				world.ScheduleBlockTick(newBlock, TickRate());
			}
		}

		private bool CanBeFlownInto(Level world, int x, int y, int z)
		{
			var block = world.GetBlock(new Vector3(x, y, z));

			return !IsSameMaterial(block) && (!(block is BlockFlowingLava) && !(block is BlockStationaryLava)) &&
			       !BlocksFluid(block);
		}

		private bool BlocksFluid(Level world, int x, int y, int z)
		{
			var block = world.GetBlock(new Vector3(x, y, z));

			return BlocksFluid(block);
		}

		private bool BlocksFluid(Block block)
		{
			return block.IsSolid;
		}

		private void SetToStill(Level world, int x, int y, int z)
		{
			var meta = world.GetBlock(new Vector3(x, y, z)).Metadata;

			var stillBlock = BlockFactory.GetBlockById((byte) (Id + 1));
			stillBlock.Metadata = meta;
			stillBlock.Coordinates = new Vector3(x, y, z);
			world.SetBlock(stillBlock, applyPhysics: false);
		}

		private int GetSmallestFlowDecay(Level world, int x, int y, int z, int decay)
		{
			var blockDecay = GetFlowDecay(world, x, y, z);

			if (blockDecay < 0)
			{
				return decay;
			}

			if (blockDecay == 0)
			{
				++_adjacentSources;
			}

			if (blockDecay >= 8)
			{
				blockDecay = 0;
			}

			return decay >= 0 && blockDecay >= decay ? decay : blockDecay;
		}

		private int GetFlowDecay(Level world, int x, int y, int z)
		{
			var block = world.GetBlock(new Vector3(x, y, z));
			return IsSameMaterial(block) ? block.Metadata : -1;
		}

		private bool IsSameMaterial(Block block)
		{
			if (this is BlockFlowingWater && (block is BlockFlowingWater || block is BlockStationaryWater)) return true;
			if (this is BlockFlowingLava && (block is BlockFlowingLava || block is BlockStationaryLava)) return true;

			return false;
		}

		private int TickRate()
		{
			return this is BlockFlowingWater ? 5 : (this is BlockFlowingLava ? 30 : 0);
		}

		private void CheckForHarden(Level world, int x, int y, int z)
		{
			var block = world.GetBlock(new Vector3(x, y, z));
			{
				var harden = false;
				if (block is BlockFlowingLava || block is BlockStationaryLava)
				{
					if (IsWater(world, x, y, z - 1))
					{
						harden = true;
					}

					if (harden || IsWater(world, x, y, z + 1))
					{
						harden = true;
					}

					if (harden || IsWater(world, x - 1, y, z))
					{
						harden = true;
					}

					if (harden || IsWater(world, x + 1, y, z))
					{
						harden = true;
					}

					if (harden || IsWater(world, x, y + 1, z))
					{
						harden = true;
					}

					if (harden)
					{
						int meta = block.Metadata;

						if (meta == 0)
						{
							world.SetBlock(new BlockObsidian {Coordinates = new Vector3(x, y, z)});
						}
						else if (meta <= 4)
						{
							world.SetBlock(new BlockCobbleStone {Coordinates = new Vector3(x, y, z)});
						}
					}
				}
			}
		}

		private bool IsWater(Level world, int x, int y, int z)
		{
			var block = world.GetBlock(new Vector3(x, y, z));
			return block is BlockFlowingWater || block is BlockStationaryWater;
		}
	}
}