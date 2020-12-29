using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Utils.Vectors;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Blocks.Stairs
{
	public class StairsBlock : Block
	{
		internal StairsBlock(ushort id) : base(id)
		{
			FuelEfficiency = 15;
		}

		public override bool PlaceBlock(Level world, Player player, Vector3 blockCoordinates, BlockFace face, Vector3 mouseLocation)
		{
			byte direction = player.GetDirection();
			byte upper = (byte)((mouseLocation.Y >= 8 && face != BlockFace.PositiveY) || face == BlockFace.NegativeY ? 0x04 : 0x00);
			switch (direction)
			{
				case 0:
					Metadata = (byte)(0 | upper);
					break;
				case 1:
					Metadata = (byte)(2 | upper);
					break;
				case 2:
					Metadata = (byte)(1 | upper);
					break;
				case 3:
					Metadata = (byte)(3 | upper);
					break;
			}
			world.SetBlock(this);
			return true;
		}
	}
}
