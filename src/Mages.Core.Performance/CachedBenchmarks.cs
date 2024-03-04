namespace Mages.Core.Performance
{
    using BenchmarkDotNet.Attributes;
    using System;
    using System.Collections.Generic;

    public class CachedBenchmarks
    {
        private static readonly String CallStandardFunctions = "sin(pi / 4) * cos(pi * 0.25) + exp(2) * log(3)";
        private static readonly Engine MagesEngine = new Engine();
        private static readonly Dictionary<String, Func<Object>> FunctionCache = [];

        [Benchmark]
        public Double Mages_Uncached_CallStandardFunctions()
        {
            return MagesUncachedNumeric(CallStandardFunctions);
        }

        [Benchmark]
        public Double Mages_Cached_CallStandardFunctions()
        {
            return MagesCachedNumeric(CallStandardFunctions);
        }

        private Double MagesCachedNumeric(String sourceCode)
        {
            var func = default(Func<Object>);
            
            if (!FunctionCache.TryGetValue(sourceCode, out func))
            {
                func = MagesEngine.Compile(sourceCode);
                FunctionCache[sourceCode] = func;
            }
            
            var result = func.Invoke();
            return (Double)result;
        }

        private Double MagesUncachedNumeric(String sourceCode)
        {
            var result = MagesEngine.Interpret(sourceCode);
            return (Double)result;
        }
    }
}
