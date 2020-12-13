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
	public class Seed : Item
	{
		public Block BecomesBlock { get; private set; }
		internal Seed(ushort id, byte metadata) : base(id, metadata)
		{
		}

		public override void UseItem(Level world, Player player, Vector3 blockCoordinates, BlockFace face)
		{
			
		}
	}
}
