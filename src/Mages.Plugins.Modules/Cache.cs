namespace Mages.Plugins.Modules
{
    using Mages.Core;
    using System;
    using System.Collections.Generic;

    static class Cache
    {
        private static readonly Dictionary<Engine, Object> _exports = new Dictionary<Engine, Object>();

        public static void Add(Engine engine)
        {
            _exports[engine] = null;
            engine.SetCache();
        }

        public static void Assign(Engine engine, Object value)
        {
            _exports[engine] = value;
        }

        public static Object Retrieve(Engine engine)
        {
            var value = default(Object);

            if (engine != null)
            {
                _exports.TryGetValue(engine, out value);
            }

            return value;
        }

        public static Engine Find(String modulePath)
        {
            foreach (var engine in _exports.Keys)
            {
                var path = engine.GetPath();

                if (modulePath.Equals(path, StringComparison.Ordinal))
                {
                    return engine;
                }
            }

            return null;
        }

        public static IDictionary<String, Object> GetPaths()
        {
            var result = new Dictionary<String, Object>();

            foreach (var engine in _exports.Keys)
            {
                result[result.Count.ToString()] = engine.GetPath();
            }

            return result;
        }

        public static Double Reset()
        {
            var count = _exports.Count;
            _exports.Clear();
            return count;
        }
    }
}
