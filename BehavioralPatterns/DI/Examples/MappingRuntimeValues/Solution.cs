using System;

namespace DI.Examples.MappingRuntimeValues.Solution
{
    interface IRoute
    {
    }

    interface ILocation
    {
    }

    public enum RouteStratagy
    {
        Shortest = 1,
        Fastest = 2
    }

    interface IRouteBuilder
    {
        IRoute Build(ILocation from, ILocation to);
    }
    
    class SomeService
    {
        private readonly IRouteBuilderFactory _routeBuilderFactory;

        public SomeService(IRouteBuilderFactory routeBuilderFactory)
        {
            _routeBuilderFactory = routeBuilderFactory;
        }

        IRoute CreateRoute(RouteStratagy userPreference, ILocation from, ILocation to)
        {
            var routeBuilder = GetBuilder(userPreference);
            return routeBuilder.Build(from, to);
        }

        IRouteBuilder GetBuilder(RouteStratagy userPreference)
        {
            return _routeBuilderFactory.GetBuilder(userPreference);
        }
    }

    interface IRouteBuilderFactory
    {
        IRouteBuilder GetBuilder(RouteStratagy userPreference);
    }

    class RouteBuilderFactory : IRouteBuilderFactory
    {
        public IRouteBuilder GetBuilder(RouteStratagy userPreference)
        {
            switch (userPreference)
            {
                case RouteStratagy.Shortest:
                    return new ShortestRouteBuilder();
                case RouteStratagy.Fastest:
                    return new FastestRouteBuilder();
                default:
                    throw new NotSupportedException();
            }
        }
    }

    class ShortestRouteBuilder : IRouteBuilder
    {
        public IRoute Build(ILocation from, ILocation to)
        {
            throw new NotImplementedException();
        }
    }

    class FastestRouteBuilder : IRouteBuilder
    {
        public IRoute Build(ILocation from, ILocation to)
        {
            throw new NotImplementedException();
        }
    }
}
