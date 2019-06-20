using System.Reflection;
using BenchmarkDotNet.Characteristics;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;

namespace ILGPU.Lightning.Benchmark
{
    public class ClockInfrastructureResolver : Resolver
    {
        public static readonly IResolver Instance = new ClockInfrastructureResolver();

        public static IClock Clock { get; set; }

        public static void AddToInfrastructureResolver()
        {
            var instanceField = typeof(InfrastructureResolver).GetField("Instance", BindingFlags.Static | BindingFlags.Public);
            var resolver = (IResolver)instanceField.GetValue(null);
            instanceField.SetValue(null, new CompositeResolver(Instance, resolver));
        }

        private ClockInfrastructureResolver()
        {
            Register(InfrastructureMode.ClockCharacteristic, () => Clock ?? Chronometer.BestClock);
        }
    }
}