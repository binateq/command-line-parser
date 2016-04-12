namespace CommandLineParser.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_WithNullFirst_ThrowsArgumentNullException()
        {
            Func<string, string> first = null;
            Func<string, string> second = (str) => str;

            var actual = first.Then(second);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_WithNullSecond_ThrowsArgumentNullException()
        {
            Func<string, string> first = (str) => str;
            Func<string, string> second = null;

            var actual = first.Then(second);
        }

        [TestMethod]
        public void Then_WithTwoFunctions_CallsBoth()
        {
            Func<string, string> first = (str) => str.Replace("aaa", "bbb");
            Func<string, string> second = (str) => str.Replace("bbb", "ccc");

            var third = first.Then(second);
            var actual = third("aaabbbccc");

            Assert.AreEqual("ccccccccc", actual);
        }
    }
}
