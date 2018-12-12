namespace Visitor
{
    public class CityBuilding : IVisitable
    {
        // some fields ...

        public void Visit(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
