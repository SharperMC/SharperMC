using SharperMC.Core.Blocks;
using SharperMC.Core.Blocks.Redstone;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils.Vectors;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Items.Redstone
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
