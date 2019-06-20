using System.Diagnostics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace ILGPU.Lightning.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GpuHostEnvironmentInfo.SetupToHostEnvironmentInfo();
            ClockInfrastructureResolver.AddToInfrastructureResolver();
            

            var config = Debugger.IsAttached ? new DebugInProcessConfig() : null;

            // See https://benchmarkdotnet.org/articles/guides/console-args.html
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
        }
    }
}
