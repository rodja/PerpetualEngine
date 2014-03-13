using NUnit.Framework;
using System;
using System.Runtime.Serialization;

namespace PerpetualEngine.Storage
{
    [TestFixture()]
    public class PersistenListTests
    {
        static int deserializationCount = 0;

        [SetUp]
        public void Setup()
        {
            deserializationCount = 0;
        }

        [Test()]
        public void TestLazyLoadingOfObjectsFromPersitence()
        {
            var list = new PersistentList<A>(Guid.NewGuid().ToString());
            list.Add("1", new A(1));
            list.Add("2", new A(2));
            list.Add("3", new A(3));
            foreach (var a in list)
                Assert.AreEqual(a.Id, deserializationCount);
        }

        [Serializable]
        class A: IDeserializationCallback
        {
            public int Id;

            public A(int id)
            {
                Id = id;
                Console.WriteLine("A created");
            }

            public virtual void OnDeserialization(object sender)
            {
                deserializationCount++;
                Console.WriteLine("A deserialized");
            }
        }
    }
}

