namespace Binateq.CommandLine
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Implements helper methods to normalize command names.
    /// </summary>
    public static class Helper
    {
        private static readonly Regex CamelCaseSplitter = new Regex(@"(?<=\p{Ll})(?=\p{Lu})|(?<=\p{Lu})(?=\p{Lu}\p{Ll})");

        /// <summary>
        /// Gets delegate to remove suffix from identifier, if suffix is presents.
        /// </summary>
        /// <param name="suffix">Suffix to remove.</param>
        /// <returns>Delegate that remove suffix.</returns>
        public static Func<string, string> RemoveSuffix(string suffix)
        {
            return identifier => identifier.EndsWith(suffix, StringComparison.Ordinal)
                               ? identifier.Substring(0, identifier.Length - suffix.Length)
                               : identifier;
        }

        /// <summary>
        /// Splits specified "lowerCamelCaseIdentifier" or "UpperCamelCaseIdentifier" to array of words.
        /// </summary>
        /// <returns>Array of words.</returns>
        public static string[] SplitCamelCase(string identifier)
        {
            return CamelCaseSplitter.Split(identifier);
        }

        /// <summary>
        /// Converts words to lower case.
        /// </summary>
        /// <param name="words">Array of words.</param>
        /// <returns>Lower-cased words.</returns>
        public static string[] ToLower(string[] words)
        {
            return words.Select(x => x.ToLowerInvariant())
                        .ToArray();
        }

        /// <summary>
        /// Joins words to "kebab-case-identifier".
        /// </summary>
        /// <param name="words">Array of words.</param>
        /// <returns>Identifier.</returns>
        public static string JoinKebabCase(string[] words)
        {
            return string.Join("-", words);
        }

        /// <summary>
        /// Joins words to "lowerCamelCaseIdentifier".
        /// </summary>
        /// <param name="words">Array of words.</param>
        /// <returns>Identifier.</returns>
        public static string JoinLowerCamelCase(string[] words)
        {
            if (words.Length == 0)
                return string.Empty;

            var builder = new StringBuilder(words[0].ToLowerInvariant());

            for (int i = 1; i < words.Length; i++)
            {
                if (string.IsNullOrEmpty(words[i]))
                    continue;

                builder.Append(words[i][0]);

                for (int j = 1; j < words[i].Length; j++)
                    builder.Append(words[i][j]);
            }

            return builder.ToString();
        }
    }
}