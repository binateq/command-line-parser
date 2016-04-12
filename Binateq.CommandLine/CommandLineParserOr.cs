namespace Binateq.CommandLine
{
    using System;

    /// <summary>
    /// Implements Chain of responsibility pattern to organize <see cref="CommandLineParser">command line parsers</see> into the chain.
    /// </summary>
    public class CommandLineParserOr : CommandLineParser
    {
        private readonly CommandLineParser _left;
        private readonly CommandLineParser _right;

        /// <summary>
        /// Initializes new instance of the <see cref="CommandLineParserOr"/> type with specified left and right parsers.
        /// </summary>
        /// <param name="left">Left parser.</param>
        /// <param name="right">Right parser.</param>
        public CommandLineParserOr(CommandLineParser left, CommandLineParser right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            _left = left;
            _right = right;
        }

        /// <summary>
        /// Applies left parser, and, if it fails, applies second parser to the specified command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <param name="result">Instance of <typeparamref name="TCommandInterface"/> type, if one of two parsers was applied; otherwise <c>null</c>.</param>
        /// <returns><c>true</c>, if one of two parsers was applied; otherwise, <c>false</c>.</returns>
        protected internal override bool TryApply<TCommandInterface>(string[] args, out TCommandInterface result)
        {
            return _left.TryApply(args, out result) || _right.TryApply(args, out result);
        }
    }
}
