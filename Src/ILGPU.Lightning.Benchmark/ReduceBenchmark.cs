using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using ILGPU.ReductionOperations;
using ILGPU.Runtime;
using ILGPU.Sequencers;
using ILGPU.ShuffleOperations;

namespace ILGPU.Lightning.Benchmark
{
    public class ReduceBenchmark : BaseBenchmark
    {
        private MemoryBuffer _buffer;
        private MemoryBuffer _output;
        private Action _reduce;

        public override void GlobalCleanup()
        {
            _buffer?.Dispose();
            _buffer = null;

            _output?.Dispose();
            _output = null;

            base.GlobalCleanup();
        }

        #region AddInt32

        [GlobalSetup(Target = nameof(ReduceAddInt32))]
        public void GlobalSetupAddInt32()
        {
            base.GlobalSetup();

            var buffer = Accelerator.Allocate<int>(Length);
            Accelerator.Sequence(Accelerator.DefaultStream, buffer.View, new Int32Sequencer());
            _buffer = buffer;

            var output = Accelerator.Allocate<int>(1);
            _output = output;

            var reduction = Accelerator.CreateReduction<int, ShuffleDownInt32, AtomicAddInt32>();
            _reduce = () =>
            {
                reduction(
                    Accelerator.DefaultStream,
                    buffer.View,
                    output.View,
                    new ShuffleDownInt32(),
                    new AtomicAddInt32());
            };
        }

        [Benchmark(Baseline = true)]
        public void ReduceAddInt32()
        {
            _reduce();
        }

        #endregion

        #region AddFloat

        [GlobalSetup(Target = nameof(ReduceAddFloat))]
        public void GlobalSetupAddFloat()
        {
            base.GlobalSetup();

            var buffer = Accelerator.Allocate<float>(Length);
            Accelerator.Sequence(Accelerator.DefaultStream, buffer.View, new FloatSequencer());
            _buffer = buffer;

            var output = Accelerator.Allocate<float>(1);
            _output = output;

            var reduction = Accelerator.CreateReduction<float, ShuffleDownFloat, AtomicAddFloat>();
            _reduce = () =>
            {
                reduction(
                    Accelerator.DefaultStream,
                    buffer.View,
                    output.View,
                    new ShuffleDownFloat(),
                    new AtomicAddFloat());
            };
        }

        [Benchmark]
        public void ReduceAddFloat()
        {
            _reduce();
        }

        #endregion

        #region AddInt64

        [GlobalSetup(Target = nameof(ReduceAddInt64))]
        public void GlobalSetupAddInt64()
        {
            base.GlobalSetup();

            var buffer = Accelerator.Allocate<long>(Length);
            Accelerator.Sequence(Accelerator.DefaultStream, buffer.View, new Int64Sequencer());
            _buffer = buffer;

            var output = Accelerator.Allocate<long>(1);
            _output = output;

            var reduction = Accelerator.CreateReduction<long, ShuffleDownInt64, AtomicAddInt64>();
            _reduce = () =>
            {
                reduction(
                    Accelerator.DefaultStream,
                    buffer.View,
                    output.View,
                    new ShuffleDownInt64(),
                    new AtomicAddInt64());
            };
        }

        [Benchmark]
        public void ReduceAddInt64()
        {
            _reduce();
        }

        #endregion

        #region AddDouble

        [GlobalSetup(Target = nameof(ReduceAddDouble))]
        public void GlobalSetupAddDouble()
        {
            base.GlobalSetup();

            var buffer = Accelerator.Allocate<double>(Length);
            Accelerator.Sequence(Accelerator.DefaultStream, buffer.View, new DoubleSequencer());
            _buffer = buffer;

            var output = Accelerator.Allocate<double>(1);
            _output = output;

            var reduction = Accelerator.CreateReduction<double, ShuffleDownDouble, AtomicAddDouble>();
            _reduce = () =>
            {
                reduction(
                    Accelerator.DefaultStream,
                    buffer.View,
                    output.View,
                    new ShuffleDownDouble(),
                    new AtomicAddDouble());
            };
        }

        [Benchmark]
        public void ReduceAddDouble()
        {
            _reduce();
        }

        #endregion
    }
}