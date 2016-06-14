namespace Mages.Core.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class ObservableDictionary : IDictionary<String, Object>
    {
        private readonly IDictionary<String, Object> _store;

        public event EventHandler<EntryChangedArgs> Changed;

        public ObservableDictionary(IDictionary<String, Object> store)
        {
            _store = store;
        }

        Object IDictionary<String, Object>.this[String key]
        {
            get { return _store[key]; }
            set
            {
                var existing = default(Object);
                _store.TryGetValue(key, out existing);
                _store[key] = value;
                Emit(key, existing, value);
            }
        }

        Int32 ICollection<KeyValuePair<String, Object>>.Count
        {
            get { return _store.Count; }
        }

        Boolean ICollection<KeyValuePair<String, Object>>.IsReadOnly
        {
            get { return _store.IsReadOnly; }
        }

        ICollection<String> IDictionary<String, Object>.Keys
        {
            get { return _store.Keys; }
        }

        ICollection<Object> IDictionary<String, Object>.Values
        {
            get { return _store.Values; }
        }

        void ICollection<KeyValuePair<String, Object>>.Add(KeyValuePair<String, Object> item)
        {
            _store.Add(item);
            Emit(item.Key, null, item.Value);
        }

        void IDictionary<String, Object>.Add(String key, Object value)
        {
            _store.Add(key, value);
            Emit(key, null, value);
        }

        void ICollection<KeyValuePair<String, Object>>.Clear()
        {
            var items = _store.ToArray();
            
            foreach (var item in items)
            {
                _store.Remove(item);
                Emit(item.Key, item.Value, null);
            }
        }

        Boolean ICollection<KeyValuePair<String, Object>>.Contains(KeyValuePair<String, Object> item)
        {
            return _store.Contains(item);
        }

        Boolean IDictionary<String, Object>.ContainsKey(String key)
        {
            return _store.ContainsKey(key);
        }

        void ICollection<KeyValuePair<String, Object>>.CopyTo(KeyValuePair<String, Object>[] array, Int32 arrayIndex)
        {
            _store.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _store.GetEnumerator();
        }

        IEnumerator<KeyValuePair<String, Object>> IEnumerable<KeyValuePair<String, Object>>.GetEnumerator()
        {
            return _store.GetEnumerator();
        }

        Boolean ICollection<KeyValuePair<String, Object>>.Remove(KeyValuePair<String, Object> item)
        {
            if (_store.Contains(item))
            {
                _store.Remove(item);
                Emit(item.Key, item.Value, null);
                return true;
            }

            return false;
        }

        Boolean IDictionary<String, Object>.Remove(String key)
        {
            var existing = default(Object);

            if (_store.TryGetValue(key, out existing))
            {
                _store.Remove(key);
                Emit(key, existing, null);
                return true;
            }

            return false;
        }

        Boolean IDictionary<String, Object>.TryGetValue(String key, out Object value)
        {
            return _store.TryGetValue(key, out value);
        }

        protected virtual void OnChanged(String key, Object oldValue, Object newValue)
        {
        }

        private void Emit(String key, Object oldValue, Object newValue)
        {
            var handler = Changed;

            if (handler != null)
            {
                handler.Invoke(this, new EntryChangedArgs(key, oldValue, newValue));
            }

            OnChanged(key, oldValue, newValue);
        }
    }
}
