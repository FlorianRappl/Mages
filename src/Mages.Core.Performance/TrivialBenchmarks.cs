namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Attributes;
    using System;
    using YAMP;

    public class TrivialBenchmarks
    {
        private static readonly String Test1 = "2 + 3";
        private static readonly String Test2 = "2 + 17 + 25 * 4^(2 * 3) / 12";
        private static readonly String Test3 = "a = 3; b = 6; a * b";

        private static readonly Parser YampParser = new Parser();
        private static readonly Engine MagesEngine = new Engine();

        [Benchmark]
        public Double Yamp_AddTwoNumbers()
        {
            return YampNumeric(Test1);
        }

        [Benchmark]
        public Double Mages_AddTwoNumbers()
        {
            return MagesNumeric(Test1);
        }

        [Benchmark]
        public Double Yamp_AddMultiplyDivideAndPowerNumbers()
        {
            return YampNumeric(Test2);
        }

        [Benchmark]
        public Double Mages_AddMultiplyDivideAndPowerNumbers()
        {
            return MagesNumeric(Test2);
        }

        [Benchmark]
        public Double Yamp_MultiplyTwoVariables()
        {
            return YampNumeric(Test3);
        }

        [Benchmark]
        public Double Mages_MultiplyTwoVariables()
        {
            return MagesNumeric(Test3);
        }

        private Double YampNumeric(String sourceCode)
        {
            var result = YampParser.Evaluate(sourceCode);
            return (result as ScalarValue).Value;
        }

        private Double MagesNumeric(String sourceCode)
        {
            var result = MagesEngine.Interpret(sourceCode);
            return ((Mages.Core.Types.Number)result).Value;
        }
    }
}
