namespace SharperMC.Core.Items
{
    public class ItemStack
    {
        public ItemStack(short id, byte itemCount)
        {
            Id = id;
            ItemCount = itemCount;
        }

        public ItemStack(short id, byte itemCount, byte metaData)
        {
            Id = id;
            ItemCount = itemCount;
            MetaData = metaData;
        }

        public short Id { get; }
        public int ItemCount { get; }
        public byte MetaData { get; }
        
        public byte Nbt { get; set; }
    }
}