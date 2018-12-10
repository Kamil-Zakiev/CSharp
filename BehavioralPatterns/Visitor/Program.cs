using System;
using System.Collections.Generic;

namespace Visitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public interface IVisitor
    {
        void Visit(CityBuilding cityBuilding);
        void Visit(CityPark cityPark);
    }

    public interface IVisitable
    {
        void Visit(IVisitor visitor);
    }

    public class CityBuilding : IVisitable
    {
        // some fields ...

        public void Visit(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class CityPark : IVisitable
    {
        // some fields ...

        public void Visit(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

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
