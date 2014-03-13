using System;
using System.Collections.Generic;

namespace PerpetualEngine.Storage
{
    /// <summary>
    /// Deskotp specific implementation to let EditGroup(string) return an DesktopSimpleStorage object
    /// </summary>
    public partial class SimpleStorage
    {
        static SimpleStorage()
        {
            SimpleStorage.EditGroup = (name) => {
                return new DesktopSimpleStorage(name);
            };
        }

        public void Clear()
        {
            (this as DesktopSimpleStorage).database.Clear();
        }
    }

    /// <summary>
    /// Does not persistent right now... only used for unit tests
    /// </summary>
    public class DesktopSimpleStorage : SimpleStorage
    {
        public Dictionary<string,string> database = new Dictionary<string,string>();

        public DesktopSimpleStorage(string groupName) : base(groupName)
        {
        }

        /// <summary>
        /// Persists a value with given key.
        /// </summary>
        /// <param name="value">if value is null, the key will be deleted</param>
        override public void Put(string key, string value)
        {
            if (value == null) {
                Delete(key);
                return;
            }
            var id = Group + "_" + key;
            if (database.ContainsKey(id))
                database.Remove(id);

            database.Add(id, value);
        }

        /// <summary>
        /// Retrieves value with given key.
        /// </summary>
        /// <returns>null, if key can not be found</returns>
        override public string Get(string key)
        {
            try {
                return database[Group + "_" + key];
            } catch (KeyNotFoundException e) {
                return null;
            }
        }

        /// <summary>
        /// Delete the specified key.
        /// </summary>
        override public void Delete(string key)
        {
            database.Remove(Group + "_" + key);
        }
    }
}