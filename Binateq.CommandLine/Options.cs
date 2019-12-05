namespace Binateq.CommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Stores named and unnamed arguments of a command line.
    /// </summary>
    /// <remarks>
    /// <para>In a command line</para>
    /// <code>git commit --patch foo.c bar.h</code>
    /// <para><code>commit</code> is a command, <code>--patch</code> is the named option, and <code>foo.c</code>
    /// and <code>bar.h</code></para> are the unnamed options.
    /// </remarks>
    public class Options
    {
        /// <summary>
        /// Gets named arguments.
        /// </summary>
        public IReadOnlyDictionary<string, string> Named { get; }

        /// <summary>
        /// Gets unnamed arguments.
        /// </summary>
        public IReadOnlyList<string> Unnamed { get; }

        /// <summary>
        /// Initializes new instance of the <see cref="Options"/> type.
        /// </summary>
        public Options(Scanner scanner, string[] prefixes, char[] separators, Func<string, string> toPropertyName)
        {
            var named = new Dictionary<string, string>();
            var unnamed = new List<string>();

            while (scanner.HasNext)
            {
                if (TryParseAsOption(prefixes, separators, scanner.Current, out (string prefix, string name, string value) option))
                {
                    named.Add(option.prefix + option.name, option.value);
                    named.Add(toPropertyName(option.name), option.value);
                }
                else
                    unnamed.Add(scanner.Current);

                scanner.Next();
            }

            Named = named;
            Unnamed = unnamed;
        }

        internal static bool TryParseAsOption(string[] prefixes, char[] separators, string arg, out (string, string, string) option)
        {
            option = (null, null, null);

            var prefix = prefixes.SingleOrDefault(arg.StartsWith);
            if (prefix == null)
                return false;

            arg = arg.Substring(prefix.Length);

            var position = arg.IndexOfAny(separators);
            if (position >= 0)
                option = (prefix, arg.Substring(0, position), arg.Substring(position + 1));
            else
                option = (prefix, arg, null);

            return true;
        }
    }
}
