namespace Binateq.CommandLine.Tests
{
    using Binateq.CommandLine;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Xunit;

    public class ParserTests
    {
        class Mock
        {
            public int Public { get; set; }

            protected int Protected { get; set; }

            public int ReadOnly { get; }

            public static int Static { get; }

            public Mock(int @protected, int @readonly)
            {
                Protected = @protected;
                ReadOnly = @readonly;
            }
        }

        [Fact]
        public void GetPublicInstanceSettableProperties_WhenCalled_ReturnsListOfValidProperties()
        {
            var actual = Parser<Mock>.GetPublicInstanceSettableProperties()
                                     .Select(x => x.Name);

            Assert.Equal(new[] { "Public" }, actual);
        }

        [Fact]
        public void GetPropertyInfo_WithSelector_ReturnsPropertyInfoWithValidName()
        {
            var info = Parser<Mock>.GetPropertyInfo(x => x.Public);

            Assert.Equal("Public", info.Name);
        }

        [Fact]
        public void GetPropertyInfo_WithMethod_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var info = Parser<Mock>.GetPropertyInfo(x => x.Public + x.ReadOnly);
            });
        }
    }
}
