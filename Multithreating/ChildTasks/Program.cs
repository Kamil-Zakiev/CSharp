using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChildTasks
{
    internal class Program
    {
        private static void Main()
        {
            Example3();
        }

        /// <summary> Применение задания внутри другого задания без привязывания к родительскому </summary>
        private static void Example1()
        {
            var task = new Task<int[]>(() =>
            {
                var value = new int[1];
                var childTask1 = new Task(() =>
                {
                    Thread.Sleep(500);
                    value[0] = 555;
                });

                childTask1.ContinueWith(t =>
                {
                    Thread.Sleep(500);
                    value[0] = 999;
                });

                childTask1.Start();
                return value;
            });

            task.ContinueWith(t => Console.WriteLine(t.Result[0]));
            task.Start();
            Console.WriteLine(task.Result[0]);
        }

        /// <summary> Применение задания внутри другого задания с привязыванием к родительскому </summary>
        private static void Example2()
        {
            var task = new Task<int[]>(() =>
            {
                var value = new int[1];
                var childTask1 = new Task(() =>
                {
                    Thread.Sleep(500);
                    value[0] = 555;
                }, TaskCreationOptions.AttachedToParent);

                childTask1.ContinueWith(t =>
                {
                    Thread.Sleep(500);
                    value[0] = 999;
                });

                childTask1.Start();
                return value;
            });

            task.ContinueWith(t => Console.WriteLine(t.Result[0]));
            task.Start();
            Console.WriteLine(task.Result[0]);
        }

        /// <summary> Применение задания внутри другого задания с привязыванием к родительскому + привязывание задачи-продолжения </summary>
        private static void Example3()
        {
            var task = new Task<int[]>(() =>
            {
                var value = new int[1];
                var childTask1 = new Task(() =>
                    {
                        Thread.Sleep(500);
                        value[0] = 555;
                    }, TaskCreationOptions.AttachedToParent
                );

                childTask1.ContinueWith(t =>
                    {
                        Thread.Sleep(500);
                        value[0] = 999;
                    }, TaskContinuationOptions.AttachedToParent
                );

                childTask1.Start();
                return value;
            });

            task.ContinueWith(t => Console.WriteLine(t.Result[0]));
            task.Start();
            Console.WriteLine(task.Result[0]);
        }

        /// <summary> Интересный случай, связанный с мехаизмов возврата результата значимого типа </summary>
        private static void Example4()
        {
            var value = 0;
            var task = new Task<int>(() =>
            {
                var childTask1 = new Task(() =>
                {
                    Thread.Sleep(500);
                    value = 555;
                }, TaskCreationOptions.AttachedToParent);

                childTask1.ContinueWith(t =>
                {
                    Console.WriteLine("childTask1ContinueWith execution");
                    Thread.Sleep(500);
                    value = 999;
                }, TaskContinuationOptions.AttachedToParent);

                childTask1.Start();
                return value;
            });

            task.ContinueWith(t => Console.WriteLine("ContinueWith value: " + value));
            task.ContinueWith(t => Console.WriteLine("ContinueWith result: " + t.Result));
            task.Start();
            task.Wait();
            Console.WriteLine("WriteLine result: " + task.Result);
            Console.WriteLine("WriteLine value: " + value);
        }
    }
}