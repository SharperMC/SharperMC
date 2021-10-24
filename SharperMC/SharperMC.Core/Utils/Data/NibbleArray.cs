﻿using fNbt;
using fNbt.Serialization;

namespace SharperMC.Core.Utils.Data
{
    /// <summary>
    ///     Represents an array of 4-bit values.
    /// </summary>
    public class NibbleArray : INbtSerializable
    {
        public NibbleArray()
        {
        }

        /// <summary>
        ///     Creates a new nibble array with the given number of nibbles.
        /// </summary>
        public NibbleArray(int length)
        {
            Data = new byte[length/2];
        }

        /// <summary>
        ///     The data in the nibble array. Each byte contains
        ///     two nibbles, stored in big-endian.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        ///     Gets the current number of nibbles in this array.
        /// </summary>
        [NbtIgnore]
        public int Length => Data.Length*2;

        /// <summary>
        ///     Gets or sets a nibble at the given index.
        /// </summary>
        [NbtIgnore]
        public byte this[int index]
        {
            get => (byte) (Data[index/2] >> ((index)%2*4) & 0xF);
            set
            {
                value &= 0xF;
                Data[index/2] &= (byte) (0xF << ((index + 1)%2*4));
                Data[index/2] |= (byte) (value << (index%2*4));
            }
        }

        public NbtTag Serialize(string tagName)
        {
            return new NbtByteArray(tagName, Data);
        }

        public void Deserialize(NbtTag value)
        {
            Data = value.ByteArrayValue;
        }
    }
}