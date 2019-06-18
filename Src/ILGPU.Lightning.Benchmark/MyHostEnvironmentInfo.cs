using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Environments;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;

namespace ILGPU.Lightning.Benchmark
{
    public class MyHostEnvironmentInfo : HostEnvironmentInfo
    {
        public static MyHostEnvironmentInfo Instance
        {
            get => _instance ?? (_instance = new MyHostEnvironmentInfo());
            set => _instance = value;
        }

        private static MyHostEnvironmentInfo _instance;
        
        public IReadOnlyDictionary<AcceleratorId, string> GpuDeviceNames { get; }

        public MyHostEnvironmentInfo()
        {
            var deviceNames = new Dictionary<AcceleratorId, string>();

            using (var context = new Context())
            {
                foreach (var acceleratorId in CudaAccelerator.CudaAccelerators)
                {
                    using (var accelerator = Accelerator.Create(context, acceleratorId))
                        deviceNames[acceleratorId] = accelerator.ToString();
                }
            }

            GpuDeviceNames = deviceNames;
        }

        public void SetupToHostEnvironmentInfo()
        {
            var field = typeof(HostEnvironmentInfo).GetField("current", BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, this);
        }

        public override IEnumerable<string> ToFormattedString()
        {
            return base.ToFormattedString().Concat(
                GpuDeviceNames.Select(x => $"{x.Value} ({x.Key})"));
        }
    }
}