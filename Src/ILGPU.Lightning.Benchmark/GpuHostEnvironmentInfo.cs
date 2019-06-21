using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Environments;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;

namespace ILGPU.Lightning.Benchmark
{
    public class GpuHostEnvironmentInfo : HostEnvironmentInfo
    {
        public static GpuHostEnvironmentInfo Instance
        {
            get => _instance ?? (_instance = new GpuHostEnvironmentInfo());
            set => _instance = value;
        }
        private static GpuHostEnvironmentInfo _instance;

        public static void SetupToHostEnvironmentInfo(HostEnvironmentInfo instance = null)
        {
            var field = typeof(HostEnvironmentInfo).GetField("current", BindingFlags.NonPublic | BindingFlags.Static);
            field.SetValue(null, instance ?? Instance);
        }


        public IReadOnlyDictionary<AcceleratorId, string> GpuDeviceNames { get; }

        public GpuHostEnvironmentInfo()
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

        public override IEnumerable<string> ToFormattedString()
        {
            return base.ToFormattedString().Concat(
                GpuDeviceNames.Select(x =>
                {
                    var str = x.Value;
                    if (1 < GpuDeviceNames.Count)
                        str = $"[{x.Key.AcceleratorType} #{x.Key.DeviceId}] {str}";
                    return str;
                }));
        }
    }
}