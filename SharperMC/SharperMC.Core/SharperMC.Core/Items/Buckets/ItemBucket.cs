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
using SharperMC.Core.Blocks.Misc;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils.Vectors;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Items.Buckets
{
	public class ItemBucket : Item
	{
		internal ItemBucket() : base(325, 0)
		{
			IsUsable = true;
		}

		public override void UseItem(Level world, Player player, Vector3 blockCoordinates, BlockFace face)
		{
			blockCoordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			Block block = world.GetBlock(blockCoordinates);

			int slot = 0;
			Item hand = player.Inventory.GetItemInHand();
			if (hand.Id.Equals(Id))
			{ 
				slot = player.Inventory.CurrentSlot + 36;
			}
			
			//player.SendChat("Block: " + bl.Id, ChatColor.Bold);
			if (block.Id == 65535) 
				return;
			switch (block.Id)
			{
				case 8: //Water
					player.Inventory.SetSlot(slot, 326, 0, 1);
					world.SetBlock(new BlockAir() {Coordinates = blockCoordinates}, true, true);
					break;
				case 10:
					player.Inventory.SetSlot(slot, 327, 0, 1);
					world.SetBlock(new BlockAir() { Coordinates = blockCoordinates }, true, true);
					break;
			}
		}
	}
}