namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Running;

    public static class Benchmarks
    {
        public static void Run()
        {
            BenchmarkRunner.Run<TrivialBenchmarks>();
        }
    }
}
