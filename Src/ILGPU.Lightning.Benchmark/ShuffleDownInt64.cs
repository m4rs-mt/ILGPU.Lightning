using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ILGPU.ShuffleOperations;

namespace ILGPU.Lightning.Benchmark
{
    public readonly struct ShuffleDownInt64 : IShuffleDown<long>
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Int2
        {
            public int X;
            public int Y;
        }

        /// <inheritdoc />
        public long ShuffleDown(long variable, int delta)
        {
            var source = Unsafe.As<long, Int2>(ref variable);
            var result = new Int2()
            {
                X = Warp.ShuffleDown(source.X, delta),
                Y = Warp.ShuffleDown(source.Y, delta),
            };
            return Unsafe.As<Int2, long>(ref result);
        }
    }
}