using NUnit.Framework;
using System;

namespace Monadicsh.Tests
{
    public class EitherTests
    {
        [Test]
        public void TestRightNull()
        {
            TestRef value = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new Either<string, TestRef>(value);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new Either<string, string>(right: null);
            });
        }

        [Test]
        public void TestLeftNull()
        {
            TestRef value = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new Either<TestRef, string>(value);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new Either<string, string>(left: null);
            });
        }

        [Test]
        public void TestLeft()
        {
            const int left = 0;
            var instance = new Either<int, string>(left);
            instance.AssertLeft(0);

            var instance2 = new Either<int, int>(left: left);
            instance.AssertLeft(left);
        }

        [Test]
        public void TestRight()
        {
            const string right = "test";
            var instance = new Either<int, string>(right);
            instance.AssertRight(right);

            var instance2 = new Either<string, string>(right: right);
            instance.AssertRight(right);
        }

        [Test]
        public void TestImplicitLeft()
        {
            const int left = 1;
            Either<int, string> instance = left;
            instance.AssertLeft(left);
        }

        [Test]
        public void TestImplicitRight()
        {
            const string right = "test";
            Either<int, string> instance = right;
            instance.AssertRight(right);
        }
        
        [Test]
        public void TestEquals()
        {
            var instance1 = new Either<int, string>(0);
            var instance2 = new Either<int, string>(0);

            Assert.AreEqual(instance1, instance2);

            instance1 = new Either<int, string>(1);
            Assert.AreNotEqual(instance1, instance2);

            Assert.AreEqual(instance1, instance1);

            instance1 = new Either<int, string>("test");
            Assert.AreNotEqual(instance1, instance2);

            instance2 = new Either<int, string>("test");
            Assert.AreEqual(instance1, instance2);

            var instance3 = new Either<int, int>(left: 0);
            var instance4 = new Either<int, int>(right: 0);

            Assert.AreNotEqual(instance3, instance4);
        }

        [Test]
        public void TestEqualsReference()
        {
            var value = new TestRef();
            var instance1 = new Either<int, TestRef>(value);
            var instance2 = new Either<int, TestRef>(value);

            Assert.AreEqual(instance1, instance2);

            var value2 = new TestRef();
            instance1 = new Either<int, TestRef>(value2);
            Assert.AreNotEqual(instance1, instance2);

            var instance3 = new Either<TestRef, TestRef>(left: value);
            var instance4 = new Either<TestRef, TestRef>(right: value);

            Assert.AreNotEqual(instance3, instance4);
        }

        [Test]
        public void TestCastLeft()
        {
            {
                var instance = new Either<int, string>(1);
                var result = instance.CastLeft<int?>();
                result.AssertLeft(1);
            }
            {
                var instance = new Either<int, string>("test");
                var result = instance.CastLeft<int?>();
                result.AssertRight("test");
            }
            {
                Assert.Throws<InvalidCastException>(() => new Either<string, int>("test").CastLeft<decimal>());
            }
        }

        [Test]
        public void TestCastRight()
        {
            {
                var instance = new Either<string, int>(1);
                var result = instance.CastRight<int?>();
                result.AssertRight(1);
            }
            {
                var instance = new Either<string, int>("test");
                var result = instance.CastRight<int?>();
                result.AssertLeft("test");
            }
            {
                Assert.Throws<InvalidCastException>(() => new Either<int, string>("test").CastRight<decimal>());
            }
        }

        private class TestRef { }
    }
}
