namespace Binateq.CommandLine.Demo
{
    using System;

    public class SubCommand : ICommand
    {
        public double A { get; set; }

        public double B { get; set; }

        public bool IsVerbose { get; set; }

        public void Run()
        {
            if (IsVerbose)
                Console.WriteLine($"Difference {A} and {B} is {A - B}");
            else
                Console.WriteLine(A - B);
        }
    }
}