namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Running;
    using System;

    static class Program
    {
        static void Main(String[] arguments)
        {
            BenchmarkRunner.Run<TrivialBenchmarks>();
        }
    }
}
