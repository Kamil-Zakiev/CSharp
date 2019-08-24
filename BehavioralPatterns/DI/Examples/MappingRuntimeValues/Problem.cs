using System;

namespace DI.Examples.MappingRuntimeValues.Problem
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
        IRoute CreateRoute(RouteStratagy userPreference, ILocation from, ILocation to)
        {
            var routeBuilder = GetBuilder(userPreference);
            return routeBuilder.Build(from, to);
        }

        IRouteBuilder GetBuilder(RouteStratagy userPreference)
        {
            throw new NotImplementedException();
        }
    }
}
