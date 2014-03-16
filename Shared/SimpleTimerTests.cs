using NUnit.Framework;
using System;

namespace PerpetualEngine
{
    [TestFixture()]
    public class SimpleTimerTests
    {
        [Test()]
        public void TestRepeating()
        {
            var timer = SimpleTimer.Create();
            var count = 0;
            timer.Repeat(TimeSpan.FromMilliseconds(100), delegate {
                count++;
            });
            Assert.That(count, Is.EqualTo(0));
            Assert.That(() => count, Is.EqualTo(2).After(220));
        }
    }
}

