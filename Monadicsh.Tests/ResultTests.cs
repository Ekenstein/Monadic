using NUnit.Framework;
using System.Collections.Generic;

namespace Monadicsh.Tests
{
    public class ResultTests
    {
        [Test]
        public void TestSuccess()
        {
            var instance = Result.Success;
            instance.AssertSuccess();
        }

        [Test]
        public void TestFailedNoErrors()
        {
            var instance = Result.Failed();
            instance.AssertFailed(new Error[0]);
        }

        [Test]
        public void TestFailedOneError()
        {
            var error = new Error
            {
                Code = "test",
                Description = "testing"
            };

            var instance = Result.Failed(error);
            instance.AssertFailed(new[] { error });
        }

        [Test]
        public void TestFailedEnumerableError()
        {
            var errors = new []
            {
                new Error
                {
                    Code = "test1",
                    Description = "testdesc1"
                },
                new Error
                {
                    Code = "test2",
                    Description = "testdesc2"
                }
            };

            var instance = Result.Failed(errors);
            instance.AssertFailed(errors);
        }

        [Test]
        public void TestFailedParamsError()
        {
            var error1 = new Error
            {
                Code = "test1", 
                Description = "testdesc1"
            };

            var error2 = new Error
            {
                Code = "test2",
                Description = "testdesc2"
            };

            var instance = Result.Failed(error1, error2);
            instance.AssertFailed(new [] { error1, error2 });
        }

        [Test]
        public void TestFailedOneErrorNull()
        {
            var error1 = new Error 
            {
                Code = "test1",
                Description = "testdesc1"
            };

            var instance = Result.Failed(error1, default(Error));
            instance.AssertFailed(new [] { error1 });
        }

        [Test]
        public void TestFailedErrorsNull()
        {
            var instance = Result.Failed(null);
            instance.AssertFailed(new Error[0]);
        }

        [Test]
        public void TestFailedImplicit()
        {
            var error = new Error 
            {
                Code = "test",
                Description = "testdesc"
            };

            Result instance = error;
            instance.AssertFailed(new [] { error });
        }

        [Test]
        public void TestFailedImplicitNull()
        {
            Result instance = default(Error);
            instance.AssertFailed(new Error[0]);
        }
    }
}
