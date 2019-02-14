using Monadicsh.Extensions;
using NUnit.Framework;
using System.Linq;

namespace Monadicsh.Tests
{
    public class NullableExtensionsTest
    {
        [Test]
        public void TestAsMaybeNonNull()
        {
            var value = (int?)1;
            var result = value.AsMaybe();
            result.AssertJust(1);
            Assert.AreEqual(value, result.AsNullable());
        }

        [Test]
        public void TestAsMaybeNull()
        {
            var value = default(int?);
            var result = value.AsMaybe();
            result.AssertNothing();
            Assert.AreEqual(value, result.AsNullable());
        }

        [Test]
        public void TestAsEnumerableNonNull()
        {
            var value = (int?)1;
            var result = value.AsEnumerable().ToArray();
            Assert.AreEqual(1, result.Length);
            var actual = result.Single();
            Assert.AreEqual(value.Value, actual);
        }

        [Test]
        public void TestAsEnumerableNull()
        {
            var value = default(int?);
            var result = value.AsEnumerable();
            Assert.IsEmpty(result);
        }
    }
}
