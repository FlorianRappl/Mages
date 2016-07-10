namespace Mages.Plugins
{
    using System;
    using System.Collections.Generic;

    public static class ObjectHelpers
    {
        public static IDictionary<String, Object> CreateObject(params Object[] keyValues)
        {
            var dict = new Dictionary<String, Object>();

            for (var i = 0; i < keyValues.Length; i += 2)
            {
                dict[(String)keyValues[i]] = keyValues[i + 1];
            }

            return dict;
        }
    }
}
