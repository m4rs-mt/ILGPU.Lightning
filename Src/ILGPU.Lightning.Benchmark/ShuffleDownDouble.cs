using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ILGPU.ShuffleOperations;

namespace ILGPU.Lightning.Benchmark
{
    public readonly struct ShuffleDownDouble : IShuffleDown<double>
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Int2
        {
            public int X;
            public int Y;
        }

        /// <inheritdoc />
        public double ShuffleDown(double variable, int delta)
        {
            var source = Unsafe.As<double, Int2>(ref variable);
            var result = new Int2()
            {
                X = Warp.ShuffleDown(source.X, delta),
                Y = Warp.ShuffleDown(source.Y, delta),
            };
            return Unsafe.As<Int2, double>(ref result);
        }
    }
}