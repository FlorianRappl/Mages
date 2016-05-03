namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Attributes;
    using System;
    using YAMP;

    public class TrivialBenchmarks
    {
        private static readonly String Test1 = "2 + 3";
        private static readonly String Test2 = "2 + 17 + 25 * 4^(2 * 3) / 12";

        [Benchmark]
        public Double AddTwoNumbersWithYamp()
        {
            return YampNumeric(Test1);
        }

        [Benchmark]
        public Double AddTwoNumbersWithMages()
        {
            return MagesNumeric(Test1);
        }

        [Benchmark]
        public Double AddMultiplyDivideAndPowerNumbersWithYamp()
        {
            return YampNumeric(Test2);
        }

        [Benchmark]
        public Double AddMultiplyDivideAndPowerNumbersWithMages()
        {
            return MagesNumeric(Test2);
        }

        private static readonly Parser YampParser = new Parser();
        private static readonly Engine MagesEngine = new Engine();

        private Double YampNumeric(String sourceCode)
        {
            var result = YampParser.Evaluate(sourceCode);
            return (result as ScalarValue).Value;
        }

        private Double MagesNumeric(String sourceCode)
        {
            var result = MagesEngine.Interpret(sourceCode);
            return (Double)result;
        }
    }
}
