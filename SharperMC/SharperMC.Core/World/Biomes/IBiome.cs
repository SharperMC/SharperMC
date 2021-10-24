using SharperMC.Core.Blocks;
using SharperMC.Core.Structures;

namespace SharperMC.Core.Biomes
{
    public interface IBiome
    {
        int Id { get; }
        byte MinecraftBiomeId { get; }
        int MaxTrees { get; }
        int MinTrees { get; }
        Structure[] TreeStructures { get; }
        ChunkDecorator[] Decorators { get; }
        float Temperature { get; }
        Block TopBlock { get; }
        Block Filling { get; }
    }
}