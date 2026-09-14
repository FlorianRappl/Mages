# MAGES

*MAGES* (Mages: Another Generalized Expression Simplifier) is a lightweight, ultra-fast expression parser, compiler, and interpreter for .NET. It lets you embed a sandboxed scripting engine into your application without any external runtime dependencies.

This package contains the MAGES library (`Mages.Core`). If you are looking for the command-line tool and REPL, install [`Mages.Compiler`](https://www.nuget.org/packages/Mages.Compiler/) instead.

## Installation

```sh
dotnet add package Mages
```

The library targets .NET Standard 2.1 and therefore runs on .NET Core 3.0+, .NET 5+, Mono 6.4+, and Unity 2021.2+.

## Usage

Create an engine to hold a global scope and start interpreting:

```cs
using Mages.Core;

var engine = new Engine();
var result = engine.Interpret("sin(2) * cos(pi / 4)"); // 0.642970376623918
```

Compile once, run many times:

```cs
var expOne = engine.Compile("exp(1)");
var result = expOne(); // 2.71828182845905
```

Expose your own data and functions to scripts:

```cs
engine.Scope["x"] = 4.0;
engine.SetFunction("sq", (Double v) => v * v);

var result = engine.Interpret("sq(x) + 1"); // 17.0
```

Get functions defined in MAGES back into C#:

```cs
var func = engine.Interpret("(x, y) => x * y + 3 * sqrt(x)") as Function;
var result = func.Call(4, 3); // 18.0
```

## Features

- Numbers, complex numbers, strings, booleans, matrices, objects, and first-class functions
- Interpolated strings, JSX syntax, pattern matching, pipes, and placeholders for currying
- No IL emitted at runtime, so it also works on platforms with AOT restrictions
- Full control over the exposed API, which makes sandboxing user scripts straightforward
- Editor tooling support such as auto-completion (`GetCompletionAt`) and AST validation

## Documentation

- [Getting started](https://github.com/FlorianRappl/MAGES/blob/main/doc/first-steps.md)
- [Language features](https://github.com/FlorianRappl/MAGES/blob/main/doc/language.md)
- [Standard functions](https://github.com/FlorianRappl/MAGES/blob/main/doc/functions.md)
- [Type system](https://github.com/FlorianRappl/MAGES/blob/main/doc/types.md)

## License

MIT - see the [license file](https://github.com/FlorianRappl/MAGES/blob/main/LICENSE) for details.
