namespace Crying
{
    class Cry
    {
        public int Index { get; set; } = 0;
        public int Offset { get; set; } = 0;
        public bool Compressed { get; set; } = false;
        public bool Looped { get; set; } = false;
        public int SampleRate { get; set; } = 0;
        public int LoopStart { get; set; } = 0;

        public sbyte[] Data { get; set; } = null;

        /// <summary>
        /// Gets or sets the number of bytes this <see cref="Cry"/> originally filled in the ROM.
        /// </summary>
        public int OriginalSize { get; set; } = 0;
    }
}
