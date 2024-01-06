# Advanced Scenarios

## Switchable Scopes

You might want to control the scope of a single engine. The `Scope` property of the `Engine` is fixed, but you can use the `SwitchableScope` class of the `Mages.Core.Runtime` namespace to achieve this:

```cs
var scope = new SwitchableScope();
var eng = new Engine(new Configuration
{
    Scope = scope,
});
```

The `SwitchableScope` starts with a default scope (that you could also configure / provide when you construct a `SwitchableScope`). Interacting with the currently selected scope works via the `Current` property, e.g.,

```cs
scope.Current.Add("foo", 4.0);
```

will add a new variable called `foo` to the scope. Be aware that this variable *should* be of a MAGES type, e.g., `Double` or `Complex`, but not (for instance) `Int32`.

You can also add new scopes to it via the `AddScope` method such as

```cs
scope.AddScope("alt");
```

Switching between the available scopes now works such as

```cs
// to the scope "alt"
scope.ChangeScope("alt");
// back to the default scope
scope.ChangeScope("default");
```

If a provided scope name is not found the `default` scope will be selected.
