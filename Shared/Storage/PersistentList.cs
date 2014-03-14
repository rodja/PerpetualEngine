using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace PerpetualEngine.Storage
{
    public class PersistentList<T> : IEnumerable, IEnumerable<T>
    {
        SimpleStorage storage;
        const string idListKey = "ids";
        List<string> ids;
        List<T> items = new List<T>();

        public PersistentList(string editGroup)
        {
            storage = SimpleStorage.EditGroup(editGroup);
            ids = storage.Get<List<string>>(idListKey) ?? new List<string>();

            var obsoleteIds = new List<string>();
            foreach (var i in ids) {
                var item = storage.Get<T>(i);
                if (item == null) {
                    obsoleteIds.Add(i);
                    continue;
                }
                items.Add(item);
            }

            if (obsoleteIds.Count > 0) {
                foreach (var id in obsoleteIds) {
                    storage.Delete(id);
                    ids.Remove(id);
                    storage.Put(idListKey, ids);
                }
            }
        }

        public event Action<T> Added = delegate {};
        public event Action<T> Removed = delegate {};
        public event Action<T> Updated = delegate {};

        public int Count { get { return items.Count; } }

        public bool IsEmpty { 
            get { 
                return items.IsEmpty(); 
            } 
        }

        public virtual void Add(string id, T value)
        {
            if (id == idListKey)
                throw new ApplicationException("The id must not be \"" + idListKey + "\".");
            if (ids.Contains(id))
                throw new ApplicationException("Object with id \"" + id + "\" already exists.");
            storage.Put(id, value);
            ids.Add(id);
            storage.Put(idListKey, ids);
            items.Add(value);

            Added(value);
        }

        public virtual void Add(T value)
        {
            if (!(value is IIdentifiable))
                throw new NotSupportedException("Value must implement IIdentifiable");
            Add((value as IIdentifiable).Id, value);
        }

        /// <summary>
        /// Updates item with specified Id.
        /// </summary>
        public virtual void Update(string id)
        {
            if (!ids.Contains(id))
                throw new ApplicationException("Object with id \"" + id + "\" does not exist.");
            var item = items[ids.IndexOf(id)];
            storage.Put(id, item);
            Updated(item);
        }

        public virtual void Remove(string id)
        {
            storage.Delete(id);
            var item = items[ids.IndexOf(id)];
            items.Remove(item);
            ids.Remove(id);
            storage.Put(idListKey, ids);
            Removed(item);
        }

        public virtual void Clear()
        {
            foreach (var id in ids) {
                storage.Delete(id);
            }
            storage.Delete(idListKey);
            ids.Clear();
            items.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public IEnumerable<T> Reverse()
        {
            for (int i = items.Count - 1; i >= 0; i--)
                yield return items[i];
        }

        public T this[int index] {
            get {
                return items[index];
            }
        }
    }
}
