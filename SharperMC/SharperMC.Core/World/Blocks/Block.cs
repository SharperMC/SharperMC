using SharperMC.Core.Entities.Player;
using SharperMC.Core.Items;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.World;
using SharperMC.Core.Utils.World.Vectors;

namespace SharperMC.Core.Blocks
{
    public class Block
    {
        public Block(ushort id, Location location)
        {
            Id = id;
            Location = location;

            
            Drops = new[] {new ItemStack((short)id, 1)};
        }
        
        public readonly ushort Id;
        public Location Location;

        public bool IsReplaceable { get; set; } = false;
        public bool IsTransparent { get; set; } = false;
        public bool IsSolid { get; set; } = true;
        public bool IsPlaceable { get; set; } = true;

        public float Durability { get; set; }
        public float Slipperiness { get; set; }
        
        public ItemStack[] Drops { get; set; }

        public void DropItems()
        {
            if(Drops == null || Drops.Length == 0) 
                return;
            //TODO
        }

        public virtual bool Place(World world, Player player, Location location, BlockFace face, Vector3D cursor)
        {
            return false;
        }
        
        public virtual bool Place(World world, Location location, BlockFace face)
        {
            return false;
        }
        
        public virtual bool Place(World world, Location location)
        {
            return false;
        }

        public virtual void Break(World world)
        {
            //Todo
        }
        
        public virtual void OnTick(World world) {}
        public virtual void OoPhysics(World world) {}
    }
}