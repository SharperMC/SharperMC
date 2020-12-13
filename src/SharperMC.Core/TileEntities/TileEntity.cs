﻿using System.Collections.Generic;
using fNbt;
using SharperMC.Core.Utils;
using SharperMC.Core.Worlds;

namespace SharperMC.Core.TileEntities
{
	public class TileEntity
	{
		public string Id { get; private set; }
		public Vector3 Coordinates { get; set; }
		public bool UpdatesOnTick { get; set; }

		public TileEntity(string id)
		{
			Id = id;
		}

		public virtual NbtCompound GetCompound()
		{
			return new NbtCompound();
		}

		public virtual void SetCompound(NbtCompound compound)
		{
		}

		public virtual void OnTick(Level level)
		{
		}


		public virtual List<ItemStack> GetDrops()
		{
			return new List<ItemStack>();
		}
	}
}
