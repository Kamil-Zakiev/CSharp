namespace Iterator
{
    public class UsageExample
    {
        public void SomeMethod(IHasSightsNearby sightseeing)
        {
            var walkingIterator = sightseeing.GetWalkingIterator();
            while (walkingIterator.MoveNext())
            {
                var anotherSightseeing = walkingIterator.Current;

                // do smth with anotherSightseeing
            }
        }
    }
}
