using NUnit.Framework;
using System;

namespace Monadic.Tests
{
    public class ErrorTests
    {
        [TestCase("", "")]
        [TestCase("", default(string))]
        [TestCase(default(string), "")]
        [TestCase(default(string), default(string))]
        [TestCase("", " ")]
        [TestCase(" ", " ")]
        [TestCase(" ", "")]
        [TestCase("test", default(string))]
        [TestCase(default(string), "test")]
        [TestCase("", "test")]
        [TestCase("test", "")]
        [TestCase("test", " ")]
        [TestCase(" ", "test")]
        public void TestConstructorArgumentException(string code, string description)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var instance = new Error(code, description);
            });
        }

        [Test]
        public void TestCode()
        {
            const string code = "test";
            var instance = new Error(code, "desc");
            Assert.AreEqual(code, instance.Code);
        }

        [Test]
        public void TestDescription()
        {
            const string description = "description";
            var instance = new Error("code", description);
            Assert.AreEqual(description, instance.Description);
        }
    }
}
