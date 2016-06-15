# Language Features

The language that comes with MAGES is nothing spectacular. Instead, it tries to be familiar to most programmers without sacrificing readability or being less concise.

There are some nice features, but nothing that has not been seen before. In general, programmers with a JavaScript or C# background should feel fairly familiar right away. The matrix usage is a little bit inspired from MATLAB without going as far as YAMP.

## Objects

Objects are at the heart of MAGES. Today objects should not be missed from any programming language.

### Literals

Since MAGES defines block statements in the regular C way (opening and closing braces `{}`) the conflict with JavaScript-like object literals had to be avoided in some way. JavaScript circumvented this by disallowing starting an expression with an object literal, however, this makes it ambiguous in the function literal.

The solution of MAGES uses the following literal:

```C
obj = new { } // empty object
obj = new { a: 2, b: 3 } // object with two entries, a = 2, b = 3
obj = new { done: false } // object with one entry, done = false
obj = new { "my key": [1, 2, 3] } //object with one entry, my key = [1, 2, 3]
```

The key is given as either an identifier or a string literal. The value may be anything.

### .NET Wrapping

The [get started](first-steps.md) guide has plenty of information how .NET types can be exposed via MAGES. To make it short: It is not only possible but highly encouraged. MAGES offers many possibilities to customize this experience and to come up with exactly the wrapping one desires.

Some .NET types will be simply converted. For instance, if an `Int32` is passed into MAGES a `Double` will continue to flow inside MAGES. Similarly, output values from calling .NET functions are converted to continue internally.

While delegates and methods are converted to MAGES functions arbitrary objects are always wrapped in `IDictionary<String, Object>` instances. This enables a pleasant working ground for users of MAGES.

### Lists

MAGES does not offer a classical array or list notation. Instead, it introduces a new function called `list`, which transforms its arguments to an object enumerating its keys from `0` to `length - 1`. This "list object" is treated like any other object, even though some helpers exist.

### Index Accessors

MAGES does not come with dedicated indexers. Instead, MAGES uses special functions called "type functions" to resolve type specific functionality that is exposed via calling a function. For instance, we can do

```C
x = new { a: 2 };
x("a")
```

to retrieve the value of "a" (`2` in this case). Similarly, we can also use a more direct way that circumvents the type function lookup:

```C
x = new { a: 2 };
x.a
```

The member function works for objects exclusively.

Both ways, the type function and the member access, work with getting **and** setting.

```C
x("b") = 3 // set a new entry b = 3
x.b = pi // set a new entry b = 3.14...
```

## Matrices

Matrices in MAGES are simple two-dimensional double arrays of .NET (`Double[,]`). This choice has many advantages, most importantly, that no transformations need to be applied before any operation. This is by far the most elegant (and efficient) solution regarding performance / memory usage.

### Literals

Matrices are created by using a literal. The literal looks similar to array literals in other language (e.g., JavaScript), however, does not accept non-number values. Any non-number will be converted (potentially `NaN`).

```C
M = [1,2,3,4] // 1x4 matrix ("vector")
M = [1,2;3,4] // 2x2 matrix ("square")
M = []
```

### Arithmetic Usage

The language contains the usual arithmetic operators and feels natural coming from other programming languages such as C, JavaScript, or MATLAB. Besides the operators described in the next section every operation can be invoked by calling a function. For the addition the function is called `add`.

```C
add(2, 3) // same as 2 + 3
subtract(5, 2) // same as 5 - 2
factorial(3) // same as 3!
```

The idea is that operators are also useful in piping scenarios, i.e., when used such that

```C
3 | add(5) | subtract(10) // same as subtract(10, add(5, 3))
```

This idea is supported by arithmetic functions and auto currying, which will be discussed later.

### Operators

MAGES comes with a similar set of operators as known from C. Besides the usual arithmetic operators (e.g., `+`, `-`, `*`, `/`) MAGES also introduces some other operators. Here we have the factorial operator `!`, the transpose operator `'`, and the reverse division operator `\`. The latter allows writing `2 \ 1` instead of `1 / 2`. This may become handy for matrices.

Some operators are different than in C. For instance, the negate operator is not `!` (that is the factorial operator, which is post-unary and not pre-unary), but the tilde `~`. Hence `~false` is `true`. Consequently, the not equals operator is given by writing `~=` instead of `!=`. Furthermore, there are no bitwise operators in MAGES. Instead, the `&` operator is a pre-unary operator to return the type and the `|` operator is a binary operator to call a function on the right with the argument on the left.

### Index Accessors

Matrices are 0-based. Therefore, both, row and column index, will be set starting at zero. Again, the type function syntax is used, which offers a one- and two-argument syntax. Let's say we defined a `5x3` matrix via `M = rand(5, 3)` we can use:

```C
M(0, 0) // first element
M(4, 2) // last element
M(6) // gets the 7th element; M(2, 0)
M(3, 1) // gets the second column of the fourth row
```

Besides the getter there is also a setter. It is invoked by using the type function on the left hand side of an assignment expression.

## Range

Matrices are perfectly additioned by ranges. A range is precomputed, i.e., it is a real matrix without lazy loading. Very large ranges therefore occupy also a lot of memory.

Ranges come in two versions. There is an implicit and an explicit range. The former requires only 2 arguments, the latter three.

### Literals

The operator to invoke the range literal is the colon `:`. It can be used with two such colons (explicit: denoting start, step, end) or just one (implicit: separating start and end). Start and end are both given inclusive.

```C
1:1:5 // Creates a 1x5 matrix [1,2,3,4,5]
1:2:5 // Creates a 1x3 matrix [1,3,5]
1:-1:-1 // Creates a 1x3 matrix [1,0,-1]
1:4 // Creates a 1x4 matrix [1,2,3,4]
1:-1 // Creates a 1x3 matrix [1,0,-1]
```

By default this creates a row vector, however, one can always transpose the result, e.g., `(1:3)'`.

## Functions

(tbd)

### Literals

(tbd)

### Arbitrary Arguments

(tbd)

### Auto Currying

(tbd)