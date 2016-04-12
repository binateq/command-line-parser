namespace Binateq.CommandLine.Demo
{
    using System;

    public class HelpCommand : ICommand
    {
        public string Command { get; set; }

        public bool IsVerbose { get; set; }

        public void Run()
        {
            if (string.IsNullOrWhiteSpace(Command))
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("  add");
                Console.WriteLine("  sub");
                Console.WriteLine("  help");
                Console.WriteLine();
                Console.WriteLine("Binateq.CommandLine.Demo help <command> to get help for specified <command>.");
            }
        }
    }
}