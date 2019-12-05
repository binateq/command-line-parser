namespace Binateq.CommandLine.Tests
{
    using Binateq.CommandLine;
    using Xunit;

    public class ScannerTests
    {
        [Fact]
        public void HasNext_WithEmptyArray_IsFalse()
        {
            var scanner = new Scanner(new string[0]);

            Assert.False(scanner.HasNext);
        }

        [Fact]
        public void HasNext_WithNonEmptyArray_IsTrue()
        {
            var scanner = new Scanner(new[] { "foo" });

            Assert.True(scanner.HasNext);
        }

        [Fact]
        public void Current_WithEmptyArray_IsNull()
        {
            var scanner = new Scanner(new string[0]);

            Assert.Null(scanner.Current);
        }

        [Fact]
        public void Current_WithNonEmptyArray_EqualsToFirstElement()
        {
            var scanner = new Scanner(new[] { "foo" });

            Assert.Equal("foo", scanner.Current);
        }

        [Fact]
        public void Current_AfterNext_EqualsToSecondElement()
        {
            var scanner = new Scanner(new[] { "foo", "bar" });

            scanner.Next();

            Assert.Equal("bar", scanner.Current);
        }
    }
}
