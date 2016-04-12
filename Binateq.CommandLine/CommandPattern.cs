namespace Binateq.CommandLine
{
    public static class CommandPattern
    {
        public static CommandLineParserOfType<TCommand> OfType<TCommand>()
        {
            return new CommandLineParserOfType<TCommand>();
        }

        public static CommandLineParserDefault<TCommand> Default<TCommand>()
        {
            return new CommandLineParserDefault<TCommand>();
        }

        public static CommandLineParserEmpty<TCommand> Empty<TCommand>()
        {
            return new CommandLineParserEmpty<TCommand>();
        }
    }
}
