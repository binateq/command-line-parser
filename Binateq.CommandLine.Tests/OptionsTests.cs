namespace Binatec.CommandLine.Tests
{
    using Binateq.CommandLine;
    using Xunit;

    public class OptionsTests
    {
        [Fact]
        public void TryParseOption_WithNamedArgument_ReturnsTrue()
        {
            (string, string, string) option;

            var actual = Options.TryParseAsOption(new[] { "-" }, new[] { ':' }, "-named-argument:100", out option);

            Assert.True(actual);
        }

        [Fact]
        public void TryParseOption_WithNamedArgument_FillsPrefix()
        {
            (string actual, string, string) option;

            Options.TryParseAsOption(new[] { "-" }, new[] { ':' }, "-named-argument:100", out option);

            Assert.Equal("-", option.actual);
        }

        [Fact]
        public void TryParseOption_WithNamedArgument_FillsName()
        {
            (string, string actual, string) option;

            Options.TryParseAsOption(new[] { "-" }, new[] { ':' }, "-named-argument:100", out option);

            Assert.Equal("named-argument", option.actual);
        }

        [Fact]
        public void TryParseOption_WithNamedArgument_FillsValue()
        {
            (string, string, string actual) option;

            var actual = Options.TryParseAsOption(new[] { "-" }, new[] { ':' }, "-named-argument:100", out option);

            Assert.Equal("100", option.actual);
        }

        [Fact]
        public void TryParseOption_WithNonamedArgument_ReturnsFalse()
        {
            (string, string, string) option;

            var actual = Options.TryParseAsOption(new[] { "-" }, new[] { ':' }, "nonamed-argument", out option);

            Assert.False(actual);
        }
    }
}
