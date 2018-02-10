using System;
using System.Web.Mvc;

namespace MyWebApp.Filters
{
    public class MyActionFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        private readonly Guid _filterGuid = Guid.NewGuid();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Logger.Log(_filterGuid);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Logger.Log(_filterGuid);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Logger.Log(_filterGuid);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Logger.Log(_filterGuid);
        }
    }
}