using System;

namespace Binateq.CommandLine
{
    public class CommandPattern
    {
        private readonly CommandLineParseOptions _options;

        public CommandPattern()
            : this(new CommandLineParseOptions())
        {
        }

        public CommandPattern(CommandLineParseOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        public TCommandInterface Resolve<TCommandInterface>(Type type)
            where TCommandInterface : class
        {
            return (TCommandInterface)_options.Resolve(type);
        }

        public string NormalizeName(string name)
        {
            return _options.NormalizeName(name);
        }

        public CommandLineParserOfType<TCommand> OfType<TCommand>()
        {
            return new CommandLineParserOfType<TCommand>(this);
        }

        public CommandLineParserDefault<TCommand> Default<TCommand>()
        {
            return new CommandLineParserDefault<TCommand>(this);
        }

        public CommandLineParserEmpty<TCommand> Empty<TCommand>()
        {
            return new CommandLineParserEmpty<TCommand>(this);
        }
    }
}
