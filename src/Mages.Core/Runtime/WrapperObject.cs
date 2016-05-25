namespace Mages.Core.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    sealed class WrapperObject : IDictionary<String, Object>
    {
        private readonly Object _content;
        private readonly Dictionary<String, Object> _extend;
        private readonly Dictionary<String, BaseProxy> _proxy;

        public WrapperObject(Object content)
        {
            _content = content;
            _extend = new Dictionary<String, Object>();
            _proxy = new Dictionary<String, BaseProxy>();
            GenerateProxies();
        }

        public Object Content
        {
            get { return _content; }
        }

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

        public Int32 Count
        {
            get { return _extend.Count + _proxy.Count; }
        }

        public Boolean IsReadOnly
        {
            get { return false; }
        }

        public ICollection<String> Keys
        {
            get { return _extend.Keys; }
        }

        public ICollection<Object> Values
        {
            get { return _extend.Values; }
        }

        public void Add(KeyValuePair<String, Object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(String key, Object value)
        {
            TrySetValue(key, value);
        }

        public void Clear()
        {
            _extend.Clear();
        }

        public Boolean Contains(KeyValuePair<String, Object> item)
        {
            return ContainsKey(item.Key);
        }

        public Boolean ContainsKey(String key)
        {
            return _proxy.ContainsKey(key) || _extend.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<String, Object>[] array, Int32 arrayIndex)
        {
        }

        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            return _extend.GetEnumerator();
        }

        public Boolean Remove(KeyValuePair<String, Object> item)
        {
            return Remove(item.Key);
        }

        public Boolean Remove(String key)
        {
            return _extend.Remove(key);
        }

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

                return Convert(value, value.GetType().Narrow());
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
                var method = Type.DefaultBinder.SelectMethod(flags, _methods, parameters, null);
                var result = default(Object);

                if (method != null)
                {
                    result = method.Invoke(target, arguments);
                }

                if (Object.ReferenceEquals(result, target))
                {
                    return _obj;
                }

                return result;
            }
        }
    }
}
