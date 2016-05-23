namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Attributes;
    using System;
    using YAMP;

    public class TrivialBenchmarks
    {
        private static readonly String AddTwoNumbers = "2 + 3";
        private static readonly String AddMultiplyDivideAndPowerNumbers = "2 + 17 + 25 * 4^(2 * 3) / 12";
        private static readonly String MultiplyTwoVariables = "a = 3; b = 6; a * b";
        private static readonly String CallStandardFunctions = "sin(pi / 4) * cos(pi * 0.25) + exp(2) * log(3)";
        private static readonly String Transpose4x5Matrix = "M = [1,2,3,4,5;6,7,8,9,10;11,12,13,14,15;16,17+2,18/3,19*4,20-5]'; 0";

        private static readonly Parser YampParser = new Parser();
        private static readonly Engine MagesEngine = new Engine();

        [Benchmark]
        public Double Yamp_AddTwoNumbers()
        {
            return YampNumeric(AddTwoNumbers);
        }

        [Benchmark]
        public Double Mages_AddTwoNumbers()
        {
            return MagesNumeric(AddTwoNumbers);
        }

        [Benchmark]
        public Double Yamp_AddMultiplyDivideAndPowerNumbers()
        {
            return YampNumeric(AddMultiplyDivideAndPowerNumbers);
        }

        [Benchmark]
        public Double Mages_AddMultiplyDivideAndPowerNumbers()
        {
            return MagesNumeric(AddMultiplyDivideAndPowerNumbers);
        }

        [Benchmark]
        public Double Yamp_MultiplyTwoVariables()
        {
            return YampNumeric(MultiplyTwoVariables);
        }

        [Benchmark]
        public Double Mages_MultiplyTwoVariables()
        {
            return MagesNumeric(MultiplyTwoVariables);
        }

        [Benchmark]
        public Double Yamp_CallStandardFunctions()
        {
            return YampNumeric(CallStandardFunctions);
        }

        [Benchmark]
        public Double Mages_CallStandardFunctions()
        {
            return MagesNumeric(CallStandardFunctions);
        }

        [Benchmark]
        public Double Yamp_Transpose4x5Matrix()
        {
            return YampNumeric(Transpose4x5Matrix);
        }

        [Benchmark]
        public Double Mages_Transpose4x5Matrix()
        {
            return MagesNumeric(Transpose4x5Matrix);
        }

        private Double YampNumeric(String sourceCode)
        {
            var result = YampParser.Evaluate(sourceCode);
            return ((ScalarValue)result).Value;
        }

        private Double MagesNumeric(String sourceCode)
        {
            var result = MagesEngine.Interpret(sourceCode);
            return (Double)result;
        }
    }
}
