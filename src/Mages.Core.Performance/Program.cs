namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Running;
    using System;

    static class Program
    {
        static void Main(String[] arguments)
        {
            var shouldPause = true;
            BenchmarkRunner.Run<TrivialBenchmarks>();
            Pause(shouldPause);
            BenchmarkRunner.Run<CachedBenchmarks>();
            Pause(shouldPause);
            BenchmarkRunner.Run<ExtendedBenchmarks>();
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
