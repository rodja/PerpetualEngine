using System;
using NUnit.Framework;
using System.Linq;

namespace PerpetualEngine.Storage
{
    [TestFixture()]
    public class PersistentStringsTest
    {
        string editGroup;

        [SetUp]
        public void Setup()
        {
            editGroup = Guid.NewGuid().ToString();
        }

        PersistentStrings BuildTestList()
        {
            var list = new PersistentStrings(editGroup);
            list.Add("1");
            list.Add("2");
            list.Add("3");
            return list;
        }

        [Test()]
        public void TestLoadingStringsFromPersitence()
        {
            var list = BuildTestList();
            list = new PersistentStrings(editGroup);
            var i = 1;
            foreach (var a in list)
                Assert.AreEqual(a, i++.ToString());
            Assert.AreEqual(3, list.Count);
        }

        [Test]
        public void TestInsertingItems()
        {
            var list = BuildTestList();
            list.Insert(1, "x");
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[1], Is.EqualTo("x"));

            list.Insert(0, "y");
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list.First(), Is.EqualTo("y"));
        }

    }
}

