namespace Command
{
    interface IStepCommand<TCommandArgument>
    {
        TCommandArgument Argument { get; }
        
        IStepMaker StepMaker { get; }
        
        void Execute();
    }
}