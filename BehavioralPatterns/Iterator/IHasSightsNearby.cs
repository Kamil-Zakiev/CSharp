using System.Collections.Generic;

namespace Iterator
{
    public interface IHasSightsNearby
    {
        IReadOnlyList<CityPark> WalkingDistanceParks { get; }
        IReadOnlyList<CityPark> TrainDistanceParks { get; }
        IReadOnlyList<CityPark> CarDistanceParks { get; }
        IReadOnlyList<IceRink> WalkingDistanceRinks { get; }
        IReadOnlyList<IceRink> TrainDistanceRinks { get; }
        IReadOnlyList<IceRink> CarDistanceRinks { get; }
        IReadOnlyList<Zoo> WalkingDistanceZoos { get; }
        IReadOnlyList<Zoo> TrainDistanceZoos { get; }
        IReadOnlyList<Zoo> CarDistanceZoos { get; }
        IReadOnlyList<CityBuilding> WalkingDistanceCityBuildings { get; }
        IReadOnlyList<CityBuilding> TrainDistanceCityBuildings { get; }
        IReadOnlyList<CityBuilding> CarDistanceCityBuildings { get; }
    }
}
