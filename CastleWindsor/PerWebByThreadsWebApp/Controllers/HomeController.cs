using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Castle.Windsor;

namespace PerWebByThreadsWebApp.Controllers
{
    public class HomeController : Controller, IDisposable
    {
        void IDisposable.Dispose()
        {
            Console.WriteLine("HomeController Dispose");
        } 
        
        private static readonly ManualResetEventSlim ManualResetEventSlim = new ManualResetEventSlim(false);

        public static I1 Method(IWindsorContainer container)
        {
            ManualResetEventSlim.Wait();
            return container.Resolve<I1>();
        }
        
        public IWindsorContainer Container { get; set; }
        
        /// <summary> HttpContext.Current is null. PerWebRequestLifestyle can only be used in ASP.Net </summary>
        public ActionResult Index()
        {
            var processorCount = 1;//Environment.ProcessorCount;
            var tasks = new Task<I1>[processorCount];
            for (var i = 0; i < processorCount; i++)
            {
                tasks[i] = Task.Run(() => Method(Container));
            }

            ManualResetEventSlim.Set();
            Task.WaitAll(tasks);

            var results = tasks.Select(t => t.Result).ToArray();
            foreach (var result in results)
            {
                Console.WriteLine(result.Guid);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}