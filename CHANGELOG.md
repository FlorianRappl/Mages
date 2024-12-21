# 3.0.0

- Fixed potential misinterpretation of fractions in hex numbers (#128)
- Fixed usage of capital `X` or `B` for hex or binary representations
- Fixed bug in JSON representation using trailing comma
- Improved GC allocations (#81)
- Added wrapping of `Task` in `Future` (#64)
- Added octal representation (e.g., `0o123`) for numbers
- Added insertion points (`_`) to function calls (#129)
- Added JSX syntax (#120)
- Added default `jsx` and `html` function (#120)
- Added events to `Engine` to handle uncaught errors (#121)

# 2.0.3

- Include checksum in chocolatey package

# 2.0.2

- Fixed vulnerability using outdated version of `System.Drawing.Common` for plugins
- Fixed ordering of parameters with likelihood (#118)
- Added `SwitchableScope` class to achieve scope switching easily (#119)
- Added improved auto usage of `IEnumerable<>` constructors (#116)

# 2.0.1

- Fixed potential overflow in number scanning (#110)
- Enhanced documentation about list types (#109)
- Enhanced documentation about results when expecting complex result type (#108)
- Added phase function for complex numbers (#107)
- Added tooltip comments when manipulating scopes directly (#106)
- Improved handling of `min` / `max` in combination with complex numbers (#101)
- Changed `Squirrel.windows` to `Clowd.Squirrel` for the standalone REPL

# 2.0.0

- Migrated to use GitHub actions
- Added missing helpers and constants (#98)
- Added complex helpers (#97)
- Added complex numbers support (#96)
- Changed the range operator from `:` to `..` (#76)
- Changed library target to .NET Standard 2 (#68)
- Changed application to run on .NET Core 3.1 (#68)
- Changed types to be reflected as objects (#60)

# 1.6.1

- Fixed `factorial` bug (#94)

# 1.6.0

- Included `Clip` function (#89)
- Fixed long integer parsing (#88)
- Allow Forced New Lines in REPL (#87)

# 1.5.0

- Added support for named arguments (#65)
- Included `clamp` and `lerp` function (#80)
- Preprocessor token (#78)
- Allow variable escaping in literal interpolated string (#79)
- Included regular expressions function (#77)
- Fixed onterpolated string escape sequence (#75)

# 1.4.0

- Added API support for callback functions
- Allow multiple arguments for `min` (#73)
- Allow multiple arguments for `max` (#73)
- Allow multiple arguments for `sum` (#73)
- Allow multiple arguments for `any` (#73)
- Allow multiple arguments for `all` (#73)
- Allow multiple arguments for `sort` (#73)
- Cast bool to number (#70)

# 1.3.0

- Added pattern matching (#58)
- Improved REPL termination (#67)
- Allow literal strings (#63)

# 1.2.0

- Auto completion for member operator (#66)
- Move delete from statement to expression (#62)
- Await to resolve futures (#59)
- Optional arguments language (#57)
- Smaller bug fixes

# 1.1.0

- Autocomplete for the REPL (#54)
- Improved error display (#55)
- `delete` existing variables (#61)
- Enable `for` loops (#53)
- Autocompletion enhancements (#54)
- Minor bug fixes

# 1.0.0

- Add more standard functions (#48)
- Document existing plugins (#47)
- Refine existing functions (#46)
- `this` for member functions (#45)
- Random plugin (#44)
- Include Nuget packages (#43)
- Include .NET Libraries (#42)
- Fixed some bugs
- Use relative jump instructions
- Extended `is` function to check objects
- Various REPL improvements

# 0.9.0

- Some improvements and fixes
- Import and export in REPL (#41)
- Serialize instructions and AST (#40)
- `intersection`, `union`, and `except` functions (#39)		
- `zip` and `concat` functions (#38)
- `reduce` and `where` functions (#37)
- Added interpolated strings (#35)
- Filesystem plugin for the REPL (#36)
- Some more REPL plugins

# 0.8.0

- Included x-assignment operator (#34)
- Provided `map` function (#33)
- Introduced attachable properties (#32)
- Extended the documentation (#31)
- Created an `ObservableDictionary` helper (#30)
- REPL improvements
- Distribute installer via Chocolatey

# 0.7.0

- Provided `is` / `as` functions (#29)
- Included the pipe operator (#28)
- Autocomplete / "intellisense" (#23)
- Released plugin architecture (#22)
- Operator improvements (`==` and `~=`)

# 0.6.0

- Some fixes and performance improvements
- Enhanced validation (#27)
- Created installer for the REPL (#26)
- Provided the if statement (#19)
- Included while (#20)
- Control loops via break (#24) and continue (#25)
- Fixed some smaller bugs

# 0.5.0

- Source code validation (#21)
- `return` statement (#18)
- Lists via `list` function (#17)
- Variable arguments collected in `args` (#16)
- Auto currying for functions (#15)
- Refactored standard functions
- Fixed increment and decrement operators
- Thread-safe operations
- REPL improvements

# 0.4.0

- Extended operators (binary and unary)
- Improved VM for performance gain
- Allow block statements in functions (#14)
- Expose API via `SetStatic` (#13)
- Include type functions (#12)
- Provide more user documentation (#10)

# 0.3.0

- Mixed operations work now
- Included more functions, e.g., `catch` and `throw` functions
- Integrated elementary matrix operations (#11)
- Object transformation and wrapping (#9)
- Added logical functions (#8)
- Implemented comparison functions (#5)

# 0.2.0

- Added trigonometric function (#7)
- Confirmed performance (#4)
- Allow extending with own functions (#3)

# 0.1.0

- Initial release
- Random numbers (#6)
- Arithmetic functions (#2)
- Simple interpretation (#1)