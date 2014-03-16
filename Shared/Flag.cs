using System;

namespace PerpetualEngine
{
    public class Flag
    {
        bool isSet = false;

        public static implicit operator bool(Flag f)
        {
            return f.isSet;
        }

        public static IDisposable Set(Flag f)
        {
            return new Setter(f);
        }

        class Setter: IDisposable
        {
            Flag flag;

            public Setter(Flag f)
            {
                if (f.isSet)
                    throw new ApplicationException("Trying to set flag which already is set!");
                flag = f;
                flag.isSet = true; 
            }

            public void Dispose()
            {
                if (!flag.isSet)
                    throw new ApplicationException("Trying to unset flag which is not set!");
                flag.isSet = false;
            }
        }
    }
}

