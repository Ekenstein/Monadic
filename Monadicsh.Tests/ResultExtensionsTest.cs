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
            yield return (Result.Failed(new Error { Code = "test1", Description = "test1" }), 
                Result.Failed(new Error 
                {
                    Code = "test2",
                    Description = "test2"
                }));
            yield return (Result.Failed(new Error 
            {
                Code = "test",
                Description = "test"
            }), Result.Failed());
            yield return (Result.Failed(), Result.Failed(new Error
            {
                Code = "test",
                Description = "test"
            }));
        }

        private static IEnumerable<(Result, Result)> ResultThatAreSuccessOrMixed()
        {
            yield return (Result.Failed(), Result.Success);
            yield return (Result.Failed(new Error 
            {
                Code = "test1",
                Description = "test1"
            }), Result.Success);
            yield return (Result.Success, Result.Success);
        }

        private static IEnumerable<(Result, Result)> ResultsThatAreMixed()
        {
            yield return (Result.Failed(), Result.Success);
            yield return (Result.Failed(new Error { Code = "test1", Description = "test1" }), Result.Success);
        }

        private static IEnumerable<(Result, Result)> ResultsThatAreAllSuccessful()
        {
            yield return (Result.Success, Result.Success);
        }

        private static void AssertFailed((Result, Result) values, Result result)
        {
            var expectedErrors = values.Item1.Errors.Union(values.Item2.Errors);
            result.AssertFailed(expectedErrors);
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
            result.AssertSuccess();

            result = testCase.Item2.Or(testCase.Item1);
            result.AssertSuccess();
        }

        [Test]
        public void TestOrFuncSuccess()
        {
            var inner = Result.Success;
            var outer = Result.Failed();

            var outer1 = outer;
            var result = inner.Or(() =>
            {
                Assert.Fail("Outer was invoked even though the inner was successful.");
                return outer1;
            });

            result.AssertSuccess();

            inner = Result.Failed();
            outer = Result.Success;

            result = inner.Or(() => outer);
            result.AssertSuccess();
        }

        [TestCaseSource(nameof(ResultsThatAreAllFailed))]
        public void TestOrFuncFailed((Result result1, Result result2) testCase)
        {
            var result = testCase.result1.Or(() => testCase.result2);
            AssertFailed(testCase, result);

            result = testCase.result2.Or(() => testCase.result1);
            AssertFailed(testCase, result);
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
            result.AssertSuccess();

            result = testCase.Item2.And(testCase.Item1);
            result.AssertSuccess();
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

            var error = new Error 
            {
                Code = "test1",
                Description = "test1"
            };
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
            var error = new Error { Code = "test", Description = "test"};
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
