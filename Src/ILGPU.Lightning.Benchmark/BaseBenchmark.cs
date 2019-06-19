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

        public IEnumerable<int> ValuesForLength => Enumerable.Range(0, 31).Select(x => 16 << x).TakeWhile(x => x <= 10_000_000);


        public Context Context { get; private set; }
        public Accelerator Accelerator { get; private set; }

        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            Context = new Context();
            Accelerator = Accelerator.Create(Context, AcceleratorId);
        }

        [GlobalCleanup]
        public virtual void GlobalCleanup()
        {
            Accelerator?.Dispose();
            Accelerator = null;
         
            Context?.Dispose();
            Context = null;
        }
    }
}