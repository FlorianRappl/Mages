# Plugins

The `Engine` provided by MAGES opens the door for many extensions. Besides integrating custom objects, types, and functions one may include a whole package of them. This mechanism is called a *plugin*. The key is to use either the `AddPlugin` / `RemovePlugin` methods of the `Engine` or an `AddPlugin` extension method.

A plugin is just a container that consists of

- metadata and
- content.

The latter is the "meat" that is used from the scripting perspective. The metadata may be interesting when writing a specific kind of client or using MAGES for a plugin host. The metadata may yield data such as the name of the plugin, its author, the version, potential dependencies, and anything else that seems interesting.

## Creating a Plugin

There are two ways to create a plugin:

1. By hand: Bundle everything together and ship it as a `Plugin` instance.
2. Automatically: Provide a type that matches the plugin convention.

Since the first variant is straight forward we'll have a closer look at the second option. Here we provide a `static class` that uses the following styles:

* The name of the class ends with `Plugin`, e.g., `MyPlugin`.
* It contains `public static readonly` fields of type `String`. The name of the fields is the metadata key, the field's value represents the metadata value.
* Public properties with a getter are used as constants.
* Public methods are used as functions.

The following code shows such a plugin with three metadata values (name, version, and author). There are three constants (bar, foo, and identity), as well as one function (numberOfArguments) exposed.

```csharp
static class MyPlugin
{
    public static readonly String Name = "Foo";
    public static readonly String Version = "1.0.0";
    public static readonly String Author = "Author";

    public static Double Bar { get { return 2.0; } }
    public static Boolean Foo { get { return true; } }
    public static Double[,] Identity { get { return new Double[2, 2] { { 1.0, 0.0 }, { 0.0, 1.0 } }; } }
    public static Object NumberOfArguments(Object[] args)
    {
        return (Double)args.Length;
    }
}
```

The great advantage of a plugin is the ability to remove it. This makes searching for still existing functionality unnecessary. Also further checks are applied to keep keys that have been overwritten since the plugin has been introduced.

### Existing Plugins

The REPL comes with some handy plugins already loaded. The main purpose of these plugins is to supply further functionality and make small tasks (e.g., saving some text in a file) much simpler.