using GBAHL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crying
{
    /// <summary>
    /// Represents a cry.
    /// </summary>
    internal class Cry
    {
        /// <summary>
        /// Gets or sets the index of the cry.
        /// </summary>
        public int Index { get; set; } = 0;

        /// <summary>
        /// Gets or sets the offset of the cry.
        /// </summary>
        public Ptr Offset { get; set; } = Ptr.Zero;

        /// <summary>
        /// Gets or sets whether the cry is compressed.
        /// </summary>
        public bool IsCompressed { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the cry is looped.
        /// </summary>
        public bool Looped { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the sample rate.
        /// </summary>
        public int SampleRate { get; set; } = 0;

        /// <summary>
        /// Gets or sets the sample the cry loops to. 
        /// </summary>
        public int LoopTo { get; set; } = 0;

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public sbyte[] Data { get; set; } = null;

        /// <summary>
        /// Gets the size of the cry in bytes.
        /// </summary>
        public int Size => 16 + (Data == null ? 0 : Data.Length);

        /// <summary>
        /// Gets or sets the original size of the cry in bytes.
        /// </summary>
        public int OriginalSize { get; set; } = 0;

        /// <summary>
        /// Determines whether this <see cref="Cry"/> is valid.
        /// </summary>
        public bool IsValid => Offset.IsValid && !Offset.IsZero;

        // The compression lookup table
        // { 0x0, 0x1, 0x4, 0x9, 0x10, 0x19, 0x24, 0x31, 0xC0, 0xCF, 0xDC, 0xE7, 0xF0, 0xF7, 0xFC, 0xFF };
        public static readonly sbyte[] Lookup = {
              0,   1,   4,   9,
             16,  25,  36,  49,
            -64, -49, -36, -25,
            -16,  -9,  -4,  -1
        };

        // Finds the closest index in the lookup table
        public static int IndexOfClosestLookup(int value)
        {
            // Determine the closest lookup entry
            int bestDifference = Math.Abs(Lookup[0] - value);
            int bestIndex      = 0;

            for (int i = 1; i < 16 /* Lookup.Length */; i++)
            {
                if (Lookup[i] == value)
                {
                    bestIndex = i;
                    break;
                }

                int difference = Math.Abs(Lookup[i] - value);
                if (difference < bestDifference)
                {
                    bestDifference = difference;
                    bestIndex      = i;
                }
            }

            return bestIndex;
        }

        /// <summary>
        /// Compresses the a cry's data.
        /// </summary>
        /// <returns></returns>
        public static sbyte[] Compress(sbyte[] data)
        {
            // Data is compressed in blocks of 1 + 0x20 bytes at a time
            // First byte is normal signed PCM data
            // Following 0x20 bytes are compressed based on previous value
            if (data == null || data.Length == 0)
                return null;

            // Calculate number of needed blocks
            int blockCount = data.Length / 0x40;
            if (data.Length % 0x40 > 0)
                blockCount++;

            // Calculate the length of the last block to save space
            int lastBlockSize = data.Length - data.Length / 0x40 * 0x40;
            if (lastBlockSize == 0)
                lastBlockSize = 0x21;
            else
                lastBlockSize = 1 + (lastBlockSize / 2) + (lastBlockSize % 2 == 0 ? 0 : 1);

            // ------------------------------
            // Compress all blocks
            byte[][] blocks = new byte[blockCount][];
            for (int n = 0; n < blockCount; n++)
            {
                if (n < blockCount - 1)
                    blocks[n] = new byte[0x21];
                else
                    blocks[n] = new byte[lastBlockSize];

                int i = n * 0x40;   // position in source
                int k = 0;          // position in block

                // Set first sample
                // Upper nybble is skipped
                sbyte previous = data[i++];
                blocks[n][k++] = (byte)previous;

                // Compress rest of block
                for (int j = 1; j < 0x40 && i < data.Length; j++)
                {
                    var sample = data[i++];
                    var difference = sample - previous;
                    
                    // Determine the index of the difference in the lookup table
                    var lookup = IndexOfClosestLookup(difference);

                    // set value in block
                    // on an odd value, increase position in block
                    if (j % 2 == 0)
                    {
                        blocks[n][k] |= (byte)(lookup << 4);
                    }
                    else
                    {
                        blocks[n][k++] |= (byte)lookup;
                    }

                    // Update previous
                    previous = (sbyte)(previous + Lookup[lookup]);
                }
            }

            // Copy blocks into output array
            sbyte[] compressed = new sbyte[blocks.Sum(block => block.Length)];
            for (int i = 0; i < blockCount; i++)
            {
                //int blockSize = i != (blockCount - 1) ? 0x21 : lastBlockSize;
                int blockSize = blocks[i].Length;
                for (int j = 0; j < blockSize; j++)
                {
                    compressed[i * 0x21 + j] = (sbyte)blocks[i][j];
                }
            }

            return compressed;
        }
    }

    #region Old Compression Method
    /*
    List<byte> data = new List<byte>();
    if (cry.IsCompressed)
    {
        // ------------------------------
        // data is compressed in blocks of 1 + 0x20 bytes at a time
        // first byte is normal signed PCM data
        // following 0x20 bytes are compressed based on previous value

        // compression lookup table
        // { 0x0, 0x1, 0x4, 0x9, 0x10, 0x19, 0x24, 0x31, 0xC0, 0xCF, 0xDC, 0xE7, 0xF0, 0xF7, 0xFC, 0xFF };
        var lookup = new sbyte[] { 0, 1, 4, 9, 16, 25, 36, 49, -64, -49, -36, -25, -16, -9, -4, -1 };

        // ------------------------------
        // calculate number of needed blocks
        var blockCount = cry.Data.Length / 0x40;
        if (cry.Data.Length % 0x40 > 0) blockCount++;

        // ------------------------------
        // calculate the length of the last block to save space
        var lastBlockSize = cry.Data.Length - cry.Data.Length / 0x40 * 0x40;
        if (lastBlockSize == 0)
            lastBlockSize = 0x21;
        else
            lastBlockSize = 1 + (lastBlockSize / 2) + (lastBlockSize % 2 == 0 ? 0 : 1);

        // ------------------------------
        // compress all blocks
        var blocks = new byte[blockCount][];
        for (int n = 0; n < blockCount; n++)
        {
            // ------------------------------
            // create new block
            if (n < blockCount - 1)
                blocks[n] = new byte[0x21];
            else
                blocks[n] = new byte[lastBlockSize];

            int i = n * 0x40;   // position in source
            int k = 0;          // position in block

            // ------------------------------
            // set first sample
            // upper nybble is skipped
            blocks[n][k++] = (byte)cry.Data[i];
            sbyte pcm = cry.Data[i++];

            // ------------------------------
            // compress rest of block
            for (int j = 1; j < 0x40 && i < cry.Data.Length; j++)
            {
                // get current sample
                var sample = cry.Data[i++];

                // difference between previous sample and this
                var diff = sample - pcm;

                // ------------------------------
                // check for a perfect match in lookup table
                var lookupI = -1;
                for (int x = 0; x < 16; x++)
                {
                    if (lookup[x] == diff)
                    {
                        lookupI = x;
                        break;
                    }
                }

                // ------------------------------
                // search for the closest match in the table (perfect match not found)
                if (lookupI == -1)
                {
                    var bestDiff = 255;
                    for (int x = 0; x < 16; x++)
                    {
                        if (Math.Abs(lookup[x] - diff) < bestDiff)
                        {
                            lookupI = x;
                            bestDiff = Math.Abs(lookup[x] - diff);
                        }
                    }
                }

                // set value in block
                // on an odd value, increase position in block
                if (j % 2 == 0)
                    blocks[n][k] |= (byte)(lookupI << 4);
                else
                    blocks[n][k++] |= (byte)lookupI;

                // set previous
                pcm = (sbyte)(pcm + lookup[lookupI]);
            }
        }

        // ------------------------------
        // copy blocks to output list
        for (int n = 0; n < blockCount; n++)
            data.AddRange(blocks[n]);
    }
    else
    {
        // ------------------------------
        // uncompressed, copy directly to output
        foreach (var s in cry.Data)
            data.Add((byte)s);
    }
    */
    #endregion
}
