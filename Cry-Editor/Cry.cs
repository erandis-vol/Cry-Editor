using GBAHL;

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
        public int Size => Data == null ? 0 : Data.Length;

        /// <summary>
        /// Gets or sets the original size of the cry in bytes.
        /// </summary>
        public int OriginalSize { get; set; } = 0;

        /// <summary>
        /// Determines whether this <see cref="Cry"/> is valid.
        /// </summary>
        public bool IsValid => Offset.IsValid && !Offset.IsZero;
    }
}
