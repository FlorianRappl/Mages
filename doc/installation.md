# Installation

MAGES can be used as a .NET library, a command-line application, or with Visual Studio Code. Choose the installation that matches how you want to use the language.

## .NET Library

Install the `Mages` NuGet package in an existing .NET application:

```sh
dotnet add package Mages
```

The package targets .NET Standard 2.1. It provides the `Mages.Core` engine for parsing, validating, compiling, and evaluating MAGES source code. See [First Steps](first-steps.md) for examples of creating an `Engine` and evaluating expressions.

## Command-Line Tool and REPL

Install the `Mages.Compiler` .NET tool globally:

```sh
dotnet tool install --global Mages.Compiler
```

This installs the `mages` command. Start an interactive session with:

```sh
mages
```

You can also evaluate a script file directly:

```sh
mages path/to/script.ms
```

To update an existing installation, use:

```sh
dotnet tool update --global Mages.Compiler
```

The compiler and REPL target .NET 10, so the [.NET 10 SDK or runtime](https://dotnet.microsoft.com/download/dotnet/10.0) must be available on the machine.

## Visual Studio Code

The MAGES extension supports `.mages` and `.swm` files with syntax highlighting, diagnostics, completion, hover information, document symbols, and signature help.

Install the extension from a packaged VSIX:

```sh
code --install-extension vscode-mages-4.0.0.vsix
```

Alternatively, open the Extensions view in Visual Studio Code and search for **MAGES Language Support** when the extension is available in the Marketplace.

The extension contains the MAGES language server and the `Mages.Core` library. The language server is implemented in .NET and communicates with Visual Studio Code through LSP over standard input and output. It uses the actual MAGES parser, engine globals, and AST services for language features rather than maintaining a separate JavaScript list of functions.

The bundled language server targets .NET 10. Install the [.NET 10 runtime](https://dotnet.microsoft.com/download/dotnet/10.0) before opening a MAGES file in Visual Studio Code. The extension itself still uses Node.js only as the VS Code client host; language analysis is performed by the .NET server.

## Choosing an Installation

- Use the `Mages` package when embedding MAGES in a .NET application.
- Use `Mages.Compiler` when you want the `mages` command, a REPL, or script execution from a terminal.
- Use the VS Code extension when you want editor support while writing MAGES files.

These installations can be used together. For example, the compiler and VS Code extension both use the same MAGES language and .NET runtime foundations.
