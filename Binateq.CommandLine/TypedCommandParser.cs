namespace Binateq.CommandLine
{
    using System;

    public abstract class TypedCommandParser : CommandLineParser
    {
        private readonly Type _commandType;
        private readonly CommandPattern _pattern;

        protected TypedCommandParser(Type commandType, CommandPattern pattern)
        {
            _commandType = commandType;
            _pattern = pattern;
        }

        protected internal virtual TCommandInterface CreateCommand<TCommandInterface>()
            where TCommandInterface : class => _pattern.Resolve<TCommandInterface>(_commandType);

        protected internal virtual string NormalizedCommandName => _pattern.NormalizeName(_commandType.Name);
    }
}