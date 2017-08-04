using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    internal partial class Program
    {
        /// <summary>
        ///     ������ �������� ������� � ��� ������, ��� ����� - ����� ������� �� ����, � ������ �� �������
        /// </summary>
        private static void Example1()
        {
            var task = new Task(Start, 182);
            task.Start(TaskScheduler.Current);
            // Task.Run(()=> {}) ������, �� ���� ������ � .NET 4.5
            Thread.Sleep(300);

            // output: Method Program.Start() got 182 as parameter
            //         Method Program.Start() is running!
        }
    }
}