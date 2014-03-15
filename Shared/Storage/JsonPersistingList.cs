using Newtonsoft.Json;

namespace PerpetualEngine.Storage
{
    public class JsonPersistingList<T> : PersistentList<T>
    {
        public JsonPersistingList(string editGroup) : base(editGroup, 
                                                           (obj) => JsonConvert.SerializeObject(obj),
                                                           (str) => JsonConvert.DeserializeObject<T>(str))
        {
        }
    }
}