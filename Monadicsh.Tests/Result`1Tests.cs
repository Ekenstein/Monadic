using Monadicsh.Extensions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Monadicsh.Tests
{
    public class TypeParameterizedResultTests
    {
        [Test]
        public void TestFailed()
        {
            var instance = Result<int>.Failed();
            instance.AssertFailed(new Error[0]);
        }

        [Test]
        public void TestFailedOneError()
        {
            var error = new Error
            {
                Code = "test",
                Description = "description"
            };

            var instance = Result<int>.Failed(error);
            instance.AssertFailed(new [] { error });
        }

        [Test]
        public void TestFailedMultipleErrors()
        {
            var error1 = new Error
            {
                Code = "test1",
                Description = "desc1"
            };

            var error2 = new Error
            {
                Code = "test2",
                Description = "desc2"
            };

            var instance = Result<int>.Failed(error1, error2);

            instance.AssertFailed(new [] { error1, error2 });
        }

        [Test]
        public void TestFailedNull()
        {
            var instance = Result<int>.Failed(null);
            instance.AssertFailed(Enumerable.Empty<Error>());
        }

        [Test]
        public void TestFailedOnErrorNull()
        {
            var error2 = new Error
            {
                Code = "test",
                Description = "testdesc"
            };
            var instance = Result<int>.Failed(default(Error), error2);
            instance.AssertFailed(new [] { error2 });
        }

        [TestCase(1)]
        [TestCase(true)]
        [TestCase(false)]
        public void TestSuccess<T>(T value)
        {
            var instance = Result<T>.Success(value);
            instance.AssertSuccess(value);
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
            result.AssertSuccess(value);
        }

        [Test]
        public void TestCreateNullValue()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = Result.Create(default(string));
            });
        }
    }
}
