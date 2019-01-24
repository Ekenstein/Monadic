using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Monadicsh.Extensions;
using NUnit.Framework;

namespace Monadicsh.Tests
{
    public class MaybeExtensionsTest
    {
        [Test]
        public void TestJustMixed()
        {
            var maybes = new[]
            {
                Maybe.Create(1),
                Maybe<int>.Nothing
            };

            var result = maybes.Just().ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.Contains(1, result);
        }

        [Test]
        public void TestJustAllNothing()
        {
            var maybes = new[]
            {
                Maybe<int>.Nothing,
                Maybe<int>.Nothing
            };

            var result = maybes.Just().ToArray();
            Assert.IsEmpty(result);
        }

        [Test]
        public void TestJustAllJust()
        {
            var maybes = new[]
            {
                Maybe.Create(1),
                Maybe.Create(2)
            };

            var result = maybes.Just().ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
        }

        [Test]
        public void TestJustEmpty()
        {
            var maybes = new Maybe<int>[0];
            var result = maybes.Just().ToArray();
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
            result.AssertJust(2);
        }

        [Test]
        public void TestCoalesceNothing()
        {
            var instance = Maybe<int>.Nothing;
            var result = instance.Coalesce(v => Maybe.Just(v + 1));
            result.AssertNothing();
        }

        [Test]
        public void TestCoalesceTypeInferenceJustValueType()
        {
            var instance = Maybe.Just(1);
            var result = instance.Coalesce(v => v + 1);
            result.AssertJust(2);
        }

        [Test]
        public void TestCoalesceTypeInferenceJustReferenceType()
        {
            var instance = Maybe.Just(new TestRef(1));
            var result = instance.Coalesce(v => v.Value);
            result.AssertJust(1);

            var result2 = instance.Coalesce(v => default(TestRef));
            result2.AssertNothing();
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
        public void TestMap()
        {
            var instance = Maybe.Just(1);
            var value = instance.Map(TimeSpan.FromHours(0), v => TimeSpan.FromHours(v));
            Assert.AreEqual(TimeSpan.FromHours(1), value);

            var value2 = instance.Map(0, v => v + 1);
            Assert.AreEqual(2, value2);
        }

        [Test]
        public void TestMapDefaultValue()
        {
            var instance = Maybe<int>.Nothing;
            var value = instance.Map(TimeSpan.FromHours(0), v => TimeSpan.FromHours(v));
            Assert.AreEqual(TimeSpan.FromHours(0), value);

            var value2 = instance.Map(0, v => v + 1);
            Assert.AreEqual(0, value2);
        }

        [Test]
        public void TestMapFunc()
        {
            var instance = Maybe.Just(1);
            var value = instance.Map(() => TimeSpan.FromHours(0), v => TimeSpan.FromHours(v));
            Assert.AreEqual(TimeSpan.FromHours(1), value);

            var value2 = instance.Map(() => 0, v => v + 1);
            Assert.AreEqual(2, value2);
        }

        [Test]
        public void TestMapFuncDefaultValue()
        {
            var instance = Maybe<int>.Nothing;
            var value = instance.Map(() => TimeSpan.FromHours(0), v => TimeSpan.FromHours(v));
            Assert.AreEqual(TimeSpan.FromHours(0), value);

            var value2 = instance.Map(0, v => v + 1);
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
            result.AssertNothing();
        }

        [Test]
        public void TestFlattenInnerNothing()
        {
            var instance = Maybe<Maybe<int>>.Just(Maybe<int>.Nothing);
            var result = instance.Flatten();
            result.AssertNothing();
        }

        [Test]
        public void TestFlattenInnerJust()
        {
            var instance = Maybe<Maybe<int>>.Just(Maybe.Create(1));
            var result = instance.Flatten();
            result.AssertJust(1);
        }

        [Test]
        public void TestIs()
        {
            var instance = Maybe.Just(1);
            Assert.True(instance.Is(1));
            Assert.False(instance.Is(2));

            Assert.True(instance.Is(() => 1));
            Assert.False(instance.Is(() => 2));
           
            Assert.Throws<ArgumentNullException>(() => instance.Is(default(Func<int>)));

            instance = Maybe<int>.Nothing;
            Assert.False(instance.Is(1));
            Assert.False(instance.Is(() => 1));

            var instance2 = Maybe<string>.Just("test");
            Assert.False(instance2.Is(default(string)));
            Assert.False(instance2.Is(() => default(string)));
            Assert.True(instance2.Is(() => "test"));
            Assert.True(instance2.Is("test"));
        }

        [Test]
        public void TestIsEqualityComparer()
        {
            var instance = Maybe.Just("test");
            var equalityComparer = new TestEqualityComparer();
            Assert.True(instance.Is("test", equalityComparer));
            Assert.False(instance.Is("notTest", equalityComparer));

            Assert.Throws<ArgumentNullException>(() => instance.Is("test", null));
            Assert.Throws<ArgumentNullException>(() => instance.Is(default(Func<string>), equalityComparer));
            Assert.Throws<ArgumentNullException>(() => instance.Is(default(Func<string>), null));

            Assert.True(instance.Is(() => "test", equalityComparer));
            Assert.False(instance.Is(() => "notTest", equalityComparer));
        }

        private class TestEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y) => string.Equals(x, y);

            public int GetHashCode(string obj) => obj?.GetHashCode() ?? 0;
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
