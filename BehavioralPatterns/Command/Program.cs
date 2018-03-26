using System.Collections.Generic;

namespace Command
{
    internal class Program
    {
        private static StepCommand CreateCommand(IStepMaker stepMaker, EDirection direction, int distance)
        {
            return  new StepCommand(stepMaker, new StepArgument()
            {
                Direction = direction,
                Distance = distance
            });
        }
        
        public static void Main(string[] args)
        {
            var scenario = new List<StepCommand>();
            var gamer1 = new StepMaker();
            var gamer2 = new StepMaker();
            var gamer3 = new StepMaker();
            var gamer4 = new StepMaker();
            
            scenario.Add(CreateCommand(gamer1, EDirection.Down, 5));
            scenario.Add(CreateCommand(gamer2, EDirection.Left, 3));
            scenario.Add(CreateCommand(gamer3, EDirection.Right, 4));
            scenario.Add(CreateCommand(gamer4, EDirection.Up, 1));
            scenario.Add(CreateCommand(gamer1, EDirection.Left, 7));
            scenario.Add(CreateCommand(gamer2, EDirection.Right, 2));

            // Invoker
            foreach (var stepCommand in scenario)
            {
                stepCommand.Execute();
            }
        }
    }
}