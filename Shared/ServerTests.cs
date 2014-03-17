using NUnit.Framework;
using System;

namespace PerpetualEngine
{
    [TestFixture()]
    public class ServerTests
    {
        [Test()]
        public void TestDownloadString()
        {
            var server = new Server();
            var url = "http://tinyurl.com/api-create.php?url=http://www.perpetual-mobile.de/";
            var response = server.DownloadString(url);
            Assert.That(response, Is.EqualTo("http://tinyurl.com/pcyutsv"));
        }
    }
}
    