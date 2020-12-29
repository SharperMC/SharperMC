using System.Collections;
using SharperMC.Core.Blocks;
using SharperMC.Core.Blocks.Decor;
using SharperMC.Core.Entity;
using SharperMC.Core.Enums;
using SharperMC.Core.Networking.Packets.Play.Client;
using SharperMC.Core.Utils.Vectors;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.Items.Decor
{
	public class ItemSign : Item
	{
		internal ItemSign() : base(323, 0)
		{
			IsUsable = true;
		}

		public override void UseItem(Level world, Player player, Vector3 blockCoordinates, BlockFace face)
		{
			blockCoordinates = GetNewCoordinatesFromFace(blockCoordinates, face);
			if (face == BlockFace.PositiveY)
			{
				var bss = new BlockStandingSign
				{
					Coordinates = blockCoordinates,
					Metadata = 0x00
				};

				var rawbytes = new BitArray(new byte[] {bss.Metadata});
				
				var direction = player.GetDirection();
				switch (direction)
				{
					case 0:
						//South
						rawbytes[2] = true;
						break;
					case 1:
						//West
						rawbytes[3] = true;
						break;
					case 2:
						//North DONE
						rawbytes[2] = true;
						rawbytes[3] = true;
						break;
					case 3:
						//East
						
						break;
				}
				bss.Metadata = ConvertToByte(rawbytes);
				world.SetBlock(bss);
				new OpenSignEditor(player.Wrapper)
				{
					Coordinates = blockCoordinates
				}.Write();
			}
			else
			{
				//TODO: implement wall signs
			}
		}
	}
}
