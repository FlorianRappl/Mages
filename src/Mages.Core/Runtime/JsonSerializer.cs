namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;

    sealed class JsonSerializer
    {
        private readonly HashSet<Object> _seen;

        public JsonSerializer()
        {
            _seen = new HashSet<Object>();
        }

        public String Serialize(Object value)
        {
            if (value == null)
            {
                return "null";
            }
            else if (value is Function)
            {
                return Stringify.AsJson("[Function]");
            }
            else if (value is IDictionary<String, Object>)
            {
                if (!_seen.Contains(value))
                {
                    _seen.Add(value);
                    return AsJson((IDictionary<String, Object>)value);
                }

                return Stringify.AsJson("[Recursion]");
            }
            else if (value is Double[,])
            {
                return Stringify.AsJson((Double[,])value);
            }
            else if (value is String)
            {
                return Stringify.AsJson((String)value);
            }
            else if (value is Double)
            {
                return Stringify.This((Double)value);
            }
            else if (value is Boolean)
            {
                return Stringify.This((Boolean)value);
            }

            return "undefined";
        }

        /// <summary>
        /// Converts the given dictionary object to a JSON string.
        /// </summary>
        /// <param name="obj">The object to represent.</param>
        /// <returns>The JSON representation.</returns>
        public String AsJson(IDictionary<String, Object> obj)
        {
            var sb = StringBuilderPool.Pull();
            sb.AppendLine("{");

            foreach (var item in obj)
            {
                var key = Stringify.AsJson(item.Key);
                var value = Serialize(item.Value);

                if (value.Contains(Environment.NewLine))
                {
                    var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    value = String.Join(Environment.NewLine + "  ", lines);
                }

                sb.Append("  ").Append(key).Append(": ").AppendLine(value);
            }

            sb.Append('}');
            return sb.Stringify();
        }
    }
}
