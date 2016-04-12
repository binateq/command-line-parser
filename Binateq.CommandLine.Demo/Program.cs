namespace Binateq.CommandLine.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var pattern = new CommandPattern();
            var parser = pattern.OfType<AddCommand>()
                                .WithRequired(c => c.A)
                                .WithRequired(c => c.B)

                       | pattern.OfType<SubCommand>()
                                .WithRequired(c => c.A)
                                .WithRequired(c => c.B)

                       | pattern.OfType<HelpCommand>()
                                .WithRequired(c => c.Command)

                       | pattern.Empty<HelpCommand>()
                                .WithProperty(c => c.Command, string.Empty)

                       | pattern.Default<InvalidCommand>();

            var command = parser.Apply<ICommand>(args);

            command.Run();
        }
    }
}
