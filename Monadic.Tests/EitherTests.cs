using NUnit.Framework;
using System;

namespace Monadic.Tests
{
    public class EitherTests
    {
        [Test]
        public void TestLeft()
        {
            const int left = 0;
            var instance = new Either<int, string>(left);
            AssertLeft(instance, 0);

            var instance2 = new Either<int, int>(left: left);
            AssertLeft(instance2, left);
        }

        [Test]
        public void TestRight()
        {
            const string right = "test";
            var instance = new Either<int, string>(right);
            AssertRight(instance, right);

            var instance2 = new Either<string, string>(right: right);
            AssertRight(instance2, right);
        }

        [Test]
        public void TestImplicitLeft()
        {
            const int left = 1;
            Either<int, string> instance = left;
            AssertLeft(instance, left);
        }

        [Test]
        public void TestImplicitRight()
        {
            const string right = "test";
            Either<int, string> instance = right;
            AssertRight(instance, right);
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

            instance1 = new Either<int, TestRef>(null);
            Assert.AreNotEqual(instance1, instance2);

            var instance3 = new Either<TestRef, TestRef>(left: value);
            var instance4 = new Either<TestRef, TestRef>(right: value);

            Assert.AreNotEqual(instance3, instance4);
        }

        private void AssertRight<T1, T2>(Either<T1, T2> instance, T2 value)
        {
            Assert.True(instance.IsRight);
            Assert.False(instance.IsLeft);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var left = instance.Left;
            });

            Assert.AreEqual(instance.Right, value);
        }

        private void AssertLeft<T1, T2>(Either<T1, T2> instance, T1 value)
        {
            Assert.True(instance.IsLeft);
            Assert.False(instance.IsRight);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var right = instance.Right;
            });
            
            Assert.AreEqual(instance.Left, value);
        }

        private class TestRef { }
    }
}
