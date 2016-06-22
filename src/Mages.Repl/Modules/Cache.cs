namespace Mages.Repl.Modules
{
    using Mages.Core;
    using System;
    using System.Runtime.CompilerServices;

    static class Cache
    {
        private static readonly ConditionalWeakTable<Engine, Object> _exports = new ConditionalWeakTable<Engine, Object>();

        public static void Assign(Engine engine, Object value)
        {
            _exports.Remove(engine);
            _exports.Add(engine, value);
        }

        public static Object Retrieve(Engine engine)
        {
            return _exports.GetOrCreateValue(engine);
        }
    }
}
