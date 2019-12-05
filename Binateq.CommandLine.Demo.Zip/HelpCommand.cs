namespace Binateq.CommandLine.Demo.Zip
{
    using System;
    using System.Collections.Generic;

    class HelpCommand : ICommand
    {
        public IReadOnlyList<string> Commands { get; set; }

        public void Run()
        {
            if (Commands.Count == 0)
                PrintHelp();
            else
                PrintCommand(Commands[0]);
        }

        private void PrintCommand(string command)
        {
            switch (command)
            {
                case "add":
                    PrintAdd();
                    break;

                case "extract":
                    PrintExtract();
                    break;

                case "list":
                    PrintList();
                    break;

                default:
                    PrintHelp();
                    break;
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  demozip add  archive  file file...");
            Console.WriteLine("  demozip extract  archive");
            Console.WriteLine("  demozip list  archive");
            Console.WriteLine();
            Console.WriteLine("  demozip help | h");
        }

        private void PrintAdd()
        {
            Console.WriteLine("  demozip add | a  archive file file...");
        }

        private void PrintExtract()
        {
            Console.WriteLine("  demozip extract | x  archive");
        }

        private void PrintList()
        {
            Console.WriteLine("  demozip list | l  archive");
        }
    }
}
