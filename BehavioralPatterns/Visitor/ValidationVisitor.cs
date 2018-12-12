using System;
using System.Collections.Generic;

namespace Visitor
{
    public class ValidationVisitor : IVisitor
    {
        private List<string> _errorsList = new List<string>();

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
