namespace Binateq.CommandLine
{
    using System;

    public abstract class CommandLineParser
    {
        public virtual TCommandInterface Apply<TCommandInterface>(string[] args)
            where TCommandInterface : class
        {
            TCommandInterface commandInterface;
            if (TryApply(args, out commandInterface))
                return commandInterface;

            throw new FormatException("Can't recognize command line arguments.");
        }

        protected internal abstract bool TryApply<TCommandInterface>(string[] args, out TCommandInterface result)
            where TCommandInterface : class;

        public static CommandLineParser operator |(CommandLineParser left, CommandLineParser right)
        {
            return new CommandLineParserOr(left, right);
        }
    }
}
