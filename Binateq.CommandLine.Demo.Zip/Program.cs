namespace Binateq.CommandLine.Demo.Zip
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var parser = Parser.Command<ICommand, AddCommand>("add", "a")
                               .Nonamed(x => x.Files)

                       | Parser.Command<ICommand, ExtractCommand>("extract", "x")
                               .Nonamed(x => x.Files)

                       | Parser.Command<ICommand, ListCommand>("list", "l")
                               .Nonamed(x => x.Files)

                       | Parser.Default<ICommand, HelpCommand>("help", "h")
                               .Nonamed(x => x.Commands);

            try
            {
                var command = parser.Parse(args);

                command.Run();
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }
    }
}
