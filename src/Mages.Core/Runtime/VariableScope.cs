namespace Mages.Core.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    sealed class VariableScope : IDictionary<String, Object>
    {
        private readonly Dictionary<String, Object> _scope;
        private readonly IDictionary<String, Object> _parent;

        public VariableScope(IDictionary<String, Object> parent)
        {
            _scope = new Dictionary<String, Object>();
            _parent = parent;
        }

        public IDictionary<String, Object> Parent
        {
            get { return _parent; }
        }

        public object this[String key]
        {
            get { return _scope[key]; }
            set { _scope[key] = value; }
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

        public Boolean TryGetValue(String key, out Object value)
        {
            return _scope.TryGetValue(key, out value) || _parent.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _scope.GetEnumerator();
        }
    }
}
