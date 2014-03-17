using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace PerpetualEngine.Storage
{
    public class PersistentList<T> : IEnumerable, IEnumerable<T>, IEventExceutor  where T : IIdentifiable
    {
        SimpleStorage storage;
        const string idListKey = "ids";
        List<string> ids;
        List<T> items = new List<T>();

        public PersistentList(string editGroup, Func<T, string> serialize = null, Func<string, T> deserialize = null)
        {
            storage = SimpleStorage.EditGroup(editGroup);
            ids = storage.Get<List<string>>(idListKey) ?? new List<string>();

            var broken = new List<string>();
            foreach (var i in ids) {
                var item = deserialize == null ? storage.Get<T>(i) : deserialize(storage.Get(i));
                if (item == null) {
                    broken.Add(i);
                    continue;
                }
                items.Add(item);
            }

            if (broken.Count > 0) {
                foreach (var id in broken) {
                    storage.Delete(id);
                    ids.Remove(id);
                    storage.Put(idListKey, ids);
                }
            }

            CustomSerializer = serialize;
        }

        Func<T, string> CustomSerializer = null;

        public event Action<T> Added = delegate {};
        public event Action<int, T> Removed = delegate {};
        public event Action<T> Updated = delegate {};

        Dictionary<string, Action> onUpdatedSubscriptions = new Dictionary<string, Action>();

        public void ClearEventDelegates()
        {
            Added = delegate {
            };
            Removed = delegate {
            };
            Updated = delegate {
            };

            foreach (var id in onUpdatedSubscriptions.Keys) {
                onUpdatedSubscriptions[id] = null;
            }
            onUpdatedSubscriptions.Clear();
        }

        public void Subscribe(string id, Action onUpdated)
        {
            if (!onUpdatedSubscriptions.ContainsKey(id))
                onUpdatedSubscriptions.Add(id, delegate {
                });
            onUpdatedSubscriptions[id] += onUpdated;
        }

        public void Unsubscribe(string id, Action onUpdated)
        {
            if (onUpdatedSubscriptions.ContainsKey(id))
                onUpdatedSubscriptions[id] -= onUpdated;
        }

        public int Count { get { return items.Count; } }

        public bool IsEmpty { 
            get { 
                return items.IsEmpty(); 
            } 
        }

        public virtual void Add(string id, T value)
        {

        }

        public virtual void Add(T value)
        {
            var id = value.Id;
            if (id == idListKey)
                throw new ApplicationException("The id must not be \"" + idListKey + "\".");
            if (ids.Contains(id))
                throw new ApplicationException("Object with id \"" + id + "\" already exists.");
            Save(id, value);
            ids.Add(id);
            storage.Put(idListKey, ids);
            items.Add(value);

            Added(value);
        }

        /// <summary>
        /// Updates item with specified Id.
        /// </summary>
        public virtual void Update(string id)
        {
            if (!ids.Contains(id))
                throw new ApplicationException("Object with id \"" + id + "\" does not exist.");
            var item = ElementWith(id);
            Save(id, item);
            Updated(item);
            if (onUpdatedSubscriptions.ContainsKey(id))
                onUpdatedSubscriptions[id]();
        }

        void Save(string id, T value)
        {
            if (CustomSerializer == null)
                storage.Put(id, value);
            else
                storage.Put(id, CustomSerializer(value));
        }

        public virtual void Remove(string id)
        {
            storage.Delete(id);
            var index = ids.IndexOf(id);
            var item = items[index];
            items.Remove(item);
            ids.Remove(id);
            storage.Put(idListKey, ids);
            Removed(index, item);
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

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public T ElementAt(int index)
        {
            return items.ElementAt(index);
        }

        public T ElementWith(string id)
        {
            var index = ids.IndexOf(id);
            return items.ElementAt(index);
        }

        public bool Contains(string id)
        {
            return ids.Contains(id);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
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
