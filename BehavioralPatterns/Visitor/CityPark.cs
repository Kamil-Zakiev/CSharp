namespace Visitor
{
    public class CityPark : IVisitable
    {
        // some fields ...

        public void Visit(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
