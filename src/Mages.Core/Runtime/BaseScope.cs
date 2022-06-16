namespace Mages.Core.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    abstract class BaseScope : IDictionary<String, Object>
    {
        protected readonly IDictionary<String, Object> _scope;
        protected readonly IDictionary<String, Object> _parent;

        public BaseScope(IDictionary<String, Object> scope, IDictionary<String, Object> parent)
        {
            _scope = scope;
            _parent = parent;
        }

        public IDictionary<String, Object> Parent => _parent;

        /// <summary>
        /// Gets or sets a variable in the scope. Keep in mind that only values should be
        /// set that are compatible with the type system of Mages. For instance, setting
        /// an integer would be an error as only Double and Complex are used.
        /// </summary>
        public Object this[String key]
        {
            get => _scope[key];
            set => SetValue(key, value);
        }

        /// <summary>
        /// Gets the number number of variables in the scope.
        /// </summary>
        public Int32 Count => _scope.Count;

        /// <summary>
        /// Gets the state if the scope is readonly. It is not.
        /// </summary>
        public Boolean IsReadOnly => false;

        public ICollection<String> Keys => _scope.Keys;

        public ICollection<Object> Values => _scope.Values;

        public void Add(KeyValuePair<String, Object> item) => _scope.Add(item.Key, item.Value);

        /// <summary>
        /// Add a value to the scope. Keep in mind that further runtime checks will not be performed.
        /// The value must be one of the allowed types in order to be correctly integrated.
        /// For instance, adding an integer will almost always lead to errors as only Complex and Double
        /// values are supported. Add wisely!
        /// </summary>
        /// <param name="key">The name of the variable / value to refer to.</param>
        /// <param name="value">The value to store.</param>
        public void Add(String key, Object value) => _scope.Add(key, value);

        /// <summary>
        /// Clears the scope.
        /// </summary>
        public void Clear() => _scope.Clear();

        public Boolean Contains(KeyValuePair<String, Object> item) => _scope.ContainsKey(item.Key);

        /// <summary>
        /// Checks if a reference is included in the scope.
        /// </summary>
        /// <param name="key">The name of the variable / value to refer to.</param>
        /// <returns>True if the variable is known, otherwise false.</returns>
        public Boolean ContainsKey(String key) => _scope.ContainsKey(key);

        public void CopyTo(KeyValuePair<String, Object>[] array, Int32 arrayIndex)
        {
        }

        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator() => _scope.GetEnumerator();

        public Boolean Remove(KeyValuePair<String, Object> item) => _scope.Remove(item.Key);

        public Boolean Remove(String key) => _scope.Remove(key);

        public Boolean TryGetValue(String key, out Object value) => _scope.TryGetValue(key, out value) || _parent.TryGetValue(key, out value);

        protected abstract void SetValue(String key, Object value);

        IEnumerator IEnumerable.GetEnumerator() => _scope.GetEnumerator();
    }
}
