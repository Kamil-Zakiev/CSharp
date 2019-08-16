using System;

namespace Iterator
{
    public static class IteratorsFactoryExtensions
    {
        public static IIterator<IHasSightsNearby> GetWalkingIterator(this IHasSightsNearby sightseeing)
        {
            return new WalkingIterator(sightseeing);
        }

        public static IIterator<IHasSightsNearby> GetTrainIterator(this IHasSightsNearby sightseeing)
        {
            throw new NotImplementedException();
        }

        private class WalkingIterator : IIterator<IHasSightsNearby>
        {
            private IHasSightsNearby sightseeing;

            public WalkingIterator(IHasSightsNearby sightseeing)
            {
                this.sightseeing = sightseeing;
            }

            public IHasSightsNearby Current => throw new NotImplementedException();

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }
        }
    }
}
