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

Similar to functions general objects can be exposed as well. Here MAGES offers the capability of denoting so-called constants, which may be shadowed by the user, but will actually never be overwritten from the user.

```cs
var engine = new Engine();
var constant = Math.Sqrt(2.0);
engine.SetConstant("sqrt2", constant);
var result = engine.Interpret("sqrt2^2"); // 2.0
```

The described way is the preferred alternative to accessing the `Globals` object directly. The main problem with the `Globals` object has been indicated previously. Here no safety net is enabled to prevent MAGES incompatible objects from entering the system. Therefore, it is highly recommended to use the wrappers `SetFunction` and `SetConstant` to provide functions and constants.

What if a constant is not good enough? What if users should be able to create multiple instances? Here a constructor function is the right answer. In the following example the `StringBuilder` class from .NET is exposed to MAGES via a constructor function.

```cs
var engine = new Engine();
var function = new Func<StringBuilder>(() => new StringBuilder());
engine.SetFunction("createStringBuilder", function);
var result = engine.Interpret("createStringBuilder().append(\"Hello\").append(\" \").appendLine(\"World!\").toString()"); // "Hello World!\n"
```

In general such constructor functions are essential combined with features of MAGES such as the automatic wrapping of arbitrary objects. There is, however, an even better way to provide such constructor functions.

## Exposing .NET Types

MAGES makes it easy to expose existing .NET types via the `SetStatic` extension method. Let's start with a simple example:

```cs
var engine = new Engine();
engine.SetStatic<System.Text.StringBuilder>().WithDefaultName();
var result = engine.Interpret("StringBuilder.create().append(\"foo\").appendLine("\bar\").toString()"); // "foobar\n"
```

Compared with the code above this seems rather straight forward and trivial. So what exactly is happening here? First, we are exposing the .NET type `System.Text.StringBuilder` with its default name. In contrast to the previously mentioned extension methods the `SetStatic` does not expose the result directly. Instead, we need to tell MAGES how to expose it. In this case we go for the standard way, which would be by its original name ("StringBuilder"). Two other ways also exist, which will be discussed later.

By convention the constructors are exposed via the `create` method. From this point on the code is equivalent to the one above. Again the underlying .NET type (a `StringBuilder` instance) has been exposed. A legit question would be: Why are the names different?

MAGES comes with the ability to expose .NET types in a API coherent manner. Therefore, every field / property / method / ... name is transformed by a centralized service, an implementation of the `INameSelector` interface. By default the `CamelNameSelector` is used, however, we could replace it if we want to. This name selector changes all .NET names from *PascalCase* to *camelCase*.

So let's expose something else - how about some kind of array?

```cs
var engine = new Engine();
engine.SetStatic<System.Collections.Generic.List<System.Object>>().WithName("Array");
var result = engine.Interpret("list = Array.create(); list.add(\"foo\"); list.add(2 + 5); list.insert(1, true); list.at(2)"); // 7
```

This time we've decided to expose the `List<Object>` type. However, the default name would be impossible to access; if it would be legit at all. Instead, we've decided to give it a custom name - "Array". We can now use the static `Array` object to create (wrapped) instances of `List<Object>`. In this case we name the instance `list`. Finally everything behaves as we've seen before. There is just one new thing here: The `at` function does not exist as such on the .NET `List<Object>`.

MAGES exposes .NET indexers via a convention called the `at` function. This convention, as with the others, can be changed by providing a custom `INameSelector` implementation.

Finally, we can use the `SetStatic` extension method to expose whole collections of functions (or other objects). Let's say we want to expose some functions, e.g.,

```cs
static class MyMath
{
	public static Double Cot(Double x)
	{
		return Math.Cos(x) / Math.Sin(x);
	}

	public static Double Arccot(Double x)
	{
		return Math.Atan(1.0 / x);
	}
}
```

What we could do is (since the class above is `static` we cannot use it with generics, but fortunately there is an overload that accepts a `Type` argument)

```cs
var engine = new Engine();
engine.SetStatic(typeof(MyMath)).WithName("mm");
var result = engine.Interpret("mm.arccot(mm.cot(pi))"); // approx. 0
```

This, however, has essentially placed all these functions in a kind of "namespace" (as its a runtime object its not exactly a namespace of course, however, from the code's perspective it could be regarded as such). What if we want to expose all these functions *directly*, i.e., *globally*? Here the third option comes into play:

```cs
var engine = new Engine();
engine.SetStatic(typeof(MyMath)).Scattered();
var result = engine.Interpret("arccot(cot(1))"); // 1
```

Using `Scattered` the object is decomposed and inserted into the global API layer.