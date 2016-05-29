# Types

MAGES maps every .NET data type to one of its data types:

* Number (`System.Double`)
* Boolean (`System.Boolean`)
* String (`System.String`)
* Matrix (`System.Double[,]`)
* Object (`System.Collections.Generic.IDictionary<System.String, System.Object>`)
* Function (`Mages.Core.Function`, essentially a `Delegate` mapping `Object[]` to `Object`)
* Nothing (`null`)

These seven kinds of types spawn the whole set of capabilities offered by MAGES. For instance, even if we use a function like `catch` (see [Functions](functions.md)) the result will be an `IDictionary<String, Object>` instance. There is no special type returned.

## Type Mapping

The following table yields the essential type mapping.

| MAGES    | .NET                        |
|--------- | --------------------------- |
| Number   | Double                      |
|          | Single                      |
|          | Decimal                     |
|          | Byte                        |
|          | UInt16                      |
|          | UInt32                      |
|          | UInt64                      |
|          | Int16                       |
|          | Int32                       |
|          | Int64                       |
| Boolean  | Boolean                     |
| String   | String                      |
|          | Char                        |
| Matrix   | Double[,]                   |
|          | Double[]                    |
|          | List<Double>                |
| Function | Function                    |
|          | Delegate                    |
| Object   | IDictionary<String, Object> |
|          | Object                      |
| Nothing  | `null`                      |

The table on described the direct relations. Furthermore, there are some additional relations.

## Type Casting

MAGES allows casting between its own types. For instance, any number can be casted to a boolean with the negate operator `~`.

```C
~~1.5 // true
~~0 // false
```

There are also other occasions where numbers are transformed to booleans. Similarly, strings transform to boolean.

```C
~~"test" // true
~~"" // false
```

Therefore any non-empty string represents `true`. Empty strings are interpreted as `false`.

Objects are `false` if they are empty. So we show the following examples:

```C
~~new { a: 0 } // true
~~new { } // false
```

Functions are always `true`. Nothing is always `false`. The big exception to all the shown identities are matrices. Here the not operator only transforms the matrix to a logical matrix (only consisting of zeros and ones, but still equivalent to a .NET `double[,]`). This is time to introduce to useful logical helper functions, `all` and `any`. Both of them transform any non-matrix value as booleans - as seen previously. However, for matrices they do not stop at logical matrices, but go all the way to booleans.

```C
any([0, 0, 1]) // true
all([0, 0, 1]) // false
any([0, 0, 0]) // false
all([1, 1, 1]) // true
```

`any` returns `true` once *any* non-zero value is found in the matrix. `all` returns `false` once *any* zero value is found in the matrix.

The following tables yield information on casting possibilities.

| From `x` | To `y`   | Code                  |
| -------- | -------- | --------------------- |
| Number   | Boolean  | `y = any(x)`          |
|          | String   | `y = stringify(x)`    |
|          | Matrix   | `y = x'`              |
| Boolean  | Number   | `y = +x`              |
|          | String   | `y = stringify(x)`    |
|          | Matrix   | `y = x'`              |
| String   | Number   | `y = +x`              |
|          | Boolean  | `y = any(x)`          |
|          | Matrix   | `y = x'`              |
| Matrix   | Number   | (nothing)             |
|          | Boolean  | `y = any(x)`          |
|          | String   | `y = stringify(x)`    |
| Object   | Number   | `y = +x // NaN`       |
|          | Boolean  | `y = any(x)`          |
|          | String   | `y = stringify(x)`    |
|          | Matrix   | `y = x' // [NaN]`     |
| Function | Number   | `y = +x // NaN`       |
|          | Boolean  | `y = any(x) // true`  |
|          | String   | `y = stringify(x)`    |
|          | Matrix   | `y = x' // NaN`       |
| Nothing  | Boolean  | `y = any(x) // false` |
|          | String   | `y = stringify(x)`    |
|          | Matrix   | `y = x' // [NaN]`     |

Things that have not been mentioned, e.g., casting from number to function are not directly possible, but can be done trivially. For instance, a number `x` can be brought into a function `y` by writing `y = () => x`. Similarly, everything can be converted to an object. Here we just need to know what we call the key. If we go with the name `value` then we can use `y = new { value: x }` to transform any value to an object.

For transforming a matrix into a number multiple ways are possible. If the cast is required internally then the first value is picked if there is just a single value. Otherwise `NaN` is taken due to ambiguity. Explicitly, however, there are multiply options, e.g., picking a specific element, performing a special operation such as `det` or `trace`, or using a self-defined way.