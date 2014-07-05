using NUnit.Framework;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Linq;

namespace PerpetualEngine
{
    [TestFixture()]
    public class HttpClientTests
    {
        private HttpClient httpClient;
        const string longUrl = "http://tinyurl.com/api-create.php?url=http://www.perpetual-mobile.de/";
        const string tinyUrl = "http://tinyurl.com/pcyutsv";
        const string imgUrl = "http://thebloodsugartrick.com/images/explenation.png";
        const string postUrl = "http://httpbin.org/post";
        //        const string postUrl = "http://www.posttestserver.com/post.php";
        //        const string postUrl = "http://sipt.perpetual-mobile.de/";
        const string deleteUrl = "http://httpbin.org/delete";
        string tmpDir = "";

        [SetUp]
        public void Setup()
        {
            tmpDir = Path.GetTempPath() + "httpCacheTests/";
            Directory.CreateDirectory(tmpDir);
            foreach (var file in Directory.EnumerateFiles(tmpDir)) {
                File.Delete(file);
            }
            httpClient = new HttpClient();
        }

        [Test()]
        public void TestGet()
        {
            var response = httpClient.Get(longUrl);
            Assert.That(response, Is.EqualTo(tinyUrl));
        }

        [Test()]
        public void TestGetAuth()
        {
            httpClient.Credentials = new System.Net.NetworkCredential("user", "passwd");
            var response = httpClient.Get("http://httpbin.org/basic-auth/user/passwd");
            Assert.That(response, Is.EqualTo("{\n  \"authenticated\": true, \n  \"user\": \"user\"\n}"));
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
            var file = GenerateFilePath();
            httpClient.Get(imgUrl, file);
            var fileInfo = new FileInfo(file);
            Assert.That(fileInfo.Length, Is.EqualTo(23725));
        }

        [Test()]
        public async void TestGetFileAsync()
        {
            var file = GenerateFilePath();
            await httpClient.GetAsync(imgUrl, file);
            Assert.That(File.Exists(file), Is.True);
            Assert.That(new FileInfo(file).Length, Is.EqualTo(23725));
        }

        [Test()]
        public void TestPostFile()
        {
            var file = GenerateFilePath();
            httpClient.Get(imgUrl, file);
            var response = httpClient.Post(postUrl, file);
            var json = JObject.Parse(response);
            Assert.NotNull(json["headers"]["Content-Type"], "should have content type");
            Assert.That(json["headers"]["Content-Type"].ToString(), Is.StringStarting("multipart/form-data; boundary="));
            Assert.That(json["files"].Count(), Is.EqualTo(1));
        }

        [Test()]
        public async void TestPostFileAsync()
        {
            var file = GenerateFilePath();
            httpClient.Get(imgUrl, file);
            var response = await httpClient.PostAsync(postUrl, file);
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

        string GenerateFilePath()
        {
            return tmpDir + Guid.NewGuid();
        }
    }
}
    