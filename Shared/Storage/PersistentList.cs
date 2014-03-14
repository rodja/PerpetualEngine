using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace PerpetualEngine.Storage
{
    public class PersistentList<T> : IEnumerable, IEnumerable<T> where T : IIdentifiable
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

        public int Count { get { return items.Count; } }

        public void Add(T value)
        {
            Insert(ids.Count, value);
        }

        public void Insert(int index, T value)
        {
            if (value.Id == idListKey)
                throw new ApplicationException("The id must not be \"" + idListKey + "\".");
            if (ids.Contains(value.Id))
                throw new ApplicationException("Object with id \"" + value.Id + "\" already exists.");
            storage.Put(value.Id, value);
            ids.Insert(index, value.Id);
            storage.Put(idListKey, ids);
            items.Add(value);
        }

        /// <summary>
        /// Updates item with specified Id.
        /// </summary>
        public void Update(string id)
        {
            if (!ids.Contains(id))
                throw new ApplicationException("Object with id \"" + id + "\" does not exist.");
            storage.Put(id, items[ids.IndexOf(id)]);
        }

        public void Remove(string id)
        {
            storage.Delete(id);
            items.RemoveAt(ids.IndexOf(id));
            ids.Remove(id);
            storage.Put(idListKey, ids);
        }

        public void Clear()
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
