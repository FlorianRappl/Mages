using Mages.Core;
using Mages.Core.Ast;
using Mages.Core.Ast.Expressions;
using Mages.Core.Ast.Walkers;
using Mages.Core.Runtime.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

var server = new LanguageServer(Console.OpenStandardInput(), Console.OpenStandardOutput());
await server.Run();

sealed class LanguageServer(Stream input, Stream output)
{
    private readonly Dictionary<String, String> _documents = new(StringComparer.Ordinal);
    private readonly Engine _engine = new();
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public async Task Run()
    {
        while (await ReadMessage() is { } message)
        {
            var method = message["method"]?.GetValue<String>();
            var id = message["id"];

            switch (method)
            {
                case "initialize":
                    await Reply(id, new JsonObject
                    {
                        ["capabilities"] = new JsonObject
                        {
                            ["textDocumentSync"] = 1,
                            ["completionProvider"] = new JsonObject { ["triggerCharacters"] = new JsonArray(".", "(", ",") },
                            ["hoverProvider"] = true,
                            ["documentSymbolProvider"] = true,
                            ["signatureHelpProvider"] = new JsonObject { ["triggerCharacters"] = new JsonArray("(", ",") },
                        },
                    });
                    break;
                case "textDocument/didOpen":
                case "textDocument/didChange":
                    UpdateDocument(message);
                    await PublishDiagnostics(message["params"]?["textDocument"]?["uri"]?.GetValue<String>());
                    break;
                case "textDocument/completion":
                    await Reply(id, Completion(message));
                    break;
                case "textDocument/hover":
                    await Reply(id, Hover(message));
                    break;
                case "textDocument/documentSymbol":
                    await Reply(id, Symbols(message));
                    break;
                case "textDocument/signatureHelp":
                    await Reply(id, SignatureHelp(message));
                    break;
                case "shutdown":
                    await Reply(id, null);
                    return;
            }
        }
    }

    private void UpdateDocument(JsonObject message)
    {
        var textDocument = message["params"]?["textDocument"];
        var uri = textDocument?["uri"]?.GetValue<String>();

        if (uri is null)
            return;

        if (message["method"]?.GetValue<String>() == "textDocument/didOpen")
        {
            _documents[uri] = textDocument?["text"]?.GetValue<String>() ?? String.Empty;
        }
        else if (message["params"]?["contentChanges"] is JsonArray changes && changes.LastOrDefault() is JsonObject change)
        {
            _documents[uri] = change["text"]?.GetValue<String>() ?? String.Empty;
        }
    }

    private JsonArray Completion(JsonObject message)
    {
        var (uri, position) = GetDocumentPosition(message);
        var result = new JsonArray();

        if (uri is null || !_documents.TryGetValue(uri, out var source))
            return result;

        try
        {
            var index = OffsetAt(source, position);
            foreach (var name in _engine.GetCompletionAt(source, index).Distinct(StringComparer.Ordinal))
            {
                var label = name.Replace("|", String.Empty, StringComparison.Ordinal);
                _engine.GetGlobalItems().TryGetValue(label, out var value);
                var item = new JsonObject
                {
                    ["label"] = label,
                    ["insertText"] = label,
                    ["kind"] = CompletionKind(value),
                };
                result.Add(item);
            }
        }
        catch (ParseException)
        {
        }

        return result;
    }

    private JsonNode Hover(JsonObject message)
    {
        var (uri, position) = GetDocumentPosition(message);

        if (uri is null || !_documents.TryGetValue(uri, out var source))
            return null;

        var index = OffsetAt(source, position);
        var (name, start, end) = WordAt(source, index);

        if (name is null || !_engine.GetGlobalItems().TryGetValue(name, out var value))
            return null;

        var type = value.ToType()["name"]?.ToString() ?? "Undefined";
        var detail = type == "Function" && value is Function function
            ? $"{name}({String.Join(", ", function.GetParameterNames())})"
            : $"{name}: {type}";

        return new JsonObject
        {
            ["contents"] = new JsonObject { ["kind"] = "markdown", ["value"] = $"```mages\n{detail}\n```" },
            ["range"] = Range(start, end),
        };
    }

    private JsonArray Symbols(JsonObject message)
    {
        var (uri, _) = GetDocumentPosition(message);
        var result = new JsonArray();

        if (uri is null || !_documents.TryGetValue(uri, out var source))
            return result;

        try
        {
            var statements = _engine.Parser.ParseStatements(source);
            var variables = new SymbolTreeWalker();
            foreach (var statement in statements)
                statement.Accept(variables);

            foreach (var symbol in variables.Symbols.GroupBy(m => m.Name).Select(m => m.First()))
            {
                result.Add(new JsonObject
                {
                    ["name"] = symbol.Name,
                    ["kind"] = 13,
                    ["range"] = Range(symbol.Start, symbol.End),
                    ["selectionRange"] = Range(symbol.Start, symbol.End),
                });
            }
        }
        catch (ParseException)
        {
        }

        return result;
    }

