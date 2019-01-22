using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monadic.Extensions;
using NUnit.Framework;

namespace Monadic.Tests
{
    public class MaybeExtensionsTest
    {
        [Test]
        public void TestCatMaybesMixed()
        {
            var maybes = new[]
            {
                Maybe.Create(1),
                Maybe<int>.Nothing
            };

            var result = maybes.CatMaybes().ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.Contains(1, result);
        }

        [Test]
        public void TestCatMaybesAllNothing()
        {
            var maybes = new[]
            {
                Maybe<int>.Nothing,
                Maybe<int>.Nothing
            };

            var result = maybes.CatMaybes().ToArray();
            Assert.IsEmpty(result);
        }

        [Test]
        public void TestCatMaybesAllJust()
        {
            var maybes = new[]
            {
                Maybe.Create(1),
                Maybe.Create(2)
            };

            var result = maybes.CatMaybes().ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
        }

        [Test]
        public void TestCatMaybesEmpty()
        {
            var maybes = new Maybe<int>[0];
            var result = maybes.CatMaybes().ToArray();
            Assert.IsEmpty(result);
        }

        [Test]
        public void TestToNullableJust()
        {
            var instance = Maybe.Just(1);
            var result = instance.ToNullable();
            Assert.True(result.HasValue);
            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void TestToNullableNothing()
        {
            var instance = Maybe<int>.Nothing;
            var result = instance.ToNullable();
            Assert.False(result.HasValue);
        }

        [Test]
        public void TestCoalesceJust()
        {
            var instance = Maybe.Just(1);
            var result = instance.Coalesce(v => Maybe.Just(v + 1));
            Assert.True(result.IsJust);
            Assert.AreEqual(2, result.Value);
        }

        [Test]
        public void TestCoalesceNothing()
        {
            var instance = Maybe<int>.Nothing;
            var result = instance.Coalesce(v => Maybe.Just(v + 1));
            Assert.False(result.IsJust);
        }

        [Test]
        public void TestCoalesceTypeInferenceJustValueType()
        {
            var instance = Maybe.Just(1);
            var result = instance.Coalesce(v => v + 1);
            Assert.True(result.IsJust);
            Assert.AreEqual(2, result.Value);
        }

        [Test]
        public void TestCoalesceTypeInferenceJustReferenceType()
        {
            var instance = Maybe.Just(new TestRef(1));
            var result = instance.Coalesce(v => v.Value);
            Assert.True(result.IsJust);
            Assert.AreEqual(1, result.Value);

            var result2 = instance.Coalesce(v => default(TestRef));
            Assert.True(result2.IsNothing);
        }

        [Test]
        public void TestEither()
        {
            var instance = Maybe.Just(1);
            var value = instance.Either(2);
            Assert.True(value.IsLeft);
            Assert.AreEqual(1, value.Left);
        }

        [Test]
        public void TestEitherRight()
        {
            var instance = Maybe<int>.Nothing;
            var value = instance.Either(1);
            Assert.True(value.IsRight);
            Assert.AreEqual(1, value.Right);
        }

        [Test]
        public void TestEitherFunc()
        {
            var instance = Maybe.Just(1);
            var value = instance.Either(() => 2);
            Assert.True(value.IsLeft);
            Assert.AreEqual(1, value.Left);
        }

        [Test]
        public void TestEitherFuncRight()
        {
            var instance = Maybe<int>.Nothing;
            var value = instance.Either(() => 2);
            Assert.True(value.IsRight);
            Assert.AreEqual(2, value.Right);
        }

        [Test]
        public void TestOrThrow()
        {
            var instance = Maybe.Just(1);
            var value = instance.OrThrow(() => new Exception());
            Assert.AreEqual(1, value);
        }

        [Test]
        public void TestOrThrowNothing()
        {
            var instance = Maybe<int>.Nothing;
            Assert.Throws<Exception>(() => instance.OrThrow(() => new Exception()));
        }

        [Test]
        public void TestFromMaybe()
        {
            var instance = Maybe.Just(1);
            var value = instance.FromMaybe(TimeSpan.FromHours(0), v => TimeSpan.FromHours(v));
            Assert.AreEqual(TimeSpan.FromHours(1), value);

            var value2 = instance.FromMaybe(0, v => v + 1);
            Assert.AreEqual(2, value2);
        }

        [Test]
        public void TestFromMaybeDefaultValue()
        {
            var instance = Maybe<int>.Nothing;
            var value = instance.FromMaybe(TimeSpan.FromHours(0), v => TimeSpan.FromHours(v));
            Assert.AreEqual(TimeSpan.FromHours(0), value);

            var value2 = instance.FromMaybe(0, v => v + 1);
            Assert.AreEqual(0, value2);
        }

        [Test]
        public void TestOr()
        {
            var instance = Maybe.Just(1);
            var value = instance.Or(0);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void TestOrNothing()
        {
            var instance = Maybe<int>.Nothing;
            var value = instance.Or(10);
            Assert.AreEqual(10, value);
        }

        [Test]
        public void TestOrFunc()
        {
            var instance = Maybe.Just(1);
            var value = instance.Or(() => 0);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void TestOrFuncNothing()
        {
            var instance = Maybe<int>.Nothing;
            var value = instance.Or(() => 10);
            Assert.AreEqual(10, value);
        }

        [Test]
        public void TestFlattenOuterNothing()
        {
            var instance = Maybe<Maybe<int>>.Nothing;
            var result = instance.Flatten();
            Assert.True(result.IsNothing);
        }

        [Test]
        public void TestFlattenInnerNothing()
        {
            var instance = Maybe<Maybe<int>>.Just(Maybe<int>.Nothing);
            var result = instance.Flatten();
            Assert.True(result.IsNothing);
        }

        [Test]
        public void TestFlattenInnerJust()
        {
            var instance = Maybe<Maybe<int>>.Just(Maybe.Create(1));
            var result = instance.Flatten();
            Assert.True(result.IsJust);
            Assert.AreEqual(1, result.Value);
        }

        private class TestRef
        {
            public TestRef(int value)
            {
                Value = value;
            }

            public int Value { get; }
        }
    }
}
