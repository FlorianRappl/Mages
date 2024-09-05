namespace Mages.Core.Runtime;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

sealed class JsonSerializer
{
    private readonly HashSet<Object> _seen;

    public JsonSerializer()
    {
        _seen = [];
    }

    public String Serialize(Object value)
    {
        var sb = StringBuilderPool.Pull();
        SerializeTo(value, sb, 0);
        return sb.Stringify();
    }

    private void SerializeTo(IDictionary<String, Object> obj, StringBuilder buffer, Int32 level)
    {
        var index = 0;
        _seen.Add(obj.Unwrap());
        buffer.AppendLine("{");

        foreach (var item in obj)
        {
            var sublevel = level + 1;
            var key = Stringify.AsJson(item.Key);
            buffer.Append(' ', 2 * sublevel).Append(key).Append(": ");
            SerializeTo(item.Value, buffer, sublevel);

            if (index + 1 < obj.Count)
            {
                buffer.Append(',');
            }

            buffer.AppendLine();
        }

        buffer.Append(' ', 2 * level).Append('}');
    }

    private void SerializeTo(Object value, StringBuilder buffer, Int32 level)
    {
        if (value == null)
        {
            buffer.Append("null");
        }
        else if (value is Function)
        {
            buffer.Append(Stringify.AsJson("[Function]"));
        }
        else if (value is IDictionary<String, Object> o)
        {
            if (!_seen.Contains(value.Unwrap()))
            {
                SerializeTo(o, buffer, level);
            }
            else
            {
                buffer.Append(Stringify.AsJson("[Recursion]"));
            }
        }
        else if (value is Double[,] m)
        {
            buffer.Append(Stringify.AsJson(m));
        }
        else if (value is String s)
        {
            buffer.Append(Stringify.AsJson(s));
        }
        else if (value is Double d)
        {
            buffer.Append(Stringify.This(d));
        }
        else if (value is Boolean b)
        {
            buffer.Append(Stringify.This(b));
        }
        else if (value is Complex c)
        {
            SerializeTo(new Dictionary<String, Object>
            {
                { "type", "cmplx" },
                { "real", c.Real },
                { "imag", c.Imaginary },
            }, buffer, level);
        }
        else
        {
            buffer.Append("undefined");
        }
    }
}
