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
        /// Gets method to normalize command and parameters names.
        /// </summary>
        public Func<string, string> NormalizeName { get; }

        /// <summary>
        /// Initializes new instance of <see cref="CommandLineParseOptions"/> with specified parameters.
        /// </summary>
        /// <param name="resolve">Resolve method.</param>
        /// <param name="normalizeName">Normalize command name method.</param>
        public CommandLineParseOptions(Func<Type, object> resolve = null,
                                       Func<string, string> normalizeName = null)
        {
            Resolve = resolve ?? Activator.CreateInstance;

            NormalizeName = normalizeName ?? Helper.RemoveSuffix("Command")
                                                   .Composite(Helper.SplitCamelCase)
                                                   .Composite(Helper.ToLower)
                                                   .Composite(Helper.JoinKebabCase);
        }
    }
}
