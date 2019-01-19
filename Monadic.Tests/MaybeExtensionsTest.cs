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
        public void TestCoalesceTypeInferenceJust()
        {
            var instance = Maybe.Just(1);
            var result = instance.Coalesce(v => v + 1);
            Assert.True(result.IsJust);
            Assert.AreEqual(2, result.Value);

            var instance2 = Maybe.Just(new TestRef(1));
            var result2 = instance2.Coalesce(v => v.Value + 1);
            Assert.True(result2.IsJust);
            Assert.AreEqual(2, result2.Value);

            var result3 = instance2.Coalesce(v => default(TestRef));
            Assert.False(result3.IsJust);
            Assert.True(result3.IsNothing);
        }

        private class TestRef
        {
            public TestRef(int value)
            {
                Value = value;
            }

            public int Value { get; }
        }

        private class TestRef2
        {
        }
    }
}
