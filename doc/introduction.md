# Introduction

MAGES is the official successor to YAMP. It is a very simple, yet powerful, expression parser and interpreter. We can use MAGES to include a sophisticated, easy to customize, and lightweight scripting engine to our application.

While all this may sound rather abstract we can think of a MAGES as a lightweight tool for scripting that is served in form of a .NET library. It has specifically been written to be compatible with Unity, so it can be the scripting engine of our choice.

## General Idea

The idea of an expression evaluator is as old as higher programming languages. One places an abstract syntax on top of a rather complicated machine and makes it possible to interact with the machine in simple understandable terms. The code written in MAGES is not compiled but interpreted. Even though this may sound like a disadvantage it could come as a surprise that there are many benefits from this choice:

- High startup time
- Reduction of layers
- Understandable outcome
- No deployment constraints

Most importantly, there are no deployment constraints, e.g., even if we use this library with a Xamarin app targeting the Apple AppStore it will pass. There is no MSIL generated anywhere.

The reduction of startup time goes hand in hand with the understandable outcome and reduction of layers. MAGES using a simple high-level virtual machine to execute a linear chain of stateless operations. Even though this may sound complicated it yields two direct advantages:

- No GIL required
- Operation chains can be transformed to .NET functions

Code in MAGES can be invoked from .NET. With the reusability of operations chains we obtain a mighty sword that allows us to cache already validated expressions (i.e., "compiled"). These cached chains can be executed arbitrarily without running into problems caused by MAGES.

## Scope

MAGES contains everything from validating a given source code (e.g., representing by a .NET `String`) to evaluating it. Helpers to find missing symbols, look, traverse, or modify the abstract syntax tree (AST) are available as well. The whole architecture is pluggable, i.e., we could throw in our own parser to re-use the whole runtime without sacrificing any of the gains.

MAGES has set the priority as follows:

1. Performance
2. Understandable
3. Lightweight

Performance was one of the main reasons for following this project. Until now there have been two kinds of math expression evaluators. Either very simple ones (usually quite fast) or very bulky overcomplex ones (some are decent fast, but most of them are quite slow). YAMP, which is predecessor of MAGES, tried to be an elegant solution that rather lives on the boundary to be decently fast. However, the inner architecture was still too complex and the maintenance costs seemed to be way too high.

In what kind of projects does MAGES make most sense? We could see big advantages when:

- User input needs to be evaluated
- A DSL is obligatory to ensure loose coupling
- A solution should be sandboxed
- Users need to be able to script parts
- Small plugins to extend existing applications
- Different applications need to be tied together
- Dynamically program safely against an unknown target

Of course, none of these scenarios can be solely handled by MAGES. Most of these could scenarios could be also solved via C# itself. However, the cost of running, e.g., Roslyn or ScriptCS is way higher than MAGES. Also it may be incompatible with the target platform. Finally, sandboxing these solutions is more cumbersome.

## Extensions and Modules

MAGES comes with the promise of extensibility. As it is simple and straight forward to expose *any* .NET code, literally *any* kind of code can be used *without much effort* from MAGES. The possibilities of the DLR (using the Iron-languages, e.g., IronRuby or IronPython) are given. Even simpler is the usage of native libraries via PInvoke. MAGES opens the door to the scripting within the whole .NET world.

The REPL makes use of this promise by introducing a module system, similar to the one being found in, e.g., Node.js. In the standard MAGES executable (which uses the MAGES library together with some custom functions and other useful things) the *Modules* plugin is loaded. This plugin adds two key functions to the given `Engine` instance. We get

- `import` to import the code exported in a module and
- `export` to export some functionality.

Custom hooks for the `import` function exist. In the standard executable the following hooks are present:

- evaluating another MAGES file; here the exported functionality is imported
- including a .NET library; here the public types are imported
- including a NuGet package; here the library is included as previously mentioned

MAGES is therefore a great and simple tool to explore NuGet packages without firing up Visual Studio and creating a pseudo solution.

So let's see how the modularity may help us. Let's say we are working in a directory with two files, *helpers.ms* and *main.ms*. The *helpers.ms* file may look as follows:

```C
export(new 
{
  prop: (name, obj) => obj(name),
  /* ... */
});
```

In our *main.ms* file we may now use this additional functionality:

```C
var h = import("helpers.ms");
engine | h.prop("version") | console.writeln;
```

Generally, the usage of modules allows writing reusable code that can come in handy in many scenarios.

## REPL

As already mentioned MAGES is a full library - not a language. Of course, MAGES comes with a specified [language](language.md) strictly defined by a [syntax](syntax.md), however, it is possible to adjust many features of MAGES - including the language itself - by the project requirements. Therefore, it is important to understand that the delivered REPL (short for Read-Evaluate-Print-Loop, i.e., a simple console application to enter expressions which are directly evaluated) only represents the default state.

The default state of MAGES has been spiced up with some auxiliary functions that enable a lot of interesting scenarios, where otherwise PowerShell, Bash, or Node.js would be used (just to name a couple of command line / general-purpose scripting environments).

On starting the REPL we see the following:

```plain

     _____      _____    ___________________ _________
    /     \    /  _  \  /  _____/\_   _____//   _____/
   /  \ /  \  /  /_\  \/   \  ___ |    __)_ \_____  \
  /    Y    \/    |    \    \_\  \|        \/        \
  \____|__  /\____|__  /\______  /_______  /_______  /
          \/         \/        \/        \/        \/

  (c) Florian Rappl, 2016
  Version 0.4.5995
  Running on Microsoft Windows NT 6.2.9200.0

  For help type 'help()'.

SWM>
```

The prompt `SWM` means "SpielWiese Mages" (German for "MAGES playground"). The REPL will not evaluate expressions but also provide error hints in case of syntax problems. Consider the following missing bracket.

```plain
SWM> x = (2 + 3) * (4 - 5
x = (2 + 3) * (4 - 5
                    ^
Error: An expected closing paranthesis could not be found ( ')' ).
```

This, of course, works with any expression. For instance we could have a typo in an escape sequence.

```plain
SWM> x = "This is the first string!\n"; y = "This is the second string!\m"
ng!\n"; y = "This is the second string!\m"
                                        ^
Error: The given escape sequence is invalid.
```

Finally, the REPL auto-stores results of the previous evaluation in a global variable called `ans`. The following snippet illustrates this behavior.

```plain
SWM> 2^2^2
16
SWM> ans
16
```

Help on the available functions can be retrieved via the `help()` function.

## Conclusions

MAGES is certainly not the only tool available to cover our scripting needs, but depending on our requirements it may be the ideal choice. MAGES is simple, lightweight, focused, and highly performant. It still offers a nice API layer that makes it possible to come up with sophisticated editor support and state-of-the-art tooling.

The other documents provide more insights and specific topics, e.g., the [language and its features](language.md) itself or an overview over the [integrated functions](functions.md).