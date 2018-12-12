using System;

namespace Visitor
{
    public class ExportVisitor : IVisitor
    {
        public void Visit(CityBuilding cityBuilding)
        {
            throw new NotImplementedException();
        }

        public void Visit(CityPark cityPark)
        {
            throw new NotImplementedException();
        }
    }
}
