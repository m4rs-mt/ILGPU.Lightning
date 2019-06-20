using System;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Horology;
using ILGPU.Runtime.Cuda;

namespace ILGPU.Lightning.Benchmark
{
    public class GpuClock : IClock, IDisposable
    {
        private const string LibName = "nvcuda";

        private int _num;
        private readonly IntPtr _start;
        private readonly IntPtr _stop;


        public GpuClock(BaseBenchmark benchmark)
        {
            Benchmark = benchmark;

            CudaException.ThrowIfFailed(cuEventCreate(out _start, 0));
            CudaException.ThrowIfFailed(cuEventCreate(out _stop, 0));
        }

        public void Dispose()
        {
            CudaException.ThrowIfFailed(cuEventDestroy(_start));
            CudaException.ThrowIfFailed(cuEventDestroy(_stop));
        }

        public BaseBenchmark Benchmark { get; }

        public long GetTimestamp()
        {
            // Start 1 - End 2, Start 3 - End 4 ...
            _num++;
            var start = (_num & 1) != 0;

            if (start)
            {
                CudaException.ThrowIfFailed(cuEventRecord(_start, ((CudaStream)Benchmark.Accelerator.DefaultStream).StreamPtr));
                return 0;
            }
            else
            {
                CudaException.ThrowIfFailed(cuEventRecord(_stop, ((CudaStream)Benchmark.Accelerator.DefaultStream).StreamPtr));
                CudaException.ThrowIfFailed(cuEventSynchronize(_stop));
                CudaException.ThrowIfFailed(cuEventElapsedTime(out var pMilliseconds, _start, _stop));

                return (long)Math.Round(pMilliseconds * 1000000);
            }
        }

        public string Title => "Gpu Event";
        public bool IsAvailable => true;

        public Frequency Frequency { get; } = Frequency.FromGHz(1);


        [DllImport(LibName)]
        private static extern CudaError cuEventCreate(out IntPtr phEvent, uint Flags);

        [DllImport(LibName)]
        private static extern CudaError cuEventRecord(IntPtr hEvent, IntPtr hStream);

        [DllImport(LibName)]
        private static extern CudaError cuEventSynchronize(IntPtr hEvent);

        [DllImport(LibName)]
        private static extern CudaError cuEventElapsedTime(out float pMilliseconds, IntPtr hStart, IntPtr hEnd);

        [DllImport(LibName)]
        private static extern CudaError cuEventDestroy(IntPtr hEvent);
    }
}