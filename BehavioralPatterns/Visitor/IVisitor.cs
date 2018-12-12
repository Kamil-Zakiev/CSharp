namespace Visitor
{
    public interface IVisitor
    {
        void Visit(CityBuilding cityBuilding);
        void Visit(CityPark cityPark);
    }
}
