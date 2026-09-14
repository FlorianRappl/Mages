# MAGES Language Support for Visual Studio Code

![MAGES logo](./logo-96x96.png)

Visual Studio Code extension providing rich language support for the [MAGES](https://mages.anglevisions.com) scripting language via Language Server Protocol (LSP). The language server is implemented in .NET and uses the `Mages.Core` library directly, so completions, diagnostics, symbols, hover information, and signatures follow the actual MAGES runtime rather than a duplicated JavaScript catalog.

## Installation

Install **MAGES Language Support** from the Visual Studio Code Marketplace, or install a packaged VSIX from the command line:

```sh
code --install-extension vscode-mages-4.0.0.vsix
```

The extension supports `.mages` and `.swm` files. The bundled language server targets .NET 10, so install the [.NET 10 runtime](https://dotnet.microsoft.com/download/dotnet/10.0) before opening a MAGES file.

## Features

- **Syntax Highlighting**: Full grammar coverage for expressions, numbers (decimal, hex, binary, octal), strings, interpolation, JSX, and operators.
- **Diagnostics**: Real-time parser and AST validation from the MAGES .NET library.
- **IntelliSense & Autocompletion**: Completions from the configured MAGES engine globals and source symbols.
- **Hover Information**: Runtime-derived type and function signatures.
- **Document Symbols & Outline**: Navigate functions and variables in the Outline view and Breadcrumbs.
- **Signature Help**: Parameter tooltips while writing function calls.

## Language Server

The extension is a thin VS Code LSP client. Language analysis runs in the bundled `Mages.LanguageServer` .NET process over standard input and output. This keeps editor behavior aligned with the MAGES parser, engine globals, plugins, and runtime, and avoids maintaining a second JavaScript implementation of the language.

## File Associations

- `.mages`
- `.swm`

## Links

- [MAGES homepage](https://mages.anglevisions.com)
- [MAGES source repository](https://github.com/FlorianRappl/MAGES)
- [MAGES documentation](https://mages.anglevisions.com/docs/introduction)
