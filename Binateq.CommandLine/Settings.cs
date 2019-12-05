namespace Binateq.CommandLine
{
    using System;

    /// <summary>
    /// Represents settings of command line parser.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets method to resolve command instances.
        /// </summary>
        /// <remarks>
        /// <para>Used to integration with IoC.</para>
        /// </remarks>
        public Func<Type, object> Resolve { get; }

        /// <summary>
        /// Gets the method to convert the name of property to the name of option.
        /// </summary>
        public Func<string, string> ToPropertyName { get; }

        /// <summary>
        /// Gets the array of the name-value separators.
        /// </summary>
        public char[] OptionSeparators { get; }

        /// <summary>
        /// Gets the array of the option's prefixes.
        /// </summary>
        public string[] OptionPrefixes { get; }

        /// <summary>
        /// Initializes new instance of <see cref="Settings"/> with specified parameters.
        /// </summary>
        /// <param name="resolve">The method to resolve command instances.</param>
        /// <param name="toOptionName">The method to convert property name to option name..</param>
        /// <param name="optionSeparators">Options separators.</param>
        /// <param name="optionPrefixes">Option prefixes.</param>
        public Settings(Func<Type, object> resolve = null,
                        Func<string, string> toOptionName = null,
                        char[] optionSeparators = null,
                        string[] optionPrefixes = null)
        {
            Resolve = resolve ?? Activator.CreateInstance;
            ToPropertyName = toOptionName ?? RemoveHyphens;
            OptionSeparators = optionSeparators ?? new[] { ':', '=' };
            OptionPrefixes = optionPrefixes ?? new[] { "-", "/" };
        }

        internal static string RemoveHyphens(string s) => s.Replace("-", "");
    }
}
