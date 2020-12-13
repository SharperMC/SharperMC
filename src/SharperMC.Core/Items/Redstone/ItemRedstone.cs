using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharperMC.Core.Blocks;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Items
{
	public class ItemRedstone : Item
	{
		internal ItemRedstone() : base(331, 0)
		{
			IsUsable = true;
		}

		public override void UseItem(Level world, Player player, Vector3 blockCoordinates, BlockFace face)
		{
			blockCoordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			var d = new BlockRedstoneDust {Coordinates = blockCoordinates};
			//d.SetPowerLevel(new Random().Next(0,15));
			world.SetBlock(d, true, true);
		}
	}
}
