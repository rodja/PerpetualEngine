using NUnit.Framework;
using System;

namespace PerpetualEngine
{
    [TestFixture()]
    public class HelpfulExtensionTests
    {
        [Test()]
        public void TestConvertingUnixTimeStampToDateTime()
        {
            long timeStamp = 1401361125;
            Assert.That(timeStamp.ToDateTime(), Is.EqualTo(DateTime.Parse("Thu, 29 May 2014 08:58:45 GMT")));
        }

        [Test()]
        public void TestConvertingDateTimeToUnixTimeStamp()
        {
            var date = DateTime.Parse("Thu, 29 May 2014 08:58:45 GMT");
            Assert.That(date.ToUnixTimeStamp(), Is.EqualTo(1401361125));
        }
    }
}

