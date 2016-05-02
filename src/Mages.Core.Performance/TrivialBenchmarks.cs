namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Attributes;
    using System;

    public class TrivialBenchmarks
    {
        [Benchmark]
        public Double AddTwoNumbersWithYamp()
        {
            return YampNumeric("2 + 3");
        }

        [Benchmark]
        public Double AddTwoNumbersWithMages()
        {
            return MagesNumeric("2 + 3");
        }

        private Double YampNumeric(String sourceCode)
        {
            //TODO
            return 10.0;
        }

        private Double MagesNumeric(String sourceCode)
        {
            //TODO
            return 10.0;
        }
    }
}
