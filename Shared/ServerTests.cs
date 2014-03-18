using NUnit.Framework;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        const string postUrl = "http://httpbin.org/post";
        //        const string postUrl = "http://www.posttestserver.com/post.php";
        //        const string postUrl = "http://sipt.perpetual-mobile.de/";
        const string deleteUrl = "http://httpbin.org/delete";

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
            server.Get(imgUrl, imgPath);
            var fileInfo = new FileInfo(imgPath);
            Assert.That(fileInfo.Length, Is.EqualTo(23725));
        }

        [Test()]
        public void TestGetFileAsync()
        {
            server.GetAsync(imgUrl, imgPath);
            var fileInfo = new FileInfo(imgPath);
            Assert.That(fileInfo.Length, Is.EqualTo(23725));
        }

        [Test()]
        public void TestPostFile()
        {
            var response = server.Post(postUrl, imgPath);
            // Assert.That(() => response, Is.Not.Null.After(10 * 60 * 1000));
            Console.WriteLine(response);

            dynamic json = JObject.Parse(response);
            Assert.NotNull(json["headers"]["Content-Type"], "should have content type");
            Assert.That(json["headers"]["Content-Type"].ToString(), Is.StringStarting("multipart/form-data; boundary="));
            Assert.That(json["files"].Count, Is.EqualTo(1));
        }

        [Test()]
        public async void TestPostFileAsync()
        {
            var response = await server.PostAsync(postUrl, imgPath);
            Assert.That(response, Is.StringContaining("\"Content-Length\": \"23725\""));
        }

        [Test()]
        public void TestDelete()
        {
            var response = server.Delete(deleteUrl);
            Assert.That(response, Is.StringContaining("\"data\": \"\""));
        }

        [Test()]
        public async void TestDeleteAsync()
        {
            var response = await server.DeleteAsync(deleteUrl);
            Assert.That(response, Is.StringContaining("\"data\": \"\""));
        }
    }
}
    