namespace Binateq.CommandLine
{
    using System;

    public abstract class TypedCommandParser : CommandLineParser
    {
        private readonly Type _commandType;

        protected TypedCommandParser(Type commandType)
        {
            _commandType = commandType;
        }

        protected internal virtual TCommandInterface CreateCommand<TCommandInterface>(Func<Type, object> resolve)
            where TCommandInterface : class
        {
            return (TCommandInterface)resolve(_commandType);
        }

        protected internal virtual string GetNormalizedCommandName(Func<string, string> normalizeCommandName)
        {
            return normalizeCommandName(_commandType.Name);
        }
    }
}