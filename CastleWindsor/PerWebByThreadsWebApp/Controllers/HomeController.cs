using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Castle.Windsor;

namespace PerWebByThreadsWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IWindsorContainer Container { get; set; }
        
        public ActionResult Index()
        {
            Console.WriteLine(Container);
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