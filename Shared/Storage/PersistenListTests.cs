using NUnit.Framework;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace PerpetualEngine.Storage
{
    [TestFixture()]
    public class PersistenListTests
    {
        string editGroup;

        [SetUp]
        public void Setup()
        {
            editGroup = Guid.NewGuid().ToString();
        }

        PersistentList<A> BuildTestList()
        {
            var list = new PersistentList<A>(editGroup);
            list.Add(new A("1"));
            list.Add(new A("2"));
            list.Add(new A("3"));
            return list;
        }

        [Test()]
        public void TestLoadingOfObjectsFromPersitence()
        {
            var list = BuildTestList();
            list = new PersistentList<A>(editGroup);
            var i = 1;
            foreach (var a in list)
                Assert.AreEqual(a.Id, i++.ToString());
            Assert.AreEqual(3, list.Count);
        }

        [Test()]
        public void TestReverseIterating()
        {
            var list = BuildTestList();
            int count = 3;
            foreach (var a in list.Reverse()) {
                Assert.AreEqual(count--.ToString(), a.Id);
            }
            Assert.AreEqual(0, count);
        }

        [Test()]
        public void TestIteratingOverListWithBrokenItems()
        {
            var list = BuildTestList();
            var storage = SimpleStorage.EditGroup(editGroup);
            storage.Put("2", "break data with id 2");
            list = new PersistentList<A>(editGroup);

            int count = 0;
            foreach (var a in list)
                Assert.AreEqual(a.Id, (++count + (count > 1 ? 1 : 0)).ToString());
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, list.Count);

            Assert.IsFalse(storage.HasKey("2"),
                "the broken object should atomaticly be removed");
            Assert.AreEqual(2, storage.Get<List<string>>("ids").Count,
                "the internal object-index should automaticly remove the broken id");
        }

        [Test()]
        public void TestKeepingInstancesAlive()
        {
            var list = BuildTestList();
            Assert.IsTrue(object.ReferenceEquals(list[1], list[1]));
        }

        [Test()]
        public void TestUsingJsonPersistingList()
        {
            var list = new JsonPersistingList<A>(editGroup);
            list.Add(new A("test"));
            Assert.AreEqual("{\"Id\":\"test\"}", SimpleStorage.EditGroup(editGroup).Get("test"));

            list = new JsonPersistingList<A>(editGroup);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("test", list.First().Id);
        }

        [Serializable]
        class A : IIdentifiable
        {
            public string Id { get; set; }

            public A(string id)
            {
                Id = id;
            }
        }

        [Serializable]
        class SomethingWhichShouldBeStoredAsJson : IIdentifiable, ISerializable
        {
            public string Id { get; set; }

            public SomethingWhichShouldBeStoredAsJson(string id)
            {
                Id = id;
            }

            public SomethingWhichShouldBeStoredAsJson(SerializationInfo info, StreamingContext context)
            {
                Id = JsonConvert.DeserializeObject<SomethingWhichShouldBeStoredAsJson>(info.GetString("json")).Id;
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("json", JsonConvert.SerializeObject(this));
            }
        }
    }
}
