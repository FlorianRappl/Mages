namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Attributes;
    using System;
    using YAMP;

    public class ExtendedBenchmarks
    {
        private static readonly String CreateAndUseFunction = "f = (x, y) => sin(x) * cos(y); g = (x, y, z) => f(x, y) * f(y, z); g(2, 1, 0)";
        private static readonly String MultiplySmallMatrices = "rand(2, 3) * rand(3, 2)";
        private static readonly String MultiplyMediumMatrices = "rand(40, 60) * rand(60, 40)";
        private static readonly String MultiplyLargeMatrices = "rand(100, 200) * rand(200, 100)";
        private static readonly String ObjectAccessMages = "obj = new { a: 2, b: 3, c: 4}; obj.a * obj.b - obj.c";
        private static readonly String ObjectAccessYamp = "obj = object(); obj.a = 2; obj.b = 3; obj.c = 4; obj.a * obj.b - obj.c";

        private static readonly Parser YampParser = new Parser();
        private static readonly Engine MagesEngine = new Engine();

        [Benchmark]
        public Value Yamp_CreateAndUseFunction()
        {
            return YampNumeric(CreateAndUseFunction);
        }

        [Benchmark]
        public Object Mages_CreateAndUseFunction()
        {
            return MagesNumeric(CreateAndUseFunction);
        }

        [Benchmark]
        public Value Yamp_MultiplySmallMatrices()
        {
            return YampNumeric(MultiplySmallMatrices);
        }

        [Benchmark]
        public Object Mages_MultiplySmallMatrices()
        {
            return MagesNumeric(MultiplySmallMatrices);
        }

        [Benchmark]
        public Value Yamp_MultiplyMediumMatrices()
        {
            return YampNumeric(MultiplyMediumMatrices);
        }

        [Benchmark]
        public Object Mages_MultiplyMediumMatrices()
        {
            return MagesNumeric(MultiplyMediumMatrices);
        }

        [Benchmark]
        public Value Yamp_MultiplyLargeMatrices()
        {
            return YampNumeric(MultiplyLargeMatrices);
        }

        [Benchmark]
        public Object Mages_MultiplyLargeMatrices()
        {
            return MagesNumeric(MultiplyLargeMatrices);
        }

        [Benchmark]
        public Value Yamp_ObjectAccess()
        {
            return YampNumeric(ObjectAccessYamp);
        }

        [Benchmark]
        public Object Mages_ObjectAccess()
        {
            return MagesNumeric(ObjectAccessMages);
        }

        private Value YampNumeric(String sourceCode)
        {
            return YampParser.Evaluate(sourceCode);
        }

        private Object MagesNumeric(String sourceCode)
        {
            return MagesEngine.Interpret(sourceCode);
        }
    }
}
