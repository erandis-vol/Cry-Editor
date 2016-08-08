using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crying
{
    struct Cry
    {
        public int Index;
        public int Offset;

        public bool Compressed;
        public bool Looped;
        public int SampleRate;
        public int LoopStart;
        public int Size;

        public sbyte[] Data;
    }
}
