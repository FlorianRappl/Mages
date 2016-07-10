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

## Existing Plugins

The REPL comes with some handy plugins already loaded. The main purpose of these plugins is to supply further functionality and make small tasks (e.g., saving some text in a file) much simpler.

### Draw

The drawing plugin allows to create and manipulate images. The entry point is the `canvas` object. Loading an existing image works with the `load` function (accepting a byte array, e.g., obtained via `file.read`). A blank canvas can be created via `create`. This function requires two parameters, specifying the width and height of the image. After manipulation the canvas's content can be retrieved via the `content` function.

Right now the API supports the following toggling functions, i.e., if supplied with a single value the function return the canvas instance and set the value, otherwise the current value is returned.

* `color(value)`
* `solidBrush(value)`
* `thickness(value)`

Besides the toggling functions the image can be manipulated with paths:

* `startPath()`
* `endPath()`
* `moveTo(x, y)`
* `lineTo(x, y)`

At any point in time a path may be manifested by using the selected pen or the brush.

* `stroke()`
* `fill()`

The `content()` function yields the image's current binary representation.

### FileSystem

The file system plugin exposes three objects: `file`, `path`, and `dir`. Additionally, two handy helper functions are introduced: `toBase64` and `fromBase64`. These helpers are able to convert a byte array into a base64 string and vice versa.

In general the functions used by the `file` object return futures to prevent a blocking of the UI. These futures have the following API:

* `done` (boolean)
* `error` (error message if any)
* `result` (result if any)
* `notify` (optional callback to be notified when `done` switches from `false` to `true`)

The futures are used in many plugins and REPL-induced functions.

### LinearAlgebra

The linear algebra plugin is a collection of functions dealing with matrices. The focus lies in solving the eigenvalue problem and inverting a matrix.

* `cg(A, b)`
* `cholesky(mat)`
* `det(mat)`
* `eigen(mat)`
* `givens(mat)`
* `gmres(A, b)`
* `householder(mat)`
* `inverse(mat)`
* `lu(mat)`
* `qr(mat)`
* `solve(A, b)`
* `svg(mat)`
* `trace(mat)`

Some of these functions return an object with more information, e.g., the `eigen` function returns an object containing the eigenvectors, real and imaginary eigenvalues.

### Modules

The modules plugin contains two functions, called `import` and `export`. These functions can be used to import functionality from some module (this may be anything from a .NET library, to a NuGet package, or another Mages file). `export` is used to declare functionality in another Mages file as being public and usable by other modules.

* `import(moduleName)`
* `export(functionality)`

Importing works by specifying a string declaring the module. This returns the exported functionality. The functionality may be any kind of Mages object - ranging from a simple boolean to a composed object or even a function.

### Plots

The plots plugin builds upon the platform-independent core of Oxyplot. It exposes the `PlotModel` in a Mages compatible form that allows for rapid prototyping. There is no rendering defined, i.e., one is free to use any kind of renderer, e.g., Windows Forms, WPF, or an image.

The main functionality is contained in the `plot` object.

### Random

The random plugin comes with the `rng` object exposing some functionality of the Troschütz Random library. This object contains a set of useful random number distributions, all accessible via a simple function. Invoking the function with the required arguments another function that can be used to generate numbers according to selected distribution. The generated function behaves exactly like the `rand` standard function.

The contained distribution functions read:

* `weibull(rng, alpha, beta)`
* `gamma(rng, alpha, beta)`
* `exp(rng, lambda)`
* `chisq(rng, alpha)`
* `beta(rng, alpha, beta)`
* `gauss(rng, mu, sigma)`
* `laplace(rng, alpha, mu)`
* `pow(rng, alpha, beta)`
* `studT(rng, nu)`
* `pareto(rng, alpha, beta)`
* `cauchy(rng, alpha, gamma)`
* `uni(rng, alpha, gamma)`
* `disc(rng, alpha, gamma)`
* `bernoulli(rng, alpha)`
* `poisson(rng, lambda)`

The first argument is the actual random number generator. The plugin comes with some generators:

* `standard(seed)`
* `mt19937(seed)`
* `nr3(seed)`
* `xorShift128(seed)`

The `seed` argument is optional. By default a random seed is selected.

As an example calling `d = rng.gauss(rng.mt19937(), 0, 1)` gives a function stored in `d` that can return numbers generated by the normal distribution around 0 with a standard deviation of 1. So `d()` will return a single number, while `d(3, 3)` will return 9 numbers stored in a 3x3 matrix. Random number generators (here obtained via `rng.mt19937()`) should be used across multiple distributions.