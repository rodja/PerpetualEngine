using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PerpetualEngine.Storage
{
    [TestFixture()]
    public class FileCacheTests
    {
        static FileCacheTests()
        {
            FileCache.Directory = Path.GetTempPath();
        }

        [Test()]
        public void TestSimultaneousFetchingAndGetting()
        {
            FileCache.Put("1", path => new HttpClient().Get("http://perpetual-mobile.de/bilder/pavillon-aussen-1200px.jpg", path));
            Task.Run(delegate {
                FileCache.Fetch("1");
            });
            var location = "no path set";
            Task.Run(async delegate {
                location = await FileCache.Get("1");
            });

            Assert.That(() => File.Exists(location), Is.True.After(2000, 120));
        }
    }
}

