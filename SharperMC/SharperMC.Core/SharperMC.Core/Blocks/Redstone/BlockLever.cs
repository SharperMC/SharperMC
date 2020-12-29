﻿using System;
using System.Collections;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils.Vectors;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Blocks.Redstone
{
	public class BlockLever : RedstonePowerSource
	{
		internal BlockLever() : base(69)
		{
			IsUsable = true;
			IsReplacible = false;
			IsSolid = false;
		}

		public override bool IsActive()
		{
			var bits = new BitArray(new byte[] { Metadata });
			return bits[3];
		}

		public override bool PlaceBlock(Level world, Player player, Vector3 blockCoordinates, BlockFace face, Vector3 mouseLocation)
		{
			var rawbits = new BitArray(new byte[] { 0x00 });

			byte direction = player.GetDirection();
			if (face == BlockFace.PositiveY)
			{
				switch (direction)
				{
					case 0:
						//South
						rawbits[1] = true;
						rawbits[2] = true;
						break;
					case 1:
						//West
						rawbits[0] = true;
						rawbits[2] = true;
						break;
					case 2:
						//North
						rawbits[1] = true;
						rawbits[2] = true;
						break;
					case 3:
						//East
						rawbits[0] = true;
						rawbits[2] = true;
						break;
				}
			}
			else if (face == BlockFace.NegativeY)
			{
				rawbits[0] = true;
				rawbits[1] = true;
				rawbits[2] = true;
			}
			else if (face == BlockFace.PositiveZ)
			{
				rawbits[0] = true;
				rawbits[1] = true;
			}
			else if (face == BlockFace.NegativeZ)
			{
				rawbits[2] = true;
			}
			else if (face == BlockFace.PositiveX)
			{
				rawbits[0] = true;
			}
			else if (face == BlockFace.NegativeX)
			{
				rawbits[1] = true;
			}

			Metadata = ConvertToByte(rawbits);

			world.SetBlock(this);
			return true;
		}

		public override void UseItem(Level world, Player player, Vector3 blockCoordinates, BlockFace face)
		{
			var bits = new BitArray(new byte[] { Metadata });

			if (bits[3])
			{
				bits[3] = false;
			}
			else
			{
				bits[3] = true;
			}

			Metadata = ConvertToByte(bits);
			world.SetBlock(this);
		}

		private byte ConvertToByte(BitArray bits)
		{
			if (bits.Count != 8)
			{
				throw new ArgumentException("bits");
			}
			byte[] bytes = new byte[1];
			bits.CopyTo(bytes, 0);
			return bytes[0];
		}
	}
}
