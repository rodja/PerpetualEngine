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
            timer.Repeat(TimeSpan.FromMilliseconds(200), delegate {
                count++;
            });
            Assert.That(count, Is.EqualTo(0));
            Assert.That(() => count, Is.EqualTo(0).After(20));  // t = 20
            Assert.That(() => count, Is.EqualTo(0).After(160)); // t = 180
            Assert.That(() => count, Is.EqualTo(1).After(40));  // t = 220
            Assert.That(() => count, Is.EqualTo(1).After(160)); // t = 380
            Assert.That(() => count, Is.EqualTo(2).After(40));  // t = 420
            timer.Clear();
            Assert.That(count, Is.EqualTo(2));
            // commenting out the following yields an error since timer needs some time to stop!
            Assert.That(() => count, Is.EqualTo(2).After(400));
        }

        [Test()]
        public void TestRepeatingImmediate()
        {
            var timer = SimpleTimer.Create();
            var count = 0;
            timer.Repeat(TimeSpan.FromMilliseconds(200), delegate {
                count++;
            }, true);
            Assert.That(count, Is.EqualTo(1));
            Assert.That(() => count, Is.EqualTo(1).After(20));  // t = 20
            Assert.That(() => count, Is.EqualTo(1).After(160)); // t = 180
            Assert.That(() => count, Is.EqualTo(2).After(40));  // t = 220
            Assert.That(() => count, Is.EqualTo(2).After(160)); // t = 380
            Assert.That(() => count, Is.EqualTo(3).After(40));  // t = 420
            timer.Clear();
            Assert.That(count, Is.EqualTo(3));
            // commenting out the following yields an error since timer needs some time to stop!
            Assert.That(() => count, Is.EqualTo(3).After(400));
        }

        [Test()]
        public void TestChangingSpan()
        {
            var timer = SimpleTimer.Create();
            var count = 0;
            timer.Repeat(TimeSpan.FromMilliseconds(200), delegate {
                count++;
            });
            Assert.That(count, Is.EqualTo(0));
            Assert.That(() => count, Is.EqualTo(0).After(20));  // t = 20
            Assert.That(() => count, Is.EqualTo(0).After(160)); // t = 180
            Assert.That(() => count, Is.EqualTo(1).After(40));  // t = 220
            Assert.That(() => count, Is.EqualTo(1).After(160)); // t = 380
            timer.Clear();
            Assert.That(count, Is.EqualTo(1));
            // commenting out the following yields an error since timer needs some time to stop!
            Assert.That(() => count, Is.EqualTo(1).After(400));

            timer.Repeat(TimeSpan.FromMilliseconds(400), delegate {
                count++;
            });
            Assert.That(count, Is.EqualTo(1));
            Assert.That(() => count, Is.EqualTo(1).After(20));  // t = 30
            Assert.That(() => count, Is.EqualTo(1).After(360)); // t = 380
            Assert.That(() => count, Is.EqualTo(2).After(40));  // t = 420
            Assert.That(() => count, Is.EqualTo(2).After(360)); // t = 780
            timer.Clear();
            Assert.That(count, Is.EqualTo(2));
            // commenting out the following yields an error since timer needs some time to stop!
            Assert.That(() => count, Is.EqualTo(2).After(800));
        }

        [Test()]
        public void TestIsRunning()
        {
            var timer = SimpleTimer.Create();
            var count = 0;
            timer.Repeat(TimeSpan.FromMilliseconds(100), delegate {
                count++;
            });
            Assert.That(timer.IsRunning());
            Assert.That(() => timer.IsRunning(), Is.True.After(100));

            timer.Clear();
            Assert.That(timer.IsRunning(), Is.False);
            Assert.That(() => timer.IsRunning(), Is.False.After(100));
        }
    }
}
