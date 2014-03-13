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
        List<T> items = new List<T>();

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

        public List<T> ValidateAndLoad()
        {
            var obsoleteIds = new List<string>();
            var result = new List<T>();
            foreach (var id in ids) {
                var value = storage.Get<T>(id);
                if (value != null) {
                    result.Add(value);
                } else
                    obsoleteIds.Add(id);
            }

            if (obsoleteIds.Count > 0) {
                foreach (var id in obsoleteIds) {
                    storage.Delete(id);
                    ids.Remove(id);
                    storage.Put(idListKey, ids);
                }
            }

            return result;
        }

        public IEnumerable<T> Reverse()
        {
            var list = ValidateAndLoad();
            list.Reverse();
            return list;
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
            foreach (var i in ids)
                yield return storage.Get<T>(i);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (var i in ids)
                yield return storage.Get<T>(i);
        }
    }
}
