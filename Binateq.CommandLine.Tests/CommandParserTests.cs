namespace CommandLineParser.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_WhenCantParse_ThrowsFormatException()
        {
            var parser = new TestCommandParser<TestCommand> {TryParseResult = false};
            var args = new string[0];

            var result = parser.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_WithNullArgs_ThrowsArgumentNullException()
        {
            var parser = new TestCommandParser<TestCommand> { TryParseResult = false };
            var args = (string[]) null;
            var settings = new CommandParserSettings();

            var result = parser.Parse(args, settings);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_WithNullSettings_ThrowsArgumentNullException()
        {
            var parser = new TestCommandParser<TestCommand> { TryParseResult = false };
            var args = new string[0];
            var settings = (CommandParserSettings) null;

            var result = parser.Parse(args, settings);
        }
    }
}
