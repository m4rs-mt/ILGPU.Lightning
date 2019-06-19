﻿using System.Collections.Generic;
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


        [ParamsSource(nameof(ValuesForBaseLength))]
        public int BaseLength { get; set; }

        public IEnumerable<int> ValuesForBaseLength => Enumerable.Range(0, 31).Select(x => 16 << x).TakeWhile(x => x <= 10_000_000);


        [GlobalSetup]
        public virtual void GlobalSetup()
        {

        }

        [GlobalCleanup]
        public virtual void GlobalCleanup()
        {

        }
    }
}