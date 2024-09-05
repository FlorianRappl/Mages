namespace Mages.Core.Runtime;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;

/// <summary>
/// Helpers to stringify objects used by MAGES.
/// </summary>
public static class Stringify
{
    /// <summary>
    /// Contains the stringify function.
    /// </summary>
    public static readonly Function Default = new(args => This(args.Length > 0 ? args[0] : null));

    /// <summary>
    /// Contains the JSON function.
    /// </summary>
    public static readonly Function Json = new(args => AsJson(args.Length > 0 ? args[0] : null));

    /// <summary>
    /// Contains the HTML function.
    /// </summary>
    public static readonly Function Html = new(args => AsHtml(args.Length > 0 ? args[0] : null));

    /// <summary>
    /// Converts the number to a string.
    /// </summary>
    public static String This(Complex value) => $"cmplx{value.ToString(CultureInfo.InvariantCulture)}";

    /// <summary>
    /// Converts the number to a string.
    /// </summary>
    public static String This(Double value) => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the boolean to a string.
    /// </summary>
    public static String This(Boolean value) => value ? Keywords.True : Keywords.False;

    /// <summary>
    /// Converts the string for output.
    /// </summary>
    public static String This(String value) => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts the matrix to a string.
    /// </summary>
    public static String This<T>(T[,] value)
    {
        var sb = StringBuilderPool.Pull();
        var rows = value.GetLength(0);
        var cols = value.GetLength(1);
        sb.Append('[');

        for (var i = 0; i < rows; i++)
        {
            if (i > 0)
            {
                sb.Append(';');
            }

            for (var j = 0; j < cols; j++)
            {
                if (j > 0)
                {
                    sb.Append(',');
                }

                sb.Append(This(value[i, j]));
            }
        }

        sb.Append(']');
        return sb.Stringify();
    }

    /// <summary>
    /// Converts the object to a string.
    /// </summary>
    public static String This(IDictionary<String, Object> value) => "[Object]";

    /// <summary>
    /// Converts the function to a string.
    /// </summary>
    public static String This(Function value) => "[Function]";

    /// <summary>
    /// Outputs the string for an undefined (null?) value.
    /// </summary>
    /// <returns></returns>
    public static String Undefined() => System.String.Empty;

    /// <summary>
    /// Converts the undetermined value to a string.
    /// </summary>
    public static String This(Object value)=> value switch
    {
        null => Undefined(),
        Function f => This(f),
        IDictionary<String, Object> o => This(o),
        Double[,] m => This(m),
        String s => This(s),
        Double d => This(d),
        Boolean b => This(b),
        Complex c => This(c),
        Complex[,] m => This(m),
        _ => "(unknown)"
    };

    /// <summary>
    /// Converts the given MAGES object to a JSON string.
    /// </summary>
    /// <param name="value">The object to represent.</param>
    /// <returns>The JSON representation.</returns>
    public static String AsJson(Object value)
    {
        var serializer = new JsonSerializer();
        return serializer.Serialize(value);
    }

    /// <summary>
    /// Converts the given string object to a JSON string.
    /// </summary>
    /// <param name="str">The string to represent.</param>
    /// <returns>The JSON representation.</returns>
    public static String AsJson(String str)
    {
        var escaped = str.Replace("\"", "\\\"");
        return String.Concat("\"", escaped, "\"");
    }

    /// <summary>
    /// Converts the given MAGES object to an HTML string.
    /// </summary>
    /// <param name="value">The object to represent.</param>
    /// <returns>The HTML representation.</returns>
    public static String AsHtml(Object value)
    {
        if (value is IDictionary<String, Object> jsxElement)
        {
            jsxElement.TryGetValue("type", out var type);
            jsxElement.TryGetValue("props", out var props);

            if (type is not null)
            {
                if (type is Function f)
                {
                    return AsHtml(f([props]));
                }
                else if (type is String name)
                {
                    var (attrs, content) = InspectProps(props);
                    // Fragment case -> type is ""
                    return String.IsNullOrEmpty(name) ? content : $"<{name}{attrs}>{content}</{name}>";
                }
            }
            else
            {
                var content = new List<String>();

                foreach (var child in jsxElement)
                {
                    content.Add(AsHtml(child.Value));
                }

                return String.Join("", content);
            }
        }
        else if (value is String str)
        {
            return str;
        }
        else if (value is Double d)
        {
            return d.ToString();
        }

        return "";
    }

    private static (string, string) InspectProps(Object args)
    {
        var content = "";

        if (args is IDictionary<String, Object> props)
        {
            var attrs = new List<String> { "" };

            foreach (var (key, value) in props)
            {
                if (key == "children")
                {
                    content = AsHtml(value);
                }
                else if (value is Boolean b)
                {
                    var k = key.ToLowerInvariant();
                    var val = b ? k : String.Empty;
                    attrs.Add($"{k}=\"{val}\"");
                }
                else
                {
                    var k = key.ToLowerInvariant();
                    attrs.Add($"{k}=\"{value}\"");
                }
            }

            return (String.Join(" ", attrs), content);
        }

        return ("", content);
    }

    /// <summary>
    /// Converts the given matrix to a JSON string.
    /// </summary>
    /// <param name="matrix">The matrix to represent.</param>
    /// <returns>The JSON representation.</returns>
    public static String AsJson(Double[,] matrix)
    {
        var sb = StringBuilderPool.Pull();
        var rows = matrix.GetRows();
        var cols = matrix.GetColumns();
        sb.Append('[');

        for (var i = 0; i < rows; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append('[');

            for (var j = 0; j < cols; j++)
            {
                if (j > 0) sb.Append(", ");
                sb.Append(This(matrix[i, j]));
            }

            sb.Append(']');
        }

        sb.Append(']');
        return sb.Stringify();
    }
}
