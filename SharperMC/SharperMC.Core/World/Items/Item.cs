using SharperMC.Core.Entities.Player;
using SharperMC.Core.Utils.Enums;
using SharperMC.Core.Utils.World;

namespace SharperMC.Core.Items
{
    public class Item
    {
        public Item(ushort id, byte metadata)
        {
            Id = id;
            MetaData = metadata;
        }
        
        public readonly ushort Id;
        public readonly byte MetaData;
        
        public bool IsConsumable { get; set; } = false;
        public bool IsBlock { get; set; } = false;
        
        public int MaxStackSize { get; set; } = 64;
        public double ItemDamage { get; set; } = 1;
        
        public virtual void UseItem(World world, Player player, Location location, BlockFace face)
        {
        }

        public ItemStack GetItemStack()
        {
            return new ((short) Id, 1, MetaData);
        }
    }
}