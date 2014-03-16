using NUnit.Framework;
using System;

namespace PerpetualEngine
{
    [TestFixture()]
    public class SimpleTimerTests
    {
        [Test()]
        public void TestInstantiation()
        {
            var timer = SimpleTimer.Create();

        }
    }
}

