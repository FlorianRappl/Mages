# First Steps

MAGES is a simple, but very powerful expression evaluator. It was designed to be fast and offer everything that may be required to open small APIs or include a dynamic engine without much overhead.

MAGES runs on essentially any platform that supports .NET 3.5 or higher. It is compatible with .NET core and can be used with Mono in, e.g., a game written using the Unity engine.

## Hello World!

The core classes and functionality lives in the `Mages.Core` namespace. A very simple console "Hello World!" application may thus look as follows:

```cs
using Mages.Core;
using System;

static class Program
{
	static void Main(String[] args)
	{
		var engine = new Engine();
		var result = engine.Interpret("21 * 2");
		Console.WriteLine("The answer to everything is {0}!", result);
	}
}
```

Of course from this point on MAGES is already nearly REPL-usable:

```cs
var engine = new Engine();

while (true)
{
	Console.Write("Query? ");
	var input = Console.ReadLine();
	var result = engine.Interpret(input);
	Console.WriteLine("Answer: {0}", result);	
}
```

At this point the user is free to start interacting with the MAGES `Engine` instance. At this point it makes sense to see how we can interact with the engine in our applications.

## Interaction

MAGES does not come with its own data types. Instead, it uses existing .NET data types to decrease layers and increase performance. As a positive side-effect the performance is improved with less GC pressure. Also the interaction feels more natural.

```cs
var engine = new Engine();
engine.Scope["seven"] = 7.0;
var result = engine.Interpret("seven / 4");
Console.WriteLine(typeof(result).FullName); // System.Double
```

The (global) scope is only a .NET dictionary, which allows us to get and set global variables. In the former example `seven` was a name we introduced. Similarly, results may be stored in this dictionary.

```cs
var engine = new Engine();
engine.Interpret("IsSevenPrime = isprime(7)");
Console.WriteLine(engine.Scope["IsSevenPrime"]); // true
```

MAGES tries to narrow every .NET data type to one of its data types:

* Number (`System.Double`)
* Boolean (`System.Boolean`)
* String (`System.String`)
* Matrix (`System.Double[,]`)
* Object (`System.Collections.Generic.IDictionary<System.String, System.Object>`)
* Function (`Mages.Core.Function`, essentially a `Delegate` mapping `Object[]` to `Object`)
* Nothing (`null`)

Most types will be simply wrapped in a wrapper object that implements the `IDictionary<String, Object>`. One thing we can easily do is to create new functions in MAGES and use them in .NET applications:

```cs
var engine = new Engine();
var euler = engine.Interpret("n => isprime(n^2 + n + 41)");
var testWith1 = (Boolean)euler.Invoke(new Object[] { 1.0 });
var testWith7 = (Boolean)euler.Invoke(new Object[] { 7.0 });
```

The objects that are given to the function defined in MAGES need to be supplied in MAGES compatible types. So the call wouldn't work with integers:

```cs
var isNull = euler.Invoke(new Object[] { 1 });
```

To circumvent such issues there is much better alternative: Using the `Call` extension method. This allows us to do the following:

```cs
var testWith1 = euler.Call<Boolean>(1);
var testWith7 = euler.Call<Boolean>(7);
```

There is also an overload without specifying the return type (resulting in an `Object` instance being returned). The call above returns the default value if the type has not been matched.

The reasoning for including the narrowing in `Call` instead of the usual `Invoke` is to allow MAGES internally to directly call the function without the otherwise introduced narrowing overhead.

## Exposing the API

Until this point we only touched the (user) surface of MAGES. However, below (at the very bottom) there is another layer that cannot be manipulated by user input: The API space. This layer is used to hold functions, e.g., `sin` or `cos` without being in danger of disappearing forever due to scope manipulation from the user.

The API space is accessible via the `Globals` property of the `Engine`. Like the `Scope` the API layer is instance bound, i.e., two different engine instances can look different here.

```cs
var engine = new Engine();
engine.Globals["three"] = 3.0;
```

At first sight interaction in MAGES looks very similar compared to the interaction with the scope. However, the difference lies in the users disability to overwrite functions in here. The suggestion is to use the scope for observing changes / variables done by the user and the globals for define the API to work with.

As the API will mostly consist of functions (and not of constants), helpers to introduce functions are an important part of MAGES.

```cs
var engine = new Engine();
var function = new Function(args => (Double)args.Length);
engine.SetFunction("argsCount", function);
var result = engine.Interpret("argsCount(1, true, [])"); // 3.0
```

If we use `Function` directly we are responsible to care about the types being used. We are sure that only MAGES compatible types are entering, however, at the same time we need to make sure to return only MAGES compatible objects.

Potentially, it is better to just use *any* kind of delegate and pass it in. For instance, the following works as expected.

```cs
var engine = new Engine();
var function = new Func<Double, String, Boolean>((n, str) => n == str.Length);
engine.SetFunction("checkLength", function.Method, function.Target);
var result = engine.Interpret("checkLength(2, \"hi\")"); // true
```

Now, in the former example all used types are MAGES compatible, however, we can even use this with (kind of) arbitrary types:

```cs
var engine = new Engine();
var func = new Func<Int32, String, Char>((n, str) => str[n]);
engine.SetFunction("getCharAt", func.Method, func.Target);
var result = engine.Interpret("getCharAt(2, \"hallo\")"); // "l"
```

In this example `Double` (the MAGES compatible type) gets automatically converted an integer. The result type (a `Char`) is automatically converted to a `String` as well.