using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using ILGPU.Runtime;

namespace ILGPU.Lightning.Benchmark
{
    public abstract class BaseBenchmark
    {
        [ParamsSource(nameof(ValuesForAcceleratorId))]
        public AcceleratorId AcceleratorId { get; set; }

        public IEnumerable<AcceleratorId> ValuesForAcceleratorId => GpuHostEnvironmentInfo.Instance.GpuDeviceNames.Keys;


        [ParamsSource(nameof(ValuesForLength))]
        public int Length { get; set; }

        public IEnumerable<int> ValuesForLength => FilterValuesForLength(Enumerable.Range(0, 31).Select(x => 1 << x).TakeWhile(x => x <= 100_000_000));

        protected virtual IEnumerable<int> FilterValuesForLength(IEnumerable<int> source) => source;


        public Context Context { get; private set; }
        public Accelerator Accelerator { get; private set; }

        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            Context = new Context();
            Accelerator = Accelerator.Create(Context, AcceleratorId);

            ClockInfrastructureResolver.Clock = new GpuClock(this);
        }

        [GlobalCleanup]
        public virtual void GlobalCleanup()
        {
            (ClockInfrastructureResolver.Clock as IDisposable)?.Dispose();
            ClockInfrastructureResolver.Clock = null;

            Accelerator?.Dispose();
            Accelerator = null;
         
            Context?.Dispose();
            Context = null;
        }
    }
}