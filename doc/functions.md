# Included Functions

MAGES offers the ability to easily expose an API via classes or single functions. However, a set of basic functions is already included in the library. The set of basic functions consists of the following signatures.

## Arithmetic Functions

### Square Root of Values

Works with numbers and matrices (applied to each value). Equivalent to the power operator (`^`) with `1/2`.

Examples:

```
x = sqrt(2) // 1.41...
M = sqrt([1, 2]) // [1,1.41...]
```

### Powers of Values

Works with numbers and matrices (applied to each value). Equivalent to the power operator (`^`). If two matrices are supplied the dimensions must match.

Examples:

```
x = pow(2, 3) // 8
M = pow(2, [1, 2, 3]) // [2, 4, 8]
M = pow([1, 2, 3], [1, 2, 3]) // [1, 4, 27]
```

### Factorial of Values

Works with numbers and matrices (applied to each value). Equivalent to the factorial operator (`!`).

```
x = factorial(5) // 120
M = factorial([1, 2, 3]) // [1, 2, 6]
```

### Absolute Value

Works with numbers and matrices (applied to the whole matrix, where the result is essentially the square root of the sum of all numbers squared).

```
x = abs(-5) // 5
M = abs([-1, 0, 1]) // sqrt(2)
```

### Sign of Values

Works with numbers and matrices (applied to each value).

```
x = sign(-5) // -1
M = sign([-3, 0, 2]) // [-1, 0, 1]
```

### Ceiling Values

Works with numbers and matrices (applied to each value).

```
x = ceil(-1.1) // -1
M = ceil([-2.1, 2.1]) // [-2, 3]
```

### Floor Values

Works with numbers and matrices (applied to each value).

```
x = floor(-1.1) // -2
M = floor([-2.1, 2.1]) // [-3, 2]
```

### Rounding Values

Works with numbers and matrices (applied to each value).

```
x = round(1.4) // 1
M = round([-2.1, 2.1]) // [-2, 2]
```

### Exponential Function

Works with numbers and matrices (applied to each value).

```
x = exp(1) // 2.72...
M = exp([-1, 0]) // [0.36..., 1]
```

### Logarithmic Function

Works with numbers and matrices (applied to each value).

```
x = log(1) // 0
M = log([0.5, 2]) // [-0.69..., 0.69...]
```

## Comparison Functions

### Minimum of Values

Works with numbers and matrices (performs a reduction).

```
x = min(1, 2) // 1
M = min([0.5, 2, -1, 10, -0.5]) // -1
```

### Maximum of Values

Works with numbers and matrices (performs a reduction).

```
x = max(1, 2) // 2
M = max([0.5, 2, -1, 10, -0.5]) // 10
```

### Less Than

Works with numbers and matrices (applied to each value). Equivalent to the less-than operator (`<`). If two matrices are supplied the dimensions must match.

```
x = lt(2, 3) // 1
M = lt(2, [1, 2, 3]) // [0, 0, 1]
M = lt([1, 2, 3], [2, 0, 3]) // [1, 0, 0]
```

### Equals

Works with numbers and matrices (applied to each value). Equivalent to the equals operator (`==`). If two matrices are supplied the dimensions must match.

```
x = eq(2, 3) // 0
M = eq(2, [1, 2, 3]) // [0, 1, 0]
M = eq([1, 2, 3], [2, 0, 3]) // [0, 0, 1]
```

### Greater Than

Works with numbers and matrices (applied to each value). Equivalent to the greater-than operator (`>`). If two matrices are supplied the dimensions must match.

```
x = gt(2, 3) // 0
M = gt(2, [1, 2, 3]) // [1, 0, 0]
M = gt([1, 2, 3], [2, 0, 3]) // [0, 1, 0]
```

## Trigonometric Functions

### Sine

Works with numbers and matrices (applied to each value).

```
x = sin(pi) // 0
M = sin([0, pi/4, pi/2, pi]) // [0, 0.70..., 1, 0]
```

### Cosine

Works with numbers and matrices (applied to each value).

```
x = cos(pi) // -1
M = cos([0, pi/4, pi/2, pi]) // [1, 0.70..., 0, -1]
```

### Tangent

Works with numbers and matrices (applied to each value).

```
x = tan(pi) // 0
M = tan([0, pi/4, pi]) // [0, 1, 0]
```

### Arcsine

Works with numbers and matrices (applied to each value).

```
x = arcsin(0) // 0
M = arcsin([0, 1/sqrt(2), 1]) // [0, 0.78..., 1.57...]
```

### Arcosine

Works with numbers and matrices (applied to each value).

```
x = arccos(1) // 0
M = arccos([0, 1/sqrt(2), 1]) // [1.57..., 0.78..., 0]
```

### Artangent

Works with numbers and matrices (applied to each value).

```
x = arctan(1) // 0.78...
M = arctan([0, 1/sqrt(2), 1]) // [0, 0.61..., 0.78...]
```

## Logical Functions

### Check if Not a Number

Works with numbers and matrices (applied to each value).

```
x = isnan(sqrt(-1)) // 1
M = isnan([0, 1, 1/0]) // [0, 0, 0]
```

### Check if Integer

Works with numbers and matrices (applied to each value).

```
x = isint(2.3) // 0
M = isint([0, ln(1), sqrt(2)]) // [1, 1, 0]
```

### Check if Prime

Works with numbers and matrices (applied to each value).

```
x = isprime(23) // 1
M = isprime([2, 17, 49]) // [1, 1, 0]
```

### Check if Infinity

Works with numbers and matrices (applied to each value).

```
x = isinfty(1/0) // 1
M = isinfty([0, 1, -1/0]) // [0, 0, 1]
```

## RNG Functions

### Generate Single Random Number

Works without any arguments.

```
x = rand() // any number between 0 and 1
```

### Generate Random Vector

Works with one argument.

```
v = rand(3) // a 1x3 matrix with numbers between 0 and 1
```

### Generate Random Matrix

Works with two arguments.

```
M = rand(3, 2) // a 3x2 matrix with numbers between 0 and 1
```

## General Functions

### Length Determination

Works with one argument, which could be anything.

```
x = length(true) // 1
x = length("hello") // 5
x = length(2.3) // 1
x = length(new { a: 2, b: 3 }) // 2
x = length([1, 2; 3, 4; 7, 0]) // 6
```

### Throwing Exceptions

Works with one argument (a `string` providing an error message).

```
throw("Not implemented yet!");
```

### Catching Exceptions

Works with one argument (a function being called without arguments).

```
f = () => throw("Error!");
ans = catch(f);
```

The resulting object has two properties `value` and `error`. The former is used for the result of the operation (if successful), the latter is used for the exception message (if one occurred). Both are `undefined` (`null`) initially.

### Evaluating Expressions

The `Engine` instance itself can be used during evaluation (if not deactivated). Works with one `string` argument.

```
x = eval("2 + 3") // 5
M = eval("[1,2,3] / 2") // [0.5, 1.0, 1.5]
```