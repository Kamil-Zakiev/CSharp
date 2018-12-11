using System.Collections.Generic;

namespace Iterator
{
    public class Zoo : IHasSightsNearby
    {
        public IReadOnlyList<CityPark> WalkingDistanceParks { get; }
        public IReadOnlyList<CityPark> TrainDistanceParks { get; }
        public IReadOnlyList<CityPark> CarDistanceParks { get; }
        public IReadOnlyList<IceRink> WalkingDistanceRinks { get; }
        public IReadOnlyList<IceRink> TrainDistanceRinks { get; }
        public IReadOnlyList<IceRink> CarDistanceRinks { get; }
        public IReadOnlyList<Zoo> WalkingDistanceZoos { get; }
        public IReadOnlyList<Zoo> TrainDistanceZoos { get; }
        public IReadOnlyList<Zoo> CarDistanceZoos { get; }
        public IReadOnlyList<CityBuilding> WalkingDistanceCityBuildings { get; }
        public IReadOnlyList<CityBuilding> TrainDistanceCityBuildings { get; }
        public IReadOnlyList<CityBuilding> CarDistanceCityBuildings { get; }
    }
}
