using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh.Tests
{
    public static class Asserts
    {
        public static void AssertNothing<T>(this Maybe<T> instance)
        {
            Assert.True(instance.IsNothing);
            Assert.False(instance.IsJust);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var value = instance.Value;
            });
        }

        public static void AssertJust<T>(this Maybe<T> instance, T value)
        {
            Assert.False(instance.IsNothing);
            Assert.True(instance.IsJust);
            Assert.AreEqual(value, instance.Value);
        }

        public static void AssertRight<T1, T2>(this Either<T1, T2> instance, T2 value, IResolveConstraint constraint = null)
        {
            Assert.True(instance.IsRight);
            Assert.False(instance.IsLeft);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var left = instance.Left;
            });

            if (constraint == null)
            {
                Assert.AreEqual(instance.Right, value);
            }
            else
            {
                Assert.That(value, constraint);
            }
        }

        public static void AssertLeft<T1, T2>(this Either<T1, T2> instance, T1 value, Action<T1> assertActual = null)
        {
            Assert.True(instance.IsLeft);
            Assert.False(instance.IsRight);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var right = instance.Right;
            });

            if (assertActual == null)
            {
                Assert.AreEqual(instance.Left, value);
            }
            else
            {
                assertActual(instance.Left);
            }
        }

        public static void AssertSuccess(this Result instance)
        {
            Assert.True(instance.Succeeded);
            Assert.IsNotNull(instance.Errors);
            Assert.IsEmpty(instance.Errors);
        }

        public static void AssertFailed(this Result instance, IEnumerable<Error> errors)
        {
            Assert.False(instance.Succeeded);
            Assert.IsNotNull(instance.Errors);
            Assert.That(errors, Is.EquivalentTo(instance.Errors));
        }

        public static void AssertSuccess<T>(this Result<T> instance, T value)
        {
            instance.AssertRight(value);

            Assert.True(instance.Succeeded);
            Assert.IsNotNull(instance.Errors);
            Assert.IsEmpty(instance.Errors);

            Assert.False(instance.Item.IsNothing);
            Assert.True(instance.Item.IsJust);
            Assert.AreEqual(instance.Item.Value, value);
        }

        public static void AssertFailed<T>(this Result<T> instance, IEnumerable<Error> errors)
        {
            instance.AssertLeft(Result.Failed(errors.ToArray()), actual => Assert.That(actual.Errors, Is.EquivalentTo(errors)));
            instance.Left.AssertFailed(errors);
            instance.Item.AssertNothing();

            Assert.False(instance.Succeeded);

            Assert.NotNull(instance.Errors);
            Assert.That(errors, Is.EquivalentTo(instance.Errors));
            Assert.True(instance.Errors.All(e => e != null));
        }
    }
}
