namespace CommandLineParser
{
    using System;

    /// <summary>
    /// Describes abstarct command parser.
    /// </summary>
    /// <typeparam name="TCommand">The type of command.</typeparam>
    public abstract class CommandParser<TCommand>
    {
        /// <summary>
        /// Parses arguments of command line using specified settings.
        /// </summary>
        /// <param name="args">Arguments of command line.</param>
        /// <param name="settings">Settings.</param>
        /// <returns>The instance of command object.</returns>
        public TCommand Parse(string[] args, CommandParserSettings settings)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            TCommand result;
            if (TryParse(args, settings, out result))
                return result;

            throw new FormatException();
        }

        /// <summary>
        /// Parses arguments of command line.
        /// </summary>
        /// <param name="args">Arguments of command line.</param>
        /// <returns>The instance of command object.</returns>
        public TCommand Parse(string[] args)
        {
            var settings = new CommandParserSettings();

            return Parse(args, settings);
        }

        /// <summary>
        /// Parses arguments of command line using specified settings.
        /// </summary>
        /// <param name="args">Arguments of command line.</param>
        /// <param name="settings">Settings.</param>
        /// <param name="result">
        /// When this method returns, contains command instance, if the command parsed successfully.
        /// </param>
        /// <returns>
        /// <c>true</c> if arguments was parsed successfully; otherwise, <c>false</c>.
        /// </returns>
        protected internal abstract bool TryParse(string[] args, CommandParserSettings settings, out TCommand result);

        /// <summary>
        /// Combines two parsers.
        /// </summary>
        /// <param name="left">Left parser.</param>
        /// <param name="right">Right parser.</param>
        /// <returns>Combined parses that tries to apply fist left parser, and then right.</returns>
        public static CommandParser<TCommand> operator |(CommandParser<TCommand> left, CommandParser<TCommand> right)
        {
            return new CommandParserOr<TCommand>(left, right);
        }
    }
}