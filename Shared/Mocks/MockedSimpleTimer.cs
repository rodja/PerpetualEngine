using NUnit.Framework;
using System;

namespace PerpetualEngine
{
    public partial class SimpleTimer
    {
        static SimpleTimer()
        {
            SimpleTimer.Create = () => {
                return new MockedSimpleTimer();
            };
        }
    }

    class MockedSimpleTimer : SimpleTimer
    {
        public override void Repeat(TimeSpan timeSpan, Action action)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }
    }
}

