using NUnit.Framework;
using System.Collections.Generic;

namespace Monadic.Tests
{
    public class ResultTests
    {
        [Test]
        public void TestSuccess()
        {
            var instance = Result.Success;
            AssertSuccess(instance);
        }

        [Test]
        public void TestFailedNoErrors()
        {
            var instance = Result.Failed();
            AssertFailed(instance, new Error[0]);
        }

        [Test]
        public void TestFailedOneError()
        {
            var error = new Error("test", "testing");
            var instance = Result.Failed(error);
            AssertFailed(instance, new [] { error });
        }

        [Test]
        public void TestFailedEnumerableError()
        {
            var errors = new []
            {
                new Error("test1", "testdesc1"),
                new Error("test2", "testdesc2")
            };

            var instance = Result.Failed(errors);
            AssertFailed(instance, errors);
        }

        [Test]
        public void TestFailedParamsError()
        {
            var error1 = new Error("test1", "testdesc1");
            var error2 = new Error("test2", "testdesc2");

            var instance = Result.Failed(error1, error2);
            AssertFailed(instance, new [] { error1, error2});
        }

        [Test]
        public void TestFailedOneErrorNull()
        {
            var error1 = new Error("test1", "testdesc1");

            var instance = Result.Failed(error1, default(Error));
            AssertFailed(instance, new [] { error1 });
        }

        [Test]
        public void TestFailedErrorsNull()
        {
            var instance = Result.Failed(null);
            AssertFailed(instance, new Error[0]);
        }

        [Test]
        public void TestFailedImplicit()
        {
            var error = new Error("test", "testdesc");
            Result instance = error;
            AssertFailed(instance, new [] { error });
        }

        [Test]
        public void TestFailedImplicitNull()
        {
            Result instance = default(Error);
            AssertFailed(instance, new Error[0]);
        }

        private static void AssertFailed(Result instance, IEnumerable<Error> errors)
        {
            Assert.False(instance.Succeeded);
            Assert.IsNotNull(instance.Errors);
            Assert.That(errors, Is.EquivalentTo(instance.Errors));
        }

        private static void AssertSuccess(Result instance)
        {
            Assert.True(instance.Succeeded);
            Assert.IsNotNull(instance.Errors);
            Assert.IsEmpty(instance.Errors);
        }
    }
}
