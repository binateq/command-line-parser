namespace Binateq.CommandLine
{
    using System;

    /// <summary>
    /// Represents options used to parse command line arguments.
    /// </summary>
    public class CommandLineParseOptions
    {
        /// <summary>
        /// Gets method to resolve command objects.
        /// </summary>
        public Func<Type, object> Resolve { get; }

        /// <summary>
        /// Gets method to normalize command names.
        /// </summary>
        public Func<string, string> NormalizeCommandName { get; }

        /// <summary>
        /// Gets string comparison.
        /// </summary>
        public StringComparison StringComparison { get; }

        /// <summary>
        /// Initializes new instance of <see cref="CommandLineParseOptions"/> with specified parameters.
        /// </summary>
        /// <param name="resolve">Resolve method.</param>
        /// <param name="normalizeCommandName">Normalize command name method.</param>
        /// <param name="stringComparison">String comparison.</param>
        public CommandLineParseOptions(Func<Type, object> resolve = null,
                                       Func<string, string> normalizeCommandName = null,
                                       StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            Resolve = resolve ?? Activator.CreateInstance;

            NormalizeCommandName = normalizeCommandName ?? Helper.RemoveSuffix("Command")
                                                                 .Composite(Helper.SplitCamelCase)
                                                                 .Composite(Helper.ToLower)
                                                                 .Composite(Helper.JoinKebabCase);

            StringComparison = stringComparison;
        }
    }
}
