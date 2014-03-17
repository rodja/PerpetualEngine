using NUnit.Framework;
using System;
using System.IO;

namespace PerpetualEngine
{
    [TestFixture()]
    public class ServerTests
    {
        private Server server;
        const string longUrl = "http://tinyurl.com/api-create.php?url=http://www.perpetual-mobile.de/";
        const string tinyUrl = "http://tinyurl.com/pcyutsv";
        const string imgUrl = "http://thebloodsugartrick.com/images/explenation.png";
        const string imgPath = "tmp.png";

        [SetUp]
        public void Setup()
        {
            server = new Server();
        }

        [Test()]
        public void TestGet()
        {
            var response = server.Get(longUrl);
            Assert.That(response, Is.EqualTo(tinyUrl));
        }

        [Test()]
        public async void TestGetAsync()
        {
            var response = await server.GetAsync(longUrl);
            Assert.That(response, Is.EqualTo(tinyUrl));
        }

        [Test()]
        public void TestGetFile()
        {
            server.GetFile(imgUrl, imgPath);
            var fileInfo = new FileInfo(imgPath);
            Assert.That(fileInfo.Length, Is.EqualTo(23725));
        }

        [Test()]
        public void TestGetFileAsync()
        {
            server.GetFileAsync(imgUrl, imgPath);
            var fileInfo = new FileInfo(imgPath);
            Assert.That(fileInfo.Length, Is.EqualTo(23725));
        }
    }
}
    