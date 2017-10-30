using System;
using System.Collections.Generic;

namespace Mills.UnitTests.Eventing
{
    public class MockEventSubscriber
    {
        int _count;

        private Dictionary<Action, int> count;

        public MockEventSubscriber()
        {
            Reset();
        }

        public int HitCount
        {
            get
            {
                return _count;
            }
        }
        
        public void Reset()
        {
            _count = 0;
        }

        public void Handle()
        {
            _count++;
        }

        public void Handle<T>(T t)
        {
            _count++;
        }

        public void Handle<T, U>(T t, U u)
        {
            _count++;
        }
    }
}
