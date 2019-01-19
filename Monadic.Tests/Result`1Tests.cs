using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadic.Tests
{
    public class TypeParameterizedResultTests
    {
        [Test]
        public void TestFailed()
        {
            var instance = Result<int>.Failed();
            AssertFailed(instance, new Error[0]);
        }

        [Test]
        public void TestFailedOneError()
        {
            var error = new Error("test", "testdesc");
            var instance = Result<int>.Failed(error);
            AssertFailed(instance, new [] { error });
        }

        [Test]
        public void TestFailedMultipleErrors()
        {
            var error1 = new Error("test1", "testdesc1");
            var error2 = new Error("test2", "testdesc2");

            var instance = Result<int>.Failed(error1, error2);

            AssertFailed(instance, new [] { error1, error2 });
        }

        [Test]
        public void TestFailedNull()
        {
            var instance = Result<int>.Failed(null);
            AssertFailed(instance, Enumerable.Empty<Error>());
        }

        [Test]
        public void TestFailedOnErrorNull()
        {
            var error2 = new Error("test", "testdesc");
            var instance = Result<int>.Failed(default(Error), error2);
            AssertFailed(instance, new [] { error2 });
        }

        [TestCase(1)]
        [TestCase(true)]
        [TestCase(false)]
        public void TestSuccess<T>(T value)
        {
            var instance = Result<T>.Success(value);
            AssertSuccess(instance, value);
        }

        [Test]
        public void TestSuccessNullValue()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = Result<string>.Success(default(string));
            });
        }

        [TestCase(1)]
        [TestCase(true)]
        public void TestCreate<T>(T value)
        {
            var result = Result.Create(value);
            AssertSuccess(result, value);
        }

        [Test]
        public void TestCreateNullValue()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = Result.Create(default(string));
            });
        }

        private static void AssertFailed<T>(Result<T> instance, IEnumerable<Error> errors)
        {
            Assert.True(instance.IsLeft);
            Assert.False(instance.IsRight);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = instance.Right;
            });

            var left = instance.Left;
            Assert.False(left.Succeeded);
            Assert.That(errors, Is.EquivalentTo(left.Errors));
            Assert.True(left.Errors.All(e => e != null));


            Assert.False(instance.Succeeded);
            Assert.True(instance.Item.IsNothing);
            Assert.False(instance.Item.IsJust);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = instance.Item.Value;
            });

            Assert.NotNull(instance.Errors);
            Assert.That(errors, Is.EquivalentTo(instance.Errors));
            Assert.True(instance.Errors.All(e => e != null));
        }

        private static void AssertSuccess<T>(Result<T> instance, T item)
        {
            Assert.False(instance.IsLeft);
            Assert.True(instance.IsRight);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = instance.Left;
            });

            var right = instance.Right;
            Assert.AreEqual(item, right);

            Assert.True(instance.Succeeded);
            Assert.IsNotNull(instance.Errors);
            Assert.IsEmpty(instance.Errors);

            Assert.False(instance.Item.IsNothing);
            Assert.True(instance.Item.IsJust);
            Assert.AreEqual(instance.Item.Value, item);
        }
    }
}
