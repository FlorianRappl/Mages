namespace Mages.Repl.Functions
{
    using Core.Runtime;
    using Mages.Core.Runtime.Converters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    sealed class HelpFunctions
    {
        private readonly IDictionary<String, Object> _globals;
        private readonly IDictionary<String, Object> _scope;

        public HelpFunctions(IDictionary<String, Object> globals, IDictionary<String, Object> scope)
        {
            _globals = globals;
            _scope = scope;
        }

        public String GetAllTopics()
        {
            var sb = new StringBuilder();
            sb.Append("Global Scope: ");
            Print(sb, _scope);
            sb.AppendLine();
            sb.Append("Available API: ");
            Print(sb, _globals);
            return sb.ToString();
        }

        public String GetTopic(String topic)
        {
            var value = default(Object);

            if (!_globals.TryGetValue(topic, out value))
            {
                var closest = ClosestEntry(topic);
                return String.Format("'{0}' was not found in the API layer. Did you mean '{1}'?", topic, closest);
            }

            return Info(topic, value);
        }

        private static void Print(StringBuilder sb, IDictionary<String, Object> items)
        {
            foreach (var item in items)
            {
                var name = item.Key;
                var type = item.Value.ToType();
                sb.AppendLine();
                sb.Append("- [");
                sb.Append(type);
                sb.Append("] ");
                sb.Append(name);
            }
        }

        private String Info(String topic, Object value)
        {
            var sb = new StringBuilder();
            var type = value.ToType();
            var typeName = $"{type["name"]}";
            sb.AppendFormat("Type of '{0}': ", topic);
            sb.AppendLine(typeName);
            sb.Append("Number value: ");
            sb.Append(Stringify.This(value.ToNumber()));
            sb.AppendLine();
            sb.Append("Boolean value: ");
            sb.Append(Stringify.This(value.ToBoolean()));
            sb.AppendLine();
            sb.Append("String value: ");
            sb.Append(Stringify.This(value));

            switch (typeName)
            {
                case "Number":
                case "Complex":
                case "String":
                case "Boolean":
                case "Function":
                case "Undefined":
                    break;
                case "CMatrix":
                    sb.AppendLine();
                    sb.Append("Columns: ");
                    sb.Append(((Complex[,])value).GetLength(1));
                    sb.AppendLine();
                    sb.Append("Rows: ");
                    sb.Append(((Complex[,])value).GetLength(0));
                    break;
                case "Matrix":
                    sb.AppendLine();
                    sb.Append("Columns: ");
                    sb.Append(((Double[,])value).GetLength(1));
                    sb.AppendLine();
                    sb.Append("Rows: ");
                    sb.Append(((Double[,])value).GetLength(0));
                    break;
                case "Object":
                    sb.AppendLine();
                    sb.Append("Keys: ");
                    sb.Append(String.Join(", ", ((IDictionary<String, Object>)value).Select(m => m.Key)));
                    break;
            }

            return sb.ToString();
        }

        private String ClosestEntry(String entry)
        {
            var topics = _globals.Keys;
            var substitute = topics.FirstOrDefault(m => m.Equals(entry, StringComparison.OrdinalIgnoreCase));

            if (substitute == null)
            {
                var min = Int32.MaxValue;

                foreach (var topic in topics)
                {
                    var sum = Distance(entry, topic, 10);

                    if (sum < min)
                    {
                        min = sum;
                        substitute = topic;
                    }
                }
            }

            return substitute;
        }

        private static Int32 Distance(String s1, String s2, Int32 maxOffset)
        {
            if (String.IsNullOrEmpty(s1))
            {
                return !String.IsNullOrEmpty(s2) ? s2.Length : 0;
            }
            else if (!String.IsNullOrEmpty(s2))
            {
                var c = 0;
                var offset1 = 0;
                var offset2 = 0;
                var lcs = 0;

                while ((c + offset1 < s1.Length) && (c + offset2 < s2.Length))
                {
                    if (s1[c + offset1] != s2[c + offset2])
                    {
                        offset1 = 0;
                        offset2 = 0;

                        for (var i = 0; i < maxOffset; i++)
                        {
                            var ci = c + i;

                            if (ci < s1.Length && AreSameIgnoreCase(s1[ci], s2[c]))
                            {
                                offset1 = i;
                                break;
                            }

                            if (ci < s2.Length && AreSameIgnoreCase(s1[c], s2[ci]))
                            {
                                offset2 = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        lcs++;
                    }

                    c++;
                }

                return (s1.Length + s2.Length) / 2 - lcs;
            }

            return s1.Length;
        }

        private static Boolean AreSameIgnoreCase(Char v1, Char v2)
        {
            return Char.ToLowerInvariant(v1) == Char.ToLowerInvariant(v2);
        }
    }
}
