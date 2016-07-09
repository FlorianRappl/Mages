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
    }
}
