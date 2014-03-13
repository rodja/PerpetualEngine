using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PerpetualEngine.Storage
{
    public class PersistentList<T>: IEnumerable, IEnumerable<T>
    {
        SimpleStorage storage;
        const string idListKey = "ids";
        List<string> ids;

        public PersistentList(string editGroup)
        {
            storage = SimpleStorage.EditGroup(editGroup);
            ids = storage.Get<List<string>>(idListKey) ?? new List<string>();
        }

        public void Add(string id, T value)
        {
            Insert(ids.Count, id, value);
        }

        public void Insert(int index, string id, T value)
        {
            if (id == idListKey)
                throw new ApplicationException("The id must not be \"" + idListKey + "\".");
            if (ids.Contains(id))
                throw new ApplicationException("Object with id \"" + id + "\" already exists.");
            storage.Put(id, value);
            ids.Insert(index, id);
            storage.Put(idListKey, ids);
        }

        /// <summary>
        /// Updates item with specified Id or adds it to the list.
        /// </summary>
        public void Update(string id, T value)
        {
            if (!ids.Contains(id))
                throw new ApplicationException("Object with id \"" + id + "\" does not exist.");
            storage.Put(id, value);
        }

        public void Remove(string id)
        {
            storage.Delete(id);
            ids.Remove(id);
            storage.Put(idListKey, ids);
        }

        public void Clear()
        {
            foreach (var id in ids) {
                storage.Delete(id);
            }
            storage.Delete(idListKey);
            ids = new List<string>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var obsoleteIds = new List<string>();
            foreach (var i in ids) {
                var item = storage.Get<T>(i);
                if (item == null) {
                    obsoleteIds.Add(i);
                    continue;
                }
                yield return item;
            }

            Remove(obsoleteIds);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var obsoleteIds = new List<string>();
            foreach (var i in ids) {
                var item = storage.Get<T>(i);
                if (item == null) {
                    obsoleteIds.Add(i);
                    continue;
                }
                yield return item;
            }

            Remove(obsoleteIds);
        }

        public IEnumerable<T> Reverse()
        {
            var obsoleteIds = new List<string>();
            for (int i = ids.Count - 1; i >= 0; i--) {
                var item = storage.Get<T>(ids[i]);
                if (item == null) {
                    obsoleteIds.Add(ids[i]);
                    continue;
                }
                yield return item;
            }

            Remove(obsoleteIds);
        }

        void Remove(List<string> obsoleteIds)
        {
            if (obsoleteIds.Count > 0) {
                foreach (var id in obsoleteIds) {
                    storage.Delete(id);
                    ids.Remove(id);
                    storage.Put(idListKey, ids);
                }
            }
        }
    }
}
