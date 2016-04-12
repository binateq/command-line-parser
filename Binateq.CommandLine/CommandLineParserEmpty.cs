namespace Binateq.CommandLine
{
    /// <summary>
    /// Implements parser that applies empty command lines.
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    public class CommandLineParserEmpty<TCommand> : CommandLineParserDefault<TCommand>
    {
        protected internal override bool TryApply<TCommandInterface>(string[] args, CommandLineParseOptions options, out TCommandInterface result)
        {
            result = default(TCommandInterface);

            if (!args.IsNullOrEmpty())
                return false;

            result = CreateCommand<TCommandInterface>(options.Resolve);
            return true;
        }
    }
}