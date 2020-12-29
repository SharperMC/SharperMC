﻿using System.Collections;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils.Vectors;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Blocks.Slabs
{
	public class Slab : Block
	{
		internal Slab(byte metadata) : base(44)
		{
			Metadata = metadata;
		}

		public override bool PlaceBlock(Level world, Player player, Vector3 blockCoordinates, BlockFace face, Vector3 mouseLocation)
		{
			var prevblock = world.GetBlock(Coordinates);
			if (prevblock.Id == Id && prevblock.Metadata == Metadata)
			{
				DoubleSlab ds = new DoubleSlab(Metadata) {Coordinates = Coordinates};
				world.SetBlock(ds);
			}
			else if (prevblock.Id == Id && prevblock.Metadata != Metadata)
			{
				if (player.Gamemode != Gamemode.Creative)
				{
					player.Inventory.AddItem((short)Id, Metadata, 1);
				}
				return true;
			}
			else
			{
				bool upper = ((mouseLocation.Y >= 8 && face != BlockFace.PositiveY) || face == BlockFace.NegativeY);
				BitArray b = new BitArray(new byte[] {Metadata});
				b[3] = upper;
				Metadata = ConvertToByte(b);
				world.SetBlock(this);
			}
			return true;
		}
	}
}
