namespace Mages.Core.Runtime
{
    using Mages.Core.Runtime.Converters;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents the object wrapper from MAGES.
    /// </summary>
    public sealed class WrapperObject : IDictionary<String, Object>
    {
        #region Fields

        private readonly Object _content;
        private readonly Dictionary<String, Object> _extend;
        private readonly Dictionary<String, BaseProxy> _proxy;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new wrapped object.
        /// </summary>
        /// <param name="content">The object to wrap.</param>
        public WrapperObject(Object content)
        {
            _content = content;
            _extend = new Dictionary<String, Object>();
            _proxy = new Dictionary<String, BaseProxy>();
            GenerateProxies();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the wrapped object (content).
        /// </summary>
        public Object Content
        {
            get { return _content; }
        }

        #endregion

        #region IDictionary Implementation

        /// <summary>
        /// Gets or sets the value of the underlying object or
        /// the extension object.
        /// </summary>
        /// <param name="key">The name of the property.</param>
        /// <returns>The value of the property.</returns>
        public Object this[String key]
        {
            get
            {
                var result = default(Object);
                TryGetValue(key, out result);
                return result;
            }
            set { TrySetValue(key, value); }
        }

        /// <summary>
        /// Gets the number of properties of the underlying
        /// object and the extension object.
        /// </summary>
        public Int32 Count
        {
            get { return _extend.Count + _proxy.Count; }
        }

        Boolean ICollection<KeyValuePair<String, Object>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets all the keys from the extension object.
        /// </summary>
        public ICollection<String> Keys
        {
            get { return _extend.Keys; }
        }

        /// <summary>
        /// Gets all the values from the extension object.
        /// </summary>
        public ICollection<Object> Values
        {
            get { return _extend.Values; }
        }

        void ICollection<KeyValuePair<String, Object>>.Add(KeyValuePair<String, Object> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Sets the provided value at the provided property.
        /// </summary>
        /// <param name="key">The name of the property.</param>
        /// <param name="value">The value to use.</param>
        public void Add(String key, Object value)
        {
            TrySetValue(key, value);
        }

        /// <summary>
        /// Resets the extension object.
        /// </summary>
        public void Clear()
        {
            _extend.Clear();
        }

        /// <summary>
        /// Checks if the underlying object or the extension
        /// object contains the given key.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        /// <returns>True if the key is used, otherwise false.</returns>
        public Boolean Contains(KeyValuePair<String, Object> item)
        {
            return ContainsKey(item.Key);
        }

        /// <summary>
        /// Checks if the underlying object or the extension
        /// object contains the given key.
        /// </summary>
        /// <param name="key">The key to check for.</param>
        /// <returns>True if the key is used, otherwise false.</returns>
        public Boolean ContainsKey(String key)
        {
            return _proxy.ContainsKey(key) || _extend.ContainsKey(key);
        }

        void ICollection<KeyValuePair<String, Object>>.CopyTo(KeyValuePair<String, Object>[] array, Int32 arrayIndex)
        {
        }

        /// <summary>
        /// Gets the enumerator over the elements of the extension.
        /// </summary>
        /// <returns>The extension's enumerator.</returns>
        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            return _extend.GetEnumerator();
        }

        Boolean ICollection<KeyValuePair<String, Object>>.Remove(KeyValuePair<String, Object> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Removes the item from the extension.
        /// </summary>
        /// <param name="key">The key of the item to be removed.</param>
        /// <returns>True if it could be removed, otherwise false.</returns>
        public Boolean Remove(String key)
        {
            return _extend.Remove(key);
        }

        /// <summary>
        /// Tries to get the value from the given key.
        /// </summary>
        /// <param name="key">The name of the property.</param>
        /// <param name="value">The resulting value.</param>
        /// <returns>True if the value could be retrieved, otherwise false.</returns>
        public Boolean TryGetValue(String key, out Object value)
        {
            var proxy = default(BaseProxy);

            if (_proxy.TryGetValue(key, out proxy))
            {
                value = proxy.Value;
                return true;
            }

            return _extend.TryGetValue(key, out value);
        }

        #endregion

        #region Helpers

        private void TrySetValue(String key, Object value)
        {
            var proxy = default(BaseProxy);
            
            if (_proxy.TryGetValue(key, out proxy))
            {
                proxy.Value = value;
            }
            else
            {
                _extend[key] = value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void GenerateProxies()
        {
            var type = _content.GetType();
            var fields = type.GetFields();
            var properties = type.GetProperties();
            var methods = type.GetMethods();

            foreach (var field in fields)
            {
                _proxy.Add(field.Name, new FieldProxy(this, field));
            }

            foreach (var property in properties)
            {
                if (property.GetIndexParameters().Length == 0)
                {
                    _proxy.Add(property.Name, new PropertyProxy(this, property));
                }
            }

            foreach (var method in methods.Where(m => !m.IsSpecialName).GroupBy(m => m.Name))
            {
                var overloads = method.ToArray();
                _proxy.Add(method.Key, new MethodProxy(this, overloads));
            }
        }

        #endregion

        #region Proxy classes

        abstract class BaseProxy
        {
            protected readonly WrapperObject _obj;

            public BaseProxy(WrapperObject obj)
            {
                _obj = obj;
            }

            public Object Value
            {
                get { return Convert(GetValue()); }
                set { SetValue(value); }
            }

            protected abstract void SetValue(Object value);

            protected abstract Object GetValue();

            protected Object Convert(Object value, Type target)
            {
                var source = value.GetType();
                var converter = Helpers.Converters.FindConverter(source, target);
                return converter.Invoke(value);
            }

            private Object Convert(Object value)
            {
                if (Object.ReferenceEquals(value, _obj.Content))
                {
                    return _obj;
                }

                return Convert(value, value.GetType().FindPrimitive());
            }
        }

        sealed class FieldProxy : BaseProxy
        {
            private readonly FieldInfo _field;

            public FieldProxy(WrapperObject obj, FieldInfo field)
                : base(obj)
            {
                _field = field;
            }

            protected override Object GetValue()
            {
                var target = _obj.Content;
                return _field.GetValue(target);
            }

            protected override void SetValue(Object value)
            {
                var target = _obj.Content;
                var result = Convert(value, _field.FieldType);
                _field.SetValue(target, result);
            }
        }

        sealed class PropertyProxy : BaseProxy
        {
            private readonly PropertyInfo _property;

            public PropertyProxy(WrapperObject obj, PropertyInfo property)
                : base(obj)
            {
                _property = property;
            }

            protected override Object GetValue()
            {
                if (_property.CanRead)
                {
                    var target = _obj.Content;
                    return _property.GetValue(target, null);
                }

                return null;
            }

            protected override void SetValue(Object value)
            {
                if (_property.CanWrite)
                {
                    var target = _obj.Content;
                    var result = Convert(value, _property.PropertyType);
                    _property.SetValue(target, result, null);
                }
            }
        }

        sealed class MethodProxy : BaseProxy
        {
            private readonly MethodInfo[] _methods;
            private readonly Function _proxy;

            public MethodProxy(WrapperObject obj, MethodInfo[] methods)
                : base(obj)
            {
                _methods = methods;
                _proxy = new Function(Invoke);
            }

            protected override Object GetValue()
            {
                return _proxy;
            }

            protected override void SetValue(Object value)
            {
            }

            private Object Invoke(Object[] arguments)
            {
                var target = _obj.Content;
                var parameters = arguments.Select(m => m.GetType()).ToArray();
                var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.OptionalParamBinding | BindingFlags.InvokeMethod;
                var method = Type.DefaultBinder.SelectMethod(flags, _methods, parameters, null) ?? _methods.Find(arguments, parameters);
                var result = default(Object);

                if (method != null)
                {
                    result = method.Invoke(target, arguments);
                }

                if (Object.ReferenceEquals(result, target))
                {
                    return _obj;
                }

                return Helpers.WrapObject(result);
            }

        }

        #endregion
    }
}
