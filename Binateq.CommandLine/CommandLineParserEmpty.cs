namespace Binateq.CommandLine
{
    /// <summary>
    /// Implements parser that applies empty command lines.
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    public class CommandLineParserEmpty<TCommand> : CommandLineParserDefault<TCommand>
    {
        public CommandLineParserEmpty(CommandPattern pattern)
            : base(pattern)
        {
        }

        protected internal override bool TryApply<TCommandInterface>(string[] args, out TCommandInterface result)
        {
            result = default(TCommandInterface);

            if (!args.IsNullOrEmpty())
                return false;

            result = CreateCommand<TCommandInterface>();
            return true;
        }
    }
}