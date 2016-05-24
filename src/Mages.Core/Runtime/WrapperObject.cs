namespace Mages.Core.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    sealed class WrapperObject : IDictionary<String, Object>
    {
        private readonly Object _obj;
        private readonly Dictionary<String, Object> _shadow;

        public WrapperObject(Object obj)
        {
            _obj = obj;
            _shadow = new Dictionary<String, Object>();
        }

        public Object this[String key]
        {
            get
            {
                var result = default(Object);
                TryGetValue(key, out result);
                return result;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Int32 Count
        {
            get { return _shadow.Count; }
        }

        public Boolean IsReadOnly
        {
            get { return false; }
        }

        public ICollection<String> Keys
        {
            get { return _shadow.Keys; }
        }

        public ICollection<Object> Values
        {
            get { return _shadow.Values; }
        }

        public void Add(KeyValuePair<String, Object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(String key, Object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _shadow.Clear();
        }

        public Boolean Contains(KeyValuePair<String, Object> item)
        {
            return ContainsKey(item.Key);
        }

        public Boolean ContainsKey(String key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<String, Object>[] array, Int32 arrayIndex)
        {
        }

        public IEnumerator<KeyValuePair<String, Object>> GetEnumerator()
        {
            return _shadow.GetEnumerator();
        }

        public Boolean Remove(KeyValuePair<String, Object> item)
        {
            return Remove(item.Key);
        }

        public Boolean Remove(String key)
        {
            return _shadow.Remove(key);
        }

        public Boolean TryGetValue(String key, out Object value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
