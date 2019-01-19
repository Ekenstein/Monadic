using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

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

        private static IEnumerable<(Error, Error, bool)> EqualityTestCases()
        {
            yield return (new Error("code1", "desc1"), new Error("code1", "desc1"), true);
            yield return (new Error("code1", "desc1"), new Error("code1", "desc2"), false);
            yield return (new Error("code1", "desc1"), new Error("code2", "desc2"), false);
            yield return (new Error("code1", "desc1"), new Error("code2", "desc1"), false);
        }

        [TestCaseSource(nameof(EqualityTestCases))]
        public void TestEquals((Error, Error, bool) testCase)
        {
            var result = testCase.Item1.Equals(testCase.Item2);
            Assert.AreEqual(testCase.Item3, result);
        }
    }
}
