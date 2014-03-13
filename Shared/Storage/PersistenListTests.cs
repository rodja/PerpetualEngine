using NUnit.Framework;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace PerpetualEngine.Storage
{
    [TestFixture()]
    public class PersistenListTests
    {
        static int deserializationCount = 0;
        string editGroup;

        [SetUp]
        public void Setup()
        {
            deserializationCount = 0;
            editGroup = Guid.NewGuid().ToString();
        }

        PersistentList<A> BuildTestList()
        {
            var list = new PersistentList<A>(editGroup);
            list.Add("1", new A(1));
            list.Add("2", new A(2));
            list.Add("3", new A(3));
            return list;
        }

        [Test()]
        public void TestLazyLoadingOfObjectsFromPersitence()
        {
            var list = BuildTestList();
            foreach (var a in list)
                Assert.AreEqual(a.Id, deserializationCount);
            Assert.AreEqual(3, deserializationCount);
        }

        [Test()]
        public void TestReverseIterating()
        {
            var list = BuildTestList();
            int count = 3;
            foreach (var a in list.Reverse()) {
                Assert.AreEqual(count--, a.Id);
                Assert.AreEqual(a.Id, 4 - deserializationCount, 
                    "should also lazy load objects from persistence");
            }
            Assert.AreEqual(0, count);
        }

        [Test()]
        public void TestIteratingOverListWithBrokenItems()
        {
            var list = BuildTestList();
            var storage = SimpleStorage.EditGroup(editGroup);
            storage.Put("2", "break data with id 2");

            int count = 0;
            foreach (var a in list)
                Assert.AreEqual(a.Id, ++count + (count > 1 ? 1 : 0));
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, deserializationCount);

            Assert.IsFalse(storage.HasKey("2"),
                "the broken object should atomaticly be removed");
            Assert.AreEqual(2, storage.Get<List<string>>("ids").Count,
                "the internal object-index should automaticly remove the broken id");
        }

        [Serializable]
        class A: IDeserializationCallback
        {
            public int Id;

            public A(int id)
            {
                Id = id;
                Console.WriteLine("A created with id " + id);
            }

            public virtual void OnDeserialization(object sender)
            {
                deserializationCount++;
                Console.WriteLine("A deserialized with id " + Id);
            }
        }
    }
}

