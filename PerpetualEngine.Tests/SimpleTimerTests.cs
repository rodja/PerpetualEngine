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
        }

        [Test()]
        public void TestRepeatingImmediate()
        {
            var timer = SimpleTimer.Create();
            var count = 0;
            timer.Repeat(TimeSpan.FromMilliseconds(400), () => count++, immediate: true);
            Assert.That(count, Is.EqualTo(1));
            Assert.That(() => count, Is.EqualTo(1).After(20));  // t = 20
            Assert.That(() => count, Is.EqualTo(1).After(330)); // t = 350
            Assert.That(() => count, Is.EqualTo(2).After(200)); // t = 550
            Assert.That(() => count, Is.EqualTo(2).After(150)); // t = 700
            Assert.That(() => count, Is.EqualTo(3).After(300)); // t = 1000
            timer.Clear();
            Assert.That(count, Is.EqualTo(3));
        }

        [Test()]
        public void TestChangingSpan()
        {
            var timer = SimpleTimer.Create();
            var count = 0;
            timer.Repeat(TimeSpan.FromMilliseconds(200), () => count++);
            Assert.That(count, Is.EqualTo(0));
            Assert.That(() => count, Is.EqualTo(0).After(20));  // t = 20
            Assert.That(() => count, Is.EqualTo(0).After(160)); // t = 180
            Assert.That(() => count, Is.EqualTo(1).After(70));  // t = 250
            Assert.That(() => count, Is.EqualTo(1).After(50)); // t = 300
            timer.Clear();
            Assert.That(() => count, Is.EqualTo(1).After(200));

            timer.Repeat(TimeSpan.FromMilliseconds(400), () => count++);
            Assert.That(count, Is.EqualTo(1));
            Assert.That(() => count, Is.EqualTo(1).After(20));  // t = 20
            Assert.That(() => count, Is.EqualTo(1).After(300)); // t = 300
            Assert.That(() => count, Is.EqualTo(2).After(150));  // t = 450
            Assert.That(() => count, Is.EqualTo(2).After(250)); // t = 780
            timer.Clear();
            Assert.That(count, Is.EqualTo(2));
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
