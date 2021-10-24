using SharperMC.Core.Blocks;
using SharperMC.Core.Structures;

namespace SharperMC.Core.Biomes
{
    public class BiomeBase : IBiome
    {
        public virtual double BaseHeight => 52.0;

        public virtual int Id => 0;

        public virtual byte MinecraftBiomeId => 0;

        public virtual int MaxTrees => 10;

        public virtual int MinTrees => 0;

        public virtual Structure[] TreeStructures
        {
            get { return new Structure[] {new OakTree()}; }
        }

        public virtual ChunkDecorator[] Decorators
        {
            get { return new ChunkDecorator[] {new TreeDecorator()}; }
        }

        public virtual float Temperature => 0.0f;

        public virtual Block TopBlock => new BlockGrass();

        public virtual Block Filling => new Block(3);
    }
}