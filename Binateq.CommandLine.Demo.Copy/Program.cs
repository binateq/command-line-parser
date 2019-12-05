namespace Binateq.CommandLine.Demo.Copy
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var parser = Parser.Simple<Command>()
                               .Named(x => x.IsHelp, "-h", "--help")
                               .Named(x => x.IsForce, "-f", "--force")
                               .Nonamed(x => x.Files);

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
