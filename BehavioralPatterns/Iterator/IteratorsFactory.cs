using System;

namespace Iterator
{
    public class IteratorsFactory
    {
        public static IIterator GetWalkingIterator(IHasSightsNearby sightseeing)
        {
            return new WalkingIterator(sightseeing);
        }

        public static IIterator GetTrainIterator(IHasSightsNearby sightseeing)
        {
            throw new NotImplementedException();
        }

        private class WalkingIterator : IIterator
        {
            private IHasSightsNearby sightseeing;

            public WalkingIterator(IHasSightsNearby sightseeing)
            {
                this.sightseeing = sightseeing;
            }

            public object Current => throw new NotImplementedException();

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }
        }
    }
}
