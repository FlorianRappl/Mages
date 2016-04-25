# MAGES

## Mages: Another Generalized Expression Simplifier

MAGES is the official successor to YAMP. It is a very simple, yet powerful, expression parser and interpreter. You can use MAGES to include a sophisticated, easy to customize, and lightweight scripting engine to your application.

### Installation

MAGES itself does not have any dependencies, however, the tests are dependent on NUnit and the benchmarks use BenchmarkDotNet. Usually, MAGES should be installed via the NuGet package source. If this does not work for you, then clone the source and build MAGES yourself. Make sure that all unit tests pass.

### Documentation

The documentation is given in form of Markdown documents being placed in the *doc* folder of this repository. The following links are worth checking out:

* [Documentation of the MAGES syntax](doc/syntax.md)

### Contributions

Contributions are highly welcome, but need to be performed in an organized and consistent way. The project follows the Git flow to ensure traceability, reduce conflicts, and improve the project management.

The following guide should help you getting on track.

1. If no issue already exists for the work you'll be doing, create one to document the problem(s) being solved and self-assign the issue.
2. Otherwise, please let us know that you are working on the problem. Regular status updates (e.g., "still in progress", "no time anymore", "practically done", "pull request issued") are highly welcome. A possible code-complete date helps us placing the issue in the overall roadmap.
2. Create a new branch! Please don't work in the `master` branch directly. It is reserved for stable versions, i.e., releases. We recommend naming the branch to match the issue being addressed (`feature-#777` or `issue-777`), but we accept all names except `master` and `devel`.
3. Add failing tests for the change you want to make. Tests are crucial and should cover the code involved in the issue.
4. Fix stuff. Always go from edge case to edge case.
5. All tests should pass now. Also your new implementation should not break existing tests.
6. Update the documentation to reflect any potential changes.
7. Push to your fork or push your issue-specific branch to the main repository, then submit a pull request against `devel`. Never create a PR against the `master` branch!

Contributions may also be taken in form of bug reports and feature requests. Long live open-source development!

### Versioning

The rules of [semver](http://semver.org/) are our bread and butter. In short this means:

1. MAJOR versions at maintainers' discretion following significant changes to the codebase (e.g., API changes)
2. MINOR versions for backwards-compatible enhancements (e.g., performance improvements)
3. PATCH versions for backwards-compatible bug fixes (e.g., spec compliance bugs, support issues)

Hence: Do not expect any breaking changes within the same major version.

## License

The MIT License (MIT)

Copyright (c) 2016 Florian Rappl

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.