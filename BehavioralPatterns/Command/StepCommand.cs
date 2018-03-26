namespace Command
{
    /// <summary> Command </summary>
    class StepCommand : IStepCommand<StepArgument>
    {
        public StepCommand(IStepMaker stepMaker, StepArgument argument)
        {
            StepMaker = stepMaker;
            Argument = argument;
        }

        public StepArgument Argument { get; }
        public IStepMaker StepMaker { get; }

        public void Execute()
        {
            StepMaker.MakeStep(Argument);
        }
    }
}