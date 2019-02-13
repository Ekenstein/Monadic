using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Monadicsh.Tests
{
    public class CaseInsensitiveTests
    {
        [TestCase(default(string))]
        [TestCase("TeSt")]
        [TestCase("")]
        [TestCase("       ")]
        public void TestOriginal(string value)
        {
            var instance = new CaseInsensitive(value);
            Assert.AreEqual(value, instance.Original);
        }

        [Test]
        public void TestDefault()
        {
            var instance = default(CaseInsensitive);
            Assert.IsNull(instance.Original);
        }

        [TestCaseSource(nameof(MixedCaseStrings))]
        public void TestCompareTo((string x, string y) testCase)
        {
            var xInstance = new CaseInsensitive(testCase.x);
            var yInstance = new CaseInsensitive(testCase.y);
            {
                var result = xInstance.CompareTo(yInstance);
                var expected = string.Compare(testCase.x, testCase.y, StringComparison.OrdinalIgnoreCase);
                Assert.AreEqual(expected, result);
            }
            {
                var result = yInstance.CompareTo(xInstance);
                var expected = string.Compare(testCase.y, testCase.x, StringComparison.OrdinalIgnoreCase);
                Assert.AreEqual(expected, result);
            }
        }

        [TestCaseSource(nameof(MixedCaseStrings))]
        public void TestEquals((string x, string y) testCase)
        {
            var xInstance = new CaseInsensitive(testCase.x);
            var yInstance = new CaseInsensitive(testCase.y);
            {
                var expected = string.Equals(testCase.x?.ToLower(), testCase.y?.ToLower());
                var result = xInstance.Equals(yInstance);
                Assert.AreEqual(expected, result);
            }
            {
                var expected = string.Equals(testCase.y?.ToLower(), testCase.x?.ToLower());
                var result = yInstance.Equals(xInstance);
                Assert.AreEqual(expected, result);
            }
            {
                var result = xInstance.Equals(xInstance);
                Assert.True(result);
            }
            {
                var result = yInstance.Equals(yInstance);
                Assert.True(result);
            }
        }

        private static IEnumerable<(string x, string y)> MixedCaseStrings()
        {
            yield return (default(string), default(string));
            yield return (default(string), "test");
            yield return ("test", default(string));
            yield return ("test", "test");
            yield return ("TeSt", "test");
            yield return ("test", "TeSt");
            yield return ("TEST", "TEST");
            yield return ("testing", "TeSt");
        }
    }
}
