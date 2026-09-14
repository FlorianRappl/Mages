# MAGES Language Support for Visual Studio Code

Visual Studio Code extension providing rich language support for [MAGES](https://github.com/FlorianRappl/MAGES) scripting language via Language Server Protocol (LSP). The language server is implemented in .NET and uses the `Mages.Core` library directly, so completions, diagnostics, symbols, hover information, and signatures follow the actual MAGES runtime rather than a duplicated JavaScript catalog.

## Features

- **Syntax Highlighting**: Full grammar coverage for expressions, numbers (decimal, hex, binary, octal), strings, interpolation, JSX, and operators.
- **Diagnostics**: Real-time parser and AST validation from the MAGES .NET library.
- **IntelliSense & Autocompletion**: Completions from the configured MAGES engine globals and source symbols.
- **Hover Information**: Runtime-derived type and function signatures.
- **Document Symbols & Outline**: Navigate functions and variables in the Outline view and Breadcrumbs.
- **Signature Help**: Parameter tooltips while writing function calls.

## File Associations

- `.mages`
- `.swm`
