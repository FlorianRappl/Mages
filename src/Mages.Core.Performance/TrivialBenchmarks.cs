namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Attributes;
    using System;

    public class TrivialBenchmarks
    {
        [Benchmark]
        public Double Foo()
        {
            return Math.Exp(1.2);
        }
    }
}
