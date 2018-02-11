using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class MyAsyncController : AsyncController
    {
        private readonly Guid _guid = Guid.NewGuid();
        // GET
        public async Task<ActionResult> Index()
        {
            Logger.Log(_guid);

            Console.WriteLine(CultureInfo.CurrentCulture);

            await Task.Run(() =>
            {
                Console.WriteLine(CultureInfo.CurrentCulture);
                var sec = 10;
                Thread.Sleep(sec * 1000);
                Logger.Log(_guid);
            });
            
            Console.WriteLine(CultureInfo.CurrentCulture);
            // ожидаем, что задачу будет продолжать тот поток, который начал выполнять экшен, а не тот, что закончил выполнять таск в другом потоке,
            // потому что await должен отработать с использованием контекста синхронизации
            // ожидания не оправдались, нужно более тщательное тестирование с разных браузеров
            Logger.Log(_guid);
            return View();
        }
    }
}