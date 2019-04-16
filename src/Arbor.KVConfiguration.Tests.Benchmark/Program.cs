using System;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Arbor.KVConfiguration.Tests.Benchmark
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Summary summary = BenchmarkRunner.Run<BenchmarkGetItemByKey>();

            Console.WriteLine(summary);

            Summary summary2 = BenchmarkRunner.Run<BenchmarkBindConfigurationKey>();

            Console.WriteLine(summary2);

            Console.ReadLine();
        }
    }
}
