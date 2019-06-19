using System;
using System.Threading;
using BenchmarkDotNet.Attributes;
using ILGPU.ReductionOperations;
using ILGPU.Runtime;
using ILGPU.Sequencers;
using ILGPU.ShuffleOperations;

namespace ILGPU.Lightning.Benchmark
{
    public class ReduceBenchmark : BaseBenchmark
    {
        public MemoryBuffer<int> Buffer { get; private set; }
        public MemoryBuffer<int> Output { get; private set; }

        public override void GlobalSetup()
        {
            base.GlobalSetup();

            Buffer = Accelerator.Allocate<int>(Length);
            Accelerator.Sequence(Accelerator.DefaultStream, Buffer.View, new Int32Sequencer());

            Output = Accelerator.Allocate<int>(1);
        }

        public override void GlobalCleanup()
        {
            Buffer?.Dispose();
            Buffer = null;

            Output?.Dispose();
            Output = null;

            base.GlobalCleanup();
        }


        [Benchmark(Baseline = true)]
        public void ReduceInt32()
        {
            Accelerator.Reduce(
                Accelerator.DefaultStream,
                Buffer.View,
                Output.View,
                new ShuffleDownInt32(),
                new AtomicAddInt32());
        }
    }
}