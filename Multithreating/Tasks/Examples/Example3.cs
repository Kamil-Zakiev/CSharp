using System;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks
{
    internal partial class Program
    {
        /// <summary>
        ///     ������ ������������� ���������� � ������� � ��� ���������
        /// </summary>
        private static void Example3()
        {
            var task = new Task<int>(RaiseException, 182);
            task.Start(TaskScheduler.Current);

            // task.Wait(); - �������������, �.�. task.Result �������� Wait()
            try
            {
                Console.WriteLine(task.Result);
            }
            catch (AggregateException aggregateException)
            {
                var message = string.Join(", ", aggregateException.InnerExceptions.Select(e => e.Message));
                Console.WriteLine(message);
                throw;
            }

            // output: Method Program.RaiseException() got 182 as parameter
            //         ��� �� ����� �� ���........

            //         �������������� ����������: System.AggregateException: 
            //                             ��������� ���� ��� ��������� ������. --->System.ArgumentException: ��� �� ����� �� ���........
        }
    }
}