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
            timer.Repeat(TimeSpan.FromMilliseconds(1000), delegate {
                count++;
            });
            Assert.That(count, Is.EqualTo(0));
            Assert.That(() => count, Is.EqualTo(0).After(100)); // t = 100
            Assert.That(() => count, Is.EqualTo(0).After(800)); // t = 900
            Assert.That(() => count, Is.EqualTo(1).After(200)); // t = 1100
            Assert.That(() => count, Is.EqualTo(1).After(800)); // t = 1900
            Assert.That(() => count, Is.EqualTo(2).After(200)); // t = 2100
        }

        [Test()]
        public void TestRepeatingImmediate()
        {
            var timer = SimpleTimer.Create();
            var count = 0;
            timer.Repeat(TimeSpan.FromMilliseconds(1000), delegate {
                count++;
            }, true);
            Assert.That(count, Is.EqualTo(1));
            Assert.That(() => count, Is.EqualTo(1).After(100)); // t = 100
            Assert.That(() => count, Is.EqualTo(1).After(800)); // t = 900
            Assert.That(() => count, Is.EqualTo(2).After(200)); // t = 1100
            Assert.That(() => count, Is.EqualTo(2).After(800)); // t = 1900
            Assert.That(() => count, Is.EqualTo(3).After(200)); // t = 2100
        }
    }
}

