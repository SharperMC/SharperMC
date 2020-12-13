﻿using System.Collections;
using SharperMC.Core.Blocks;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Items
{
	public class InfoTool : Item
	{
		internal InfoTool() : base(286,0)
		{
			IsUsable = true;
		}

		public override void UseItem(Level world, Player player, Vector3 blockCoordinates, BlockFace face)
		{
			var blockatpos = world.GetBlock(blockCoordinates);
			if (!(blockatpos is BlockAir))
			{
				BitArray b = new BitArray(new byte[] {blockatpos.Metadata});
				ConsoleFunctions.WriteLine("\n\n");
				ConsoleFunctions.WriteInfoLine("------------------------------------");
				ConsoleFunctions.WriteInfoLine("Block: " + blockatpos);
				ConsoleFunctions.WriteInfoLine("------------------------------------");
				for (int i = 0; i < b.Count; i++)
				{
					ConsoleFunctions.WriteInfoLine("Bit " + i + ": " + b[i]);
				}
				ConsoleFunctions.WriteInfoLine("------------------------------------\n\n");
				player.SendChat("Info tool used, Metadata written to chat!", ChatColor.Gold);
			}
		}
	}
}
