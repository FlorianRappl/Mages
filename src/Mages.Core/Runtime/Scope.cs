namespace Mages.Core.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Scope:IEnumerable
    {
        protected readonly IDictionary<String, Object> _scope;
        protected readonly Scope _parent;

        public Scope(Scope parent)
        {
            _scope = new Dictionary<string, object>();
            _parent = parent;
        }

        public Scope Parent
        {
            get { return _parent; }
        }

        public virtual object this[string key]
        {
            get
            {
                object value;
                if (_scope.TryGetValue(key, out value))
                    return value;

                return _parent[key];
            }
            set
            {
                if (_scope.ContainsKey(key))
                {
                    _scope[key] = value;
                    return;
                }
                _parent[key] = value;
            }
        }

        public virtual Boolean TryGetValue(String key, out Object value)
        {
            return _scope.TryGetValue(key, out value) || _parent.TryGetValue(key, out value);
        }


        public Int32 Count
        {
            get { return _scope.Count; }
        }

        public Boolean IsReadOnly
        {
            get { return false; }
        }

        public ICollection<String> Keys
        {
            get { return _scope.Keys; }
        }

        public ICollection<Object> Values
        {
            get { return _scope.Values; }
        }

        public void Add(KeyValuePair<String, Object> item)
        {
            _scope.Add(item.Key, item.Value);
        }

        public void Add(String key, Object value)
        {
            _scope.Add(key, value);
        }

        public void Clear()
        {
            _scope.Clear();
        }

        public Boolean Contains(KeyValuePair<String, Object> item)
        {
            return _scope.ContainsKey(item.Key);
        }

        public Boolean ContainsKey(String key)
        {
            return _scope.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<String, Object>[] array, Int32 arrayIndex)
        {
        }

        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            return _scope.GetEnumerator();
        }

        public Boolean Remove(KeyValuePair<String, Object> item)
        {
            return _scope.Remove(item.Key);
        }

        public Boolean Remove(String key)
        {
            return _scope.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _scope.GetEnumerator();
        }
    }
}
