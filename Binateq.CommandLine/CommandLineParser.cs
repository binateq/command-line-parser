namespace Binateq.CommandLine
{
    using System;

    public abstract class CommandLineParser
    {
        public virtual TCommandInterface Apply<TCommandInterface>(string[] args)
            where TCommandInterface : class
        {
            var defaultOptions = new CommandLineParseOptions();

            return Apply<TCommandInterface>(args, defaultOptions);
        }

        public virtual TCommandInterface Apply<TCommandInterface>(string[] args, CommandLineParseOptions options)
            where TCommandInterface : class
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            TCommandInterface commandInterface;
            if (TryApply(args, options, out commandInterface))
                return commandInterface;

            throw new FormatException("Can't recognize command line arguments.");
        }

        protected internal abstract bool TryApply<TCommandInterface>(string[] args, CommandLineParseOptions options, out TCommandInterface result)
            where TCommandInterface : class;

        public static CommandLineParser operator |(CommandLineParser left, CommandLineParser right)
        {
            return new CommandLineParserOr(left, right);
        }
    }
}
