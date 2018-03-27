﻿using System;

namespace Command
{
    /// <summary> Reciever </summary>
    internal class StepMaker : IStepMaker
    {
        public void MakeStep(StepArgument argument)
        {
            var id = GetHashCode();
            Console.WriteLine($"#{id} made step {argument.Direction} on {argument.Distance} positions.");
        }
    }
}