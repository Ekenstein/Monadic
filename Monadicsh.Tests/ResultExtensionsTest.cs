using System;
using Monadicsh.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh.Tests
{
    public class ResultExtensionsTest
    {
        private static IEnumerable<(Result, Result)> ResultsThatAreAllFailed()
        {
            yield return (Result.Failed(), Result.Failed());
            yield return (Result.Failed(new Error("test1", "test1")), Result.Failed(new Error("test2", "test2")));
            yield return (Result.Failed(new Error("test1", "test1")), Result.Failed());
            yield return (Result.Failed(), Result.Failed(new Error("test2", "test2")));
        }

        private static IEnumerable<(Result, Result)> ResultThatAreSuccessOrMixed()
        {
            yield return (Result.Failed(), Result.Success);
            yield return (Result.Failed(new Error("test1", "test1")), Result.Success);
            yield return (Result.Success, Result.Success);
        }

        private static IEnumerable<(Result, Result)> ResultsThatAreMixed()
        {
            yield return (Result.Failed(), Result.Success);
            yield return (Result.Failed(new Error("test1", "test1")), Result.Success);
        }

        private static IEnumerable<(Result, Result)> ResultsThatAreAllSuccessful()
        {
            yield return (Result.Success, Result.Success);
        }

        private static void AssertFailed((Result, Result) values, Result result)
        {
            var expectedErrors = values.Item1.Errors.Union(values.Item2.Errors);
            Assert.False(result.Succeeded);
            Assert.IsNotNull(result.Errors);
            Assert.That(expectedErrors, Is.EquivalentTo(result.Errors));
        }

        private static void AssertSuccess(Result result)
        {
            Assert.True(result.Succeeded);
            Assert.IsNotNull(result.Errors);
            Assert.IsEmpty(result.Errors);
        }

        [TestCaseSource(nameof(ResultsThatAreAllFailed))]
        public void TestOrFailed((Result, Result) testCase)
        {
            var result = testCase.Item1.Or(testCase.Item2);
            AssertFailed(testCase, result);

            result = testCase.Item1.Or(testCase.Item2);
            AssertFailed(testCase, result);

            result = testCase.Item1.Or(testCase.Item2);
            AssertFailed(testCase, result);
        }

        [TestCaseSource(nameof(ResultThatAreSuccessOrMixed))]
        public void TestOrSuccess((Result, Result) testCase)
        {
            var result = testCase.Item1.Or(testCase.Item2);
            AssertSuccess(result);

            result = testCase.Item2.Or(testCase.Item1);
            AssertSuccess(result);
        }

        [TestCaseSource(nameof(ResultsThatAreMixed))]
        public void TestAndFailed((Result, Result) testCase)
        {
            var result = testCase.Item1.And(testCase.Item2);
            AssertFailed(testCase, result);

            result = testCase.Item2.And(testCase.Item1);
            AssertFailed(testCase, result);
        }

        [TestCaseSource(nameof(ResultsThatAreAllSuccessful))]
        public void TestAndSuccess((Result, Result) testCase)
        {
            var result = testCase.Item1.And(testCase.Item2);
            AssertSuccess(result);

            result = testCase.Item2.And(testCase.Item1);
            AssertSuccess(result);
        }

        [Test]
        public void TestThrowIfUnsuccessful()
        {
            var instance = Result.Success;
            Assert.DoesNotThrow(() =>
            {
                instance.ThrowIfUnsuccessful(e => new Exception());
            });

            instance = Result.Failed();
            Assert.Throws<Exception>(() =>
            {
                instance.ThrowIfUnsuccessful(e =>
                {
                    Assert.IsEmpty(e);
                    return new Exception();
                });
            });

            var error = new Error("test", "test");
            instance = Result.Failed(error);

            Assert.Throws<Exception>(() =>
            {
                instance.ThrowIfUnsuccessful(e =>
                {
                    Assert.Contains(error, e.ToArray());
                    return new Exception();
                });
            });
        }

        [Test]
        public void TestOrThrowSuccess()
        {
            const int value = 1;
            var instance = Result.Create(value);
            Assert.DoesNotThrow(() =>
            {
                var result = instance.OrThrow(e => throw new Exception());
                Assert.AreEqual(value, result);
            });
        }

        [Test]
        public void TestOrThrowFailed()
        {
            var error = new Error("test", "test");
            var instance = Result<int>.Failed(error);
            Assert.Throws<Exception>(() =>
            {
                var result = instance.OrThrow(e =>
                {
                    Assert.Contains(error, e.ToArray());
                    return new Exception();
                });
            });
        }
    }
}
