﻿using System.Reflection;
using System.Security.Cryptography;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Running;

namespace ILGPU.Lightning.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GpuHostEnvironmentInfo.SetupToHostEnvironmentInfo();

            // See https://benchmarkdotnet.org/articles/guides/console-args.html
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig()); // TODO Remove DebugInProcessConfig!
        }
    }
}
