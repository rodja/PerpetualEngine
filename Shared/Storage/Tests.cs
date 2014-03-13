using System;
using NUnit.Framework;

namespace PerpetualEngine.Storage
{
    [TestFixture()]
    public partial class SimpleStorageTests
    {
        [Test()]
        public void testSavingString()
        {
            var storage = SimpleStorage.EditGroup(Guid.NewGuid().ToString());
            storage.Put("test-key", "22");
            Assert.AreEqual("22", storage.Get("test-key"));
        }
    }
}