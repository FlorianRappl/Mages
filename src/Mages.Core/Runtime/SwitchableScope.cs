namespace Mages.Core.Runtime;

using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Helper class to create switchable scopes.
/// </summary>
public class SwitchableScope : IDictionary<String, Object>
{
    private const String DEFAULT = "default";
    private IDictionary<String, Object> _current;
    private readonly Dictionary<String, IDictionary<String, Object>> _scopes;

    /// <summary>
    /// Creates a new switchable scope equipped with an empty default scope.
    /// </summary>
    public SwitchableScope() : this(null)
    {
    }

    /// <summary>
    /// Creates a new switchable scope equipped with the provided scope.
    /// </summary>
    /// <param name="defaultScope">The scope.</param>
    public SwitchableScope(IDictionary<String, Object> defaultScope)
    {
        _scopes = new Dictionary<String, IDictionary<String, Object>>
        {
            { DEFAULT, defaultScope ?? new Dictionary<String, Object>() }
        };
        ChangeScope(DEFAULT);
    }

    /// <summary>
    /// Gets the currently selected scope.
    /// </summary>
    public IDictionary<String, Object> Current => _current;

    /// <summary>
    /// Gets the available scope names.
    /// </summary>
    public IEnumerable<String> Names => _scopes.Keys;

    ICollection<string> IDictionary<string, object>.Keys => throw new NotImplementedException();

    ICollection<object> IDictionary<string, object>.Values => throw new NotImplementedException();

    int ICollection<KeyValuePair<string, object>>.Count => throw new NotImplementedException();

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => throw new NotImplementedException();

    object IDictionary<string, object>.this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    /// <summary>
    /// Changes the currently selected scope to the provided name.
    /// Falls back to the default scope if nothing is found.
    /// </summary>
    /// <param name="name">The name of the scope to switch to.</param>
    public void ChangeScope(String name)
    {
        if (_scopes.TryGetValue(name, out var value))
        {
            _current = value;
        }
        else
        {
            ChangeScope(DEFAULT);
        }
    }

    /// <summary>
    /// Adds a new scope with the provided name. The name has to be available.
    /// </summary>
    /// <param name="name">The name of the new scope.</param>
    /// <param name="scope">The values of the scope, otherwise a new value container is created.</param>
    public void AddScope(String name, IDictionary<String, Object> scope = null)
    {
        if (!_scopes.ContainsKey(name))
        {
            scope ??= new Dictionary<String, Object>();
            _scopes.Add(name, scope);
        }
    }

    /// <summary>
    /// Removes the available scope by name. The name cannot be the default scope.
    /// </summary>
    /// <param name="name">The name of the scope to remove.</param>
    /// <returns>The values of the removed scope in case a scope was removed.</returns>
    public IDictionary<String, Object> RemoveScope(String name)
    {
        if (name != DEFAULT && _scopes.TryGetValue(name, out var scope))
        {
            _scopes.Remove(name);

            if (scope == _current)
            {
                ChangeScope(DEFAULT);
            }

            return scope;
        }

        return null;
    }

    void IDictionary<string, object>.Add(string key, object value)
    {
        _current.Add(key, value);
    }

    bool IDictionary<string, object>.ContainsKey(string key)
    {
        return _current.ContainsKey(key);
    }

    bool IDictionary<string, object>.Remove(string key)
    {
        return _current.Remove(key);
    }

    bool IDictionary<string, object>.TryGetValue(string key, out object value)
    {
        return _current.TryGetValue(key, out value);
    }

    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
    {
        _current.Add(item);
    }

    void ICollection<KeyValuePair<string, object>>.Clear()
    {
        _current.Clear();
    }

    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
    {
        return _current.Contains(item);
    }

    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        _current.CopyTo(array, arrayIndex);
    }

    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
    {
        return _current.Remove(item);
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
    {
        return _current.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _current.GetEnumerator();
    }
}
