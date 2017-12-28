using System.Threading;

namespace Cancellation
{
    internal partial class Program
    {
        /// <summary> Пример использования паттерна отмены операции </summary>
        private static void ThreadPoolExample1()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            ThreadPool.QueueUserWorkItem(x => { Start(123, token); });

            // подождём, пока поток выполнит какие-нибудь действия
            Thread.Sleep(2000);

            // вызываем отмену операции
            // throwOnFirstException: true - выброс исключения сразу, иначе - ошибка положится в список ошибок
            cts.Cancel(throwOnFirstException: true); 
            
            // подождём, пока поток из пула завершит свою работу
            Thread.Sleep(200);
        }
    }
}