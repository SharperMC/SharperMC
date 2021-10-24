using System;

namespace SharperMC.Core.Utils.Enums
{
    public class BlockFace
    {
        private int Face { get; }
        
        public BlockFace(int face)
        {
            Face = face;
        }

        public EnumFace GetFace()
        {
            return (EnumFace)Face;
        }
        
        public enum EnumFace
        {
            NegativeY = 0,
            PositiveY = 1,
            NegativeZ = 2,
            PositiveZ = 3,
            NegativeX = 4,
            PositiveX = 5
        }
    }
}