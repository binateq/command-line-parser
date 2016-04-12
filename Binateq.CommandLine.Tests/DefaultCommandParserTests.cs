using System.Linq;

namespace CommandLineParser.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DefaultCommandParserTests
    {
        [TestMethod]
        public void DefaultCommandParser_WhenCalled_CreatesEmptyPropertyValuesDictionary()
        {
            var parser = new DefaultCommandParser<TestCommand>();

            Assert.AreEqual(0, parser.PropertyValues.Count);
        }

        [TestMethod]
        public void WithPropertyValue_WithSelector_IncreasesDictionary()
        {
            var parser = new DefaultCommandParser<TestCommand>();

            parser.WithPropertyValue(x => x.String, "foo");

            Assert.AreEqual(1, parser.PropertyValues.Count);
        }

        [TestMethod]
        public void WithPropertyValue_WithSelector_AppendsPropertyInfo()
        {
            var parser = new DefaultCommandParser<TestCommand>();

            parser.WithPropertyValue(x => x.String, "foo");

            var expected = typeof(TestCommand).GetProperty("String");
            var actual = parser.PropertyValues.Keys.First();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WithPropertyValue_WithSelector_AppendsValue()
        {
            var parser = new DefaultCommandParser<TestCommand>();

            parser.WithPropertyValue(x => x.String, "foo");

            var actual = parser.PropertyValues.Values.First();

            Assert.AreEqual("foo", actual);
        }

        [TestMethod]
        public void TryParse_WhenCalled_ReturnsTrue()
        {
            var parser = new DefaultCommandParser<TestCommand>();
            var settings = new CommandParserSettings();

            TestCommand command;
            var condition = parser.TryParse(null, settings, out command);

            Assert.IsTrue(condition);
        }

        [TestMethod]
        public void TryParse_WhenCalled_SetsCommand()
        {
            var parser = new DefaultCommandParser<TestCommand>();
            var settings = new CommandParserSettings();

            TestCommand command;
            var condition = parser.TryParse(null, settings, out command);

            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void TryParse_WhenCalled_SetsProperty()
        {
            var parser = new DefaultCommandParser<TestCommand>();
            var settings = new CommandParserSettings();

            parser.WithPropertyValue(x => x.String, "foo");

            TestCommand command;
            var condition = parser.TryParse(null, settings, out command);

            Assert.AreEqual("foo", command.String);
        }
    }
}
