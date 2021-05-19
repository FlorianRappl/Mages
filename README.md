![MAGES Logo](https://raw.githubusercontent.com/FlorianRappl/Mages/master/logo.png)

# MAGES

[![GitHub CI](https://github.com/FlorianRappl/Mages/actions/workflows/ci.yml/badge.svg)](https://github.com/FlorianRappl/Mages/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/MAGES.svg?style=flat-square)](https://www.nuget.org/packages/Mages/)
[![Issues](https://img.shields.io/github/issues/FlorianRappl/MAGES.svg?style=flat-square)](https://github.com/FlorianRappl/Mages/issues)

## Mages: Another Generalized Expression Simplifier

MAGES is the official successor to [YAMP](https://github.com/FlorianRappl/YAMP). It is a very simple, yet powerful, expression parser and interpreter. You can use MAGES to include a sophisticated, easy to customize, and lightweight scripting engine to your application.

Among other applications, MAGES is used in [Microsoft's PowerToys](https://github.com/microsoft/PowerToys).

### Current Status

MAGES was just updated with support for complex numbers. Also, the build target and runtime has been updated to make use of modern possibilities.

### Previous Status

The first stable version has been released. The current version 1.6.0 contains an improved REPL. The library contains everything to perform lightweight scripting operations in C#. A [CodeProject article](http://www.codeproject.com/Articles/1108939/MAGES-Ultimate-Scripting-for-NET) about the library (also containing some background and performance comparisons) is also available.

### Installation

MAGES itself does not have any dependencies, however, the tests are dependent on NUnit and the benchmarks use BenchmarkDotNet. Usually, MAGES should be installed via the NuGet package source. If this does not work for you, then clone the source and build MAGES yourself. Make sure that all unit tests pass.

The whole library was designed to be consumed from .NET 3.5 (or higher) applications. This means it is (amongst others) compatible with Unity / Mono 2.6. The NuGet package is available via [the official package feed](https://www.nuget.org/packages/MAGES).

### Get Me Started!

In the most simple case you are creating a new engine to hold a global scope (for variables and functions) and launch the interpretation.

```cs
var engine = new Mages.Core.Engine();
var result = engine.Interpret("sin(2) * cos(pi / 4)"); // 0.642970376623918
```

You can also go-ahead and make reusable blocks from snippets.

```cs
var expOne = engine.Compile("exp(1)");
var result = expOne(); // 2.71828182845905
```

Or you can interact with elements created by MAGES.

```cs
var func = engine.Interpret("(x, y) => x * y + 3 * sqrt(x)") as Mages.Core.Function;
var result = func.Invoke(new Object[] { 4.0, 3.0 }); // 18.0
```

Or even simpler (details are explained in the [getting started](doc/first-steps.md) document):

```cs
var func = engine.Interpret("(x, y) => x * y + 3 * sqrt(x)") as Mages.Core.Function;
var result = func.Call(4, 3); // 18.0
```

These are just some of the more basic examples. More information can be found in the documentation.

### Documentation

The documentation is given in form of Markdown documents being placed in the *doc* folder of this repository. The following links are worth checking out:

* [First steps](doc/first-steps.md)
* [Syntax documentation](doc/syntax.md)
* [Included functions](doc/functions.md)
* [Type system](doc/types.md)
* [Performance evaluations](doc/performance.md)

If anything is missing, unclear, or wrong then either submit a PR or file an issue. See the following section on contributions for more information.

### Contributions

Contributions in form of feature implementations or bug fixes are highly welcome, but need to be performed in an organized and consistent way. The [contribution guidelines](doc/contributing.md) should be read before starting any work.

Contributions may also be taken in form of bug reports and feature requests. Long live open-source development!

### Versioning

The rules of [semver](http://semver.org/) are our bread and butter. In short this means:

1. MAJOR versions at maintainers' discretion following significant changes to the codebase (e.g., breaking API changes)
2. MINOR versions for backwards-compatible enhancements (e.g., performance improvements, additional extensions)
3. PATCH versions for backwards-compatible bug fixes (e.g., specification compliance bugs, support issues)

Hence: Do not expect any breaking changes within the same major version.

## Sponsors

The following companies sponsored part of the development of MAGES.

<div style="display:flex;justify-content:space-evenly;align-items:center">
    <img width="150" height="150" src="https://raw.githubusercontent.com/polytroper/polytroper.github.io/master/favicon.png">
    <img width="200" height="200" src="https://smapiot.com/smapiot_green.03d1162a.svg">
    <img width="200" height="200" src="https://www.omicron-lab.com/fileadmin/website/images/OMICRON-LAB.svg">
</div>

Thanks for all the support and trust in the project!

## License

The MIT License (MIT)

Copyright (c) 2016-2021 Florian Rappl

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
