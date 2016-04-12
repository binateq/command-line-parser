namespace CommandLineParser.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveCommandSuffix_WithNullIdentifier_ThrowsArgumentNullException()
        {
            var typeName = (string)null;

            var actual = StringHelper.RemoveCommandSuffix(typeName);
        }

        [TestMethod]
        public void RemoveCommandSuffix_WithSuffix_RemovesIt()
        {
            var typeName = "FooCommand";

            var actual = StringHelper.RemoveCommandSuffix(typeName);

            Assert.AreEqual("Foo", actual);
        }

        [TestMethod]
        public void RemoveCommandSuffix_WithLowerSuffix_DoesNotRemoveIt()
        {
            var typeName = "foocommand";

            var actual = StringHelper.RemoveCommandSuffix(typeName);

            Assert.AreEqual("foocommand", actual);
        }

        [TestMethod]
        public void RemoveCommandSuffix_WithoutSuffix_ReturnsSameValue()
        {
            var typeName = "FooBarBaz";

            var actual = StringHelper.RemoveCommandSuffix(typeName);

            Assert.AreEqual("FooBarBaz", actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToKebabCase_WithNullIdentifier_ThrowsArgumentNullException()
        {
            var typeName = (string)null;

            var actual = StringHelper.ToKebabCase(typeName);
        }

        [TestMethod]
        public void ToKebabCase_WithCamelCase_ReturnsKebabCase()
        {
            var typeName = "FooBarBaz";

            var actual = StringHelper.ToKebabCase(typeName);

            Assert.AreEqual("foo-bar-baz", actual);
        }

    }
}
