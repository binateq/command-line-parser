namespace Binateq.CommandLine.Tests
{
    using Binateq.CommandLine;
    using Xunit;

    public class SettingsTests
    {
        [Fact]
        public void RemoveHyphens_WithArgWithHyphens_RemovesHyphens()
        {
            var actual = Settings.RemoveHyphens("foo-bar-baz");

            Assert.Equal("foobarbaz", actual);
        }

        [Fact]
        public void RemoveHyphens_WithArgWithoutHyphens_ReturnsSameArg()
        {
            var actual = Settings.RemoveHyphens("fooBarBaz");

            Assert.Equal("fooBarBaz", actual);
        }
    }
}
