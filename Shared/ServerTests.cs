using NUnit.Framework;
using System;

namespace PerpetualEngine
{
    [TestFixture()]
    public class ServerTests
    {
        private Server server;
        const string url = "http://tinyurl.com/api-create.php?url=http://www.perpetual-mobile.de/";
        const string tinyUrl = "http://tinyurl.com/pcyutsv";

        [SetUp]
        public void Setup()
        {
            server = new Server();
        }

        [Test()]
        public void TestGet()
        {
            var response = server.Get(url);
            Assert.That(response, Is.EqualTo(tinyUrl));
        }

        [Test()]
        public async void TestGetAsync()
        {
            var response = await server.GetAsync(url);
            Assert.That(response, Is.EqualTo(tinyUrl));
        }
    }
}
    