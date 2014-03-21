using NUnit.Framework;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PerpetualEngine
{
    [TestFixture()]
    public class HttpClientTests
    {
        private HttpClient httpClient;
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
            httpClient = new HttpClient();
        }

        [Test()]
        public void TestGet()
        {
            var response = httpClient.Get(longUrl);
            Assert.That(response, Is.EqualTo(tinyUrl));
        }

        [Test()]
        public async void TestGetAsync()
        {
            var response = await httpClient.GetAsync(longUrl);
            Assert.That(response, Is.EqualTo(tinyUrl));
        }

        [Test()]
        public void TestGetFile()
        {
            httpClient.Get(imgUrl, imgPath);
            var fileInfo = new FileInfo(imgPath);
            Assert.That(fileInfo.Length, Is.EqualTo(23725));
        }

        [Test()]
        public void TestGetFileAsync()
        {
            httpClient.GetAsync(imgUrl, imgPath);
            var fileInfo = new FileInfo(imgPath);
            Assert.That(fileInfo.Length, Is.EqualTo(23725));
        }

        [Test()]
        public void TestPostFile()
        {
            var response = httpClient.Post(postUrl, imgPath);
            dynamic json = JObject.Parse(response);
            Assert.NotNull(json["headers"]["Content-Type"], "should have content type");
            Assert.That(json["headers"]["Content-Type"].ToString(), Is.StringStarting("multipart/form-data; boundary="));
            Assert.That(json["files"].Count, Is.EqualTo(1));
        }

        [Test()]
        public async void TestPostFileAsync()
        {
            var response = await httpClient.PostAsync(postUrl, imgPath);
            dynamic json = JObject.Parse(response);
            Assert.NotNull(json["headers"]["Content-Type"], "should have content type");
            Assert.That(json["headers"]["Content-Type"].ToString(), Is.StringStarting("multipart/form-data; boundary="));
            Assert.That(json["files"].Count, Is.EqualTo(1));
        }

        [Test()]
        public void TestDelete()
        {
            var response = httpClient.Delete(deleteUrl);
            Assert.That(response, Is.StringContaining("\"data\": \"\""));
        }

        [Test()]
        public async void TestDeleteAsync()
        {
            var response = await httpClient.DeleteAsync(deleteUrl);
            Assert.That(response, Is.StringContaining("\"data\": \"\""));
        }
    }
}
    