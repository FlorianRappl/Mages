# MAGES Compiler

*MAGES* (Mages: Another Generalized Expression Simplifier) is a lightweight, ultra-fast expression parser, compiler, and interpreter for .NET.

This package is the cross-platform command-line application: it provides the `mages` command, which runs MAGES scripts and offers an interactive REPL. If you want to embed MAGES into your own application, use the [`Mages`](https://www.nuget.org/packages/Mages/) library instead.

## Installation

```sh
dotnet tool install --global Mages.Compiler
```

Updating to a newer release:

```sh
dotnet tool update --global Mages.Compiler
```

The tool requires the .NET 10 runtime.

## Usage

Start the interactive REPL:

```sh
mages
```

Run a script file:

```sh
mages path/to/script.ms
```

Walk through the built-in interactive tutorial:

```sh
mages --tutorial
```

## In the REPL

```plain
SWM> 2 + 3
5
SWM> var f = (x, y) => x^2 + y
SWM> f(3, 1)
10
SWM> ans * 2
20
```

The REPL comes with multi-line editing, history, and tab auto-completion. Results of the previous evaluation are available in the `ans` variable, and `help()` lists the available functions.

Besides the standard library the tool ships with plugins for file system access, linear algebra, plotting, drawing, random numbers, transpilers, and a module system exposing `import` / `export` - the latter can even load .NET assemblies and NuGet packages.

## Documentation

- [Introduction](https://github.com/FlorianRappl/MAGES/blob/main/doc/introduction.md)
- [Language features](https://github.com/FlorianRappl/MAGES/blob/main/doc/language.md)
- [Standard functions](https://github.com/FlorianRappl/MAGES/blob/main/doc/functions.md)
- [Plugins](https://github.com/FlorianRappl/MAGES/blob/main/doc/plugins.md)

## License

MIT - see the [license file](https://github.com/FlorianRappl/MAGES/blob/main/LICENSE) for details.
