using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PerpetualEngine.Storage
{
    [TestFixture()]
    public class FileCacheTests
    {
        [SetUp]
        public void Setup()
        {
            var tmp = Path.GetTempPath() + "fileCache";
            Directory.CreateDirectory(tmp);
            FileCache.Directory = tmp;
            foreach (var file in Directory.EnumerateFiles(FileCache.Directory)) {
                File.Delete(file);
            }
        }

        [Test()]
        public void TestAutoCreationOfCacheDirectory()
        {
            var tmp = Path.GetTempPath() + "fileCache_" + Guid.NewGuid().ToString();
            FileCache.Directory = tmp;
        }

        [Test()]
        public void TestSimultaneousFetchingAndGetting()
        {
            Console.WriteLine("starting");

            var fetchCount = 0;
            FileCache.Put("1", path => {
                Console.WriteLine("fetching new image");
                fetchCount++;
                new HttpClient().Get("http://perpetual-mobile.de/bilder/pavillon-aussen-1200px.jpg", path);
            });
            Task.Run(async delegate {
                var p = await FileCache.Get("1");
                Console.WriteLine("location1 is " + p);
            });
            Task.Run(delegate {
                FileCache.Fetch("1");
            });
            var location = "no path set";
            Task.Run(async delegate {
                var l = await FileCache.Get("1");
                Console.WriteLine("locatio2 is " + l);
                location = l;
            });

            Assert.That(() => File.Exists(location), Is.True.After(2000, 120));
            Assert.That(fetchCount, Is.EqualTo(1));
        }
    }
}

