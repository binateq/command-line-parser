using System.Collections.Generic;

namespace CommandLineParser.Tests
{
    public class TestCommandParser<TCommand> : CommandParser<TCommand>
    {
        public bool TryParseResult { get; set; }

        protected internal override bool TryParse(string[] args, CommandParserSettings settings, out TCommand result)
        {
            result = default(TCommand);

            return TryParseResult;
        }
    }
}