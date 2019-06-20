using BenchmarkDotNet.Attributes;
using ILGPU.ReductionOperations;
using ILGPU.Runtime;
using ILGPU.Sequencers;
using ILGPU.ShuffleOperations;

namespace ILGPU.Lightning.Benchmark
{
    public class ReduceBenchmark : BaseBenchmark
    {
        private MemoryBuffer<int> _buffer;
        private MemoryBuffer<int> _output;
        private Reduction<int, ShuffleDownInt32, AtomicAddInt32> _reductionInt32;

        public override void GlobalSetup()
        {
            base.GlobalSetup();

            _buffer = Accelerator.Allocate<int>(Length);
            Accelerator.Sequence(Accelerator.DefaultStream, _buffer.View, new Int32Sequencer());

            _output = Accelerator.Allocate<int>(1);

            _reductionInt32 = Accelerator.CreateReduction<int, ShuffleDownInt32, AtomicAddInt32>();
        }

        public override void GlobalCleanup()
        {
            _buffer?.Dispose();
            _buffer = null;

            _output?.Dispose();
            _output = null;

            base.GlobalCleanup();
        }

        [Benchmark(Baseline = true)]
        public void ReduceInt32()
        {
            _reductionInt32(
                Accelerator.DefaultStream,
                _buffer.View,
                _output.View,
                new ShuffleDownInt32(),
                new AtomicAddInt32());
        }
    }
}