    private JsonNode SignatureHelp(JsonObject message)
    {
        var (uri, position) = GetDocumentPosition(message);

        if (uri is null || !_documents.TryGetValue(uri, out var source))
            return null;

        var index = OffsetAt(source, position);
        var open = source.LastIndexOf('(', Math.Max(0, index - 1));
        var end = open < 0 ? -1 : open;

        while (end > 0 && Char.IsWhiteSpace(source[end - 1]))
            end--;

        var start = end;
        while (start > 0 && (Char.IsLetterOrDigit(source[start - 1]) || source[start - 1] == '_'))
            start--;

        var name = start < end ? source[start..end] : null;
        if (name is null || !_engine.GetGlobalItems().TryGetValue(name, out var value) || value is not Function function)
            return null;

        var parameters = function.GetParameterNames();
        return new JsonObject
        {
            ["signatures"] = new JsonArray(new JsonObject
            {
                ["label"] = $"{name}({String.Join(", ", parameters)})",
                ["parameters"] = new JsonArray(parameters.Select(parameter => (JsonNode)new JsonObject { ["label"] = parameter }).ToArray()),
            }),
            ["activeSignature"] = 0,
            ["activeParameter"] = 0,
        };
    }

    private async Task PublishDiagnostics(String uri)
    {
        if (uri is null || !_documents.TryGetValue(uri, out var source))
            return;

        var diagnostics = new JsonArray();
        try
        {
            _engine.Parser.ParseStatements(source).MakeRunnable();
        }
        catch (ParseException error)
        {
            diagnostics.Add(new JsonObject
            {
                ["severity"] = 1,
                ["source"] = "mages",
                ["message"] = error.Error.Code.ToString(),
                ["range"] = Range(error.Error.Start, error.Error.End),
            });
        }

        await Notify("textDocument/publishDiagnostics", new JsonObject { ["uri"] = uri, ["diagnostics"] = diagnostics });
    }

    private async Task<JsonObject> ReadMessage()
    {
        var headers = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
        String line;
        do
        {
            line = await ReadLine();
            if (line is null)
                return null;
            if (line.Length > 0)
            {
                var separator = line.IndexOf(':');
                if (separator > 0)
                    headers[line[..separator]] = line[(separator + 1)..].Trim();
            }
        }
        while (line.Length > 0);

        var length = Int32.Parse(headers["Content-Length"]);
        var buffer = new Byte[length];
        var read = 0;
        while (read < length)
            read += await input.ReadAsync(buffer.AsMemory(read, length - read));
        return JsonNode.Parse(Encoding.UTF8.GetString(buffer)).AsObject();
    }

    private async Task<String> ReadLine()
    {
        var bytes = new List<Byte>();
        while (true)
        {
            var value = input.ReadByte();
            if (value < 0)
                return bytes.Count == 0 ? null : Encoding.ASCII.GetString([.. bytes]);
            if (value == '\n')
                return Encoding.ASCII.GetString([.. bytes]).TrimEnd('\r');
            bytes.Add((Byte)value);
        }
    }

    private Task Reply(JsonNode id, JsonNode result) => Send(new JsonObject { ["jsonrpc"] = "2.0", ["id"] = id?.DeepClone(), ["result"] = result });

    private Task Notify(String method, JsonNode parameters) => Send(new JsonObject { ["jsonrpc"] = "2.0", ["method"] = method, ["params"] = parameters });

    private async Task Send(JsonObject message)
    {
        var payload = Encoding.UTF8.GetBytes(message.ToJsonString(_json));
        var header = Encoding.ASCII.GetBytes($"Content-Length: {payload.Length}\r\n\r\n");
        await output.WriteAsync(header);
        await output.WriteAsync(payload);
        await output.FlushAsync();
    }

    private static (String uri, Position position) GetDocumentPosition(JsonObject message)
    {
        var parameters = message["params"];
        return (parameters?["textDocument"]?["uri"]?.GetValue<String>(), Position.From(parameters?["position"]?.AsObject()));
    }

    private static (String name, TextPosition start, TextPosition end) WordAt(String source, Int32 index)
    {
        var start = index;
        while (start > 0 && (Char.IsLetterOrDigit(source[start - 1]) || source[start - 1] == '_'))
            start--;
        var end = index;
        while (end < source.Length && (Char.IsLetterOrDigit(source[end]) || source[end] == '_'))
            end++;
        return (start == end ? null : source[start..end], ToTextPosition(source, start), ToTextPosition(source, end));
    }

    private static TextPosition ToTextPosition(String source, Int32 index)
    {
        var row = 0;
        var lineStart = 0;

        for (var i = 0; i < index && i < source.Length; i++)
        {
            if (source[i] == '\n')
            {
                row++;
                lineStart = i + 1;
            }
        }

        return new TextPosition(row, index - lineStart, index);
    }

    private static JsonObject Range(TextPosition start, TextPosition end) => Range(new Position(start.Row, start.Column), new Position(end.Row, end.Column));

    private static JsonObject Range(Position start, Position end) => new()
    {
        ["start"] = new JsonObject { ["line"] = start.Line, ["character"] = start.Character },
        ["end"] = new JsonObject { ["line"] = end.Line, ["character"] = end.Character },
    };

    private static Int32 CompletionKind(Object value) => value?.ToType()["name"]?.ToString() == "Function" ? 3 : 6;

    private static Int32 OffsetAt(String source, Position position)
    {
        var line = 0;
        var index = 0;
        while (line < position.Line && index < source.Length)
        {
            if (source[index++] == '\n')
                line++;
        }
        return Math.Min(index + position.Character, source.Length);
    }

    private readonly record struct Position(Int32 Line, Int32 Character)
    {
        public static Position From(JsonObject value) => new(value?["line"]?.GetValue<Int32>() ?? 0, value?["character"]?.GetValue<Int32>() ?? 0);
    }
}