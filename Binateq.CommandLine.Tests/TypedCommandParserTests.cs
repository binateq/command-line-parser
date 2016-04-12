namespace CommandLineParser.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypedCommandParserTests
    {
        [TestMethod]
        public void TryParse_WithEmptyArgs_ReturnsFalse()
        {
            var parser = new TypedCommandParser<TestCommand>();
            var settings = new CommandParserSettings();
            var args = new string[0];
            TestCommand result;

            var condition = parser.TryParse(args, settings, out result);

            Assert.IsFalse(condition);
        }

        [TestMethod]
        public void TryParse_WithValidCommandName_ReturnsTrue()
        {
            var parser = new TypedCommandParser<TestCommand>();
            var settings = new CommandParserSettings();
            var args = new[] { "test" };
            TestCommand result;

            var condition = parser.TryParse(args, settings, out result);

            Assert.IsTrue(condition);
        }

        [TestMethod]
        public void TryParse_WithInvalidCommandName_ReturnsFalse()
        {
            var parser = new TypedCommandParser<TestCommand>();
            var settings = new CommandParserSettings();
            var args = new[] { "non-test" };
            TestCommand result;

            var condition = parser.TryParse(args, settings, out result);

            Assert.IsFalse(condition);
        }
    }
}
