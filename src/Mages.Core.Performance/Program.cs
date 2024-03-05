namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Running;
    using System;

    static class Program
    {
        static void Main(String[] arguments)
        {
            var config = DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);
            var shouldPause = true;
            BenchmarkRunner.Run<TrivialBenchmarks>(config);
            Pause(shouldPause);
            BenchmarkRunner.Run<CachedBenchmarks>(config);
            Pause(shouldPause);
            BenchmarkRunner.Run<ExtendedBenchmarks>(config);
        }

        private static void Pause(Boolean shouldPause)
        {
            if (shouldPause)
            {
                Console.WriteLine("Execution paused. Press any key to continue ...");
                Console.ReadLine();
            }
        }
    }
}
