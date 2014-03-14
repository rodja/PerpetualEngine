using NUnit.Framework;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

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
            list.Add("1", new A(1));
            list.Add("2", new A(2));
            list.Add("3", new A(3));
            return list;
        }

        [Test()]
        public void TestLoadingOfObjectsFromPersitence()
        {
            var list = BuildTestList();
            list = new PersistentList<A>(editGroup);
            var i = 1;
            foreach (var a in list)
                Assert.AreEqual(a.Id, i++);
            Assert.AreEqual(3, list.Count);
        }

        [Test()]
        public void TestReverseIterating()
        {
            var list = BuildTestList();
            int count = 3;
            foreach (var a in list.Reverse()) {
                Assert.AreEqual(count--, a.Id);
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
                Assert.AreEqual(a.Id, ++count + (count > 1 ? 1 : 0));
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

        [Serializable]
        class A
        {
            public int Id;

            public A(int id)
            {
                Id = id;
            }
        }
    }
}

