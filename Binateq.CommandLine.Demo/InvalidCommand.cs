namespace Binateq.CommandLine.Demo
{
    using System;

    public class InvalidCommand : ICommand
    {
        public void Run()
        {
            Console.Error.WriteLine("Unrecognized command line.");
            Console.Error.WriteLine("Run the program without any parameters to get help.");
        }
    }
}