using System;
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

            var result = maybes.AllJust().ToArray();
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

            var result = maybes.AllJust().ToArray();
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

            var result = maybes.AllJust().ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
        }

        [Test]
        public void TestJustEmpty()
        {
            var maybes = new Maybe<int>[0];
            var result = maybes.AllJust().ToArray();
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
        public void TestToEitherRight()
        {
            var instance = Maybe.Just(1);
            var value = instance.ToEither(2);
            value.AssertRight(1);
        }

        [Test]
        public void TestToEitherLeft()
        {
            var instance = Maybe<int>.Nothing;
            var value = instance.ToEither(1);
            value.AssertLeft(1);
        }

        [Test]
        public void TestToEitherLeftNull()
        {
            var instance = Maybe<string>.Nothing;
            Assert.Throws<ArgumentNullException>(() => instance.ToEither(default(string)));
        }
    
        [Test]
        public void TestToEitherRightNull()
        {
            var instance = Maybe.Just("test");
            Assert.Throws<ArgumentNullException>(() => instance.ToEither(default(string)));
        }

        [Test]
        public void TestToEitherFuncRight()
        {
            var instance = Maybe.Just(1);
            var value = instance.ToEither(() => 2);
            value.AssertRight(1);
        }

        [Test]
        public void TestToEitherFuncLeft()
        {
            var instance = Maybe<int>.Nothing;
            var value = instance.ToEither(() => 2);
            value.AssertLeft(2);
        }

        [Test]
        public void TestToEitherFuncLeftNull()
        {
            var instance = Maybe<string>.Nothing;
            Assert.Throws<ArgumentNullException>(() => instance.ToEither(default(Func<string>)));
            Assert.Throws<ArgumentNullException>(() => instance.ToEither(() => default(string)));
        }

        public void TestToEitherFuncRightNull()
        {
            var instance = Maybe.Just("test");
            Assert.Throws<ArgumentNullException>(() => instance.ToEither(default(Func<string>)));
            var result = instance.ToEither(() => default(string));
            result.AssertRight("test");
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
        public void TestOrThrowFuncNull()
        {
            var instance = Maybe<int>.Nothing;
            Assert.Throws<ArgumentNullException>(() => instance.OrThrow(default(Func<Exception>)));

            instance = Maybe.Just(1);
            Assert.Throws<ArgumentNullException>(() => instance.OrThrow(default(Func<Exception>)));
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

        [Test]
        public void TestToResultJust()
        {
            var instance = Maybe.Create(1);
            var result = instance.ToResult(() => new[]
            {
                new Error
                {
                    Code = "UnexpectedError",
                    Description = "Unexpected error"
                }
            });

            result.AssertSuccess(1);
        }

        [Test]
        public void TestToResultNothing()
        {
            var errors = new[]
            {
                new Error {Code = "test", Description = "testing"},
                new Error {Code = "test2", Description = "testing2"}
            };

            var instance = Maybe<int>.Nothing;
            var result = instance.ToResult(() => errors);
            result.AssertFailed(errors);
        }

        [Test]
        public void TestToResultNothingNullErrors()
        {
            var instance = Maybe<int>.Nothing;
            var result = instance.ToResult(() => null);
            result.AssertFailed(new Error[0]);
        }

        [Test]
        public void TestToResultNull()
        {
            var instance = Maybe<int>.Nothing;
            Assert.Throws<ArgumentNullException>(() => instance.ToResult(default(Func<IEnumerable<Error>>)));

            instance = Maybe.Just(1);
            Assert.Throws<ArgumentNullException>(() => instance.ToResult(default(Func<IEnumerable<Error>>)));
        }

        [Test]
        public void TestToResultParamsJust()
        {
            var instance = Maybe.Just(1);
            var result = instance.ToResult(new Error("test", "testing"), new Error("test2", "testing2"));
            result.AssertSuccess(1);
        }

        [Test]
        public void TestToResultParamsNothing()
        {
            var error = new Error("test", "testing");
            var instance = Maybe<int>.Nothing;
            var result = instance.ToResult(error);
            result.AssertFailed(new [] {error});
        }

        [Test]
        public void TestToResultParamsJustNull()
        {
            var instance = Maybe.Just(1);
            var result = instance.ToResult(default(Error[]));
            result.AssertSuccess(1);
        }

        [Test]
        public void TestToResultParamsNothingNull()
        {
            var instance = Maybe<int>.Nothing;
            var result = instance.ToResult(default(Error[]));
            result.AssertFailed(new Error[0]);
        }

        [Test]
        public void TestOrDefaultReferenceType()
        {
            var value = new TestRef(1);
            var instance = Maybe.Just(value);
            var result = instance.OrDefault();
            Assert.IsNotNull(result);
            Assert.AreEqual(value, result);
        }

        [Test]
        public void TestOrDefaultValueType()
        {
            var instance = Maybe.Just(1);
            var result = instance.OrDefault();
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestOrDefaultNothingReferenceType()
        {
            var instance = Maybe<TestRef>.Nothing;
            var result = instance.OrDefault();
            Assert.IsNull(result);
        }

        [Test]
        public void TestOrDefaultNothingValueType()
        {
            var instance = Maybe<int>.Nothing;
            var result = instance.OrDefault();
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void TestGuardPredicate()
        {
            var instance = Maybe.Just(5);
            var result = instance.Guard(v => v - 1 == 4);
            result.AssertJust(5);

            result = instance.Guard(v => v == 4);
            result.AssertNothing();

            instance = Maybe<int>.Nothing;
            result = instance.Guard(v => v == 5);
            result.AssertNothing();

            Assert.Throws<ArgumentNullException>(() => instance.Guard(default(Func<int, bool>)));
        }

        [Test]
        public void TestGuardValueWithoutEqualityComparer()
        {
            var instance = Maybe.Just("test");
            var result = instance.Guard("test");
            result.AssertJust("test");

            result = instance.Guard("notTest");
            result.AssertNothing();

            result = instance.Guard(default(string));
            result.AssertNothing();

            result = instance.Guard(() => "test");
            result.AssertJust("test");

            result = instance.Guard(() => "notTest");
            result.AssertNothing();

            result = instance.Guard(() => default(string));
            result.AssertNothing();

            Assert.Throws<ArgumentNullException>(() => instance.Guard(default(Func<string>)));
        }

        [Test]
        public void TestGuardValueWithEqualityComparer()
        {
            var equalityComparer = new TestEqualityComparer();
            var instance = Maybe.Just("test");
            
            var result = instance.Guard("test", equalityComparer);
            result.AssertJust("test");

            result = instance.Guard("notTest", equalityComparer);
            result.AssertNothing();

            result = instance.Guard(default(string), equalityComparer);
            result.AssertNothing();

            result = instance.Guard(() => "test", equalityComparer);
            result.AssertJust("test");

            result = instance.Guard(() => "notTest", equalityComparer);
            result.AssertNothing();

            result = instance.Guard(() => default(string), equalityComparer);
            result.AssertNothing();

            Assert.Throws<ArgumentNullException>(() => instance.Guard("test", null));
            Assert.Throws<ArgumentNullException>(() => instance.Guard(() => "test", null));
            Assert.Throws<ArgumentNullException>(() => instance.Guard(default(Func<string>), equalityComparer));
            Assert.Throws<ArgumentNullException>(() => instance.Guard(default(Func<string>), null));
        }

        [Test]
        public void TestDefaultIfNothingValueType()
        {
            {
                var instance = Maybe.Just(1);
                var result = instance.DefaultIfNothing();
                result.AssertJust(1);
            }
            {
                var instance = Maybe<int>.Nothing;
                var result = instance.DefaultIfNothing();
                result.AssertJust(0);
            }
        }

        [Test]
        public void TestDefaultIfNothing()
        {
            {
                var instance = Maybe.Just("test");
                var result = instance.DefaultIfNothing("nothing");
                instance.AssertJust("test");
            }
            {
                var instance = Maybe<string>.Nothing;
                var result = instance.DefaultIfNothing("nothing");
                result.AssertJust("nothing");
            }
            {
                var instance = Maybe.Just("test");
                Assert.Throws<ArgumentNullException>(() => instance.DefaultIfNothing(default(string)));
            }
            {
                var instance = Maybe<string>.Nothing;
                Assert.Throws<ArgumentNullException>(() => instance.DefaultIfNothing(default(string)));
            }
            {
                var instance = Maybe.Just(1);
                var result = instance.DefaultIfNothing(2);
                result.AssertJust(1);
            }
            {
                var instance = Maybe<int>.Nothing;
                var result = instance.DefaultIfNothing(2);
                result.AssertJust(2);
            }
        }

        [Test]
        public void TestAsComparableValueComparableJust()
        {
            var instance = Maybe.Just(1);
            var comparable = instance.AsComparable();

            {
                var other = Maybe<int>.Nothing;
                var result = comparable.CompareTo(other);
                Assert.AreEqual(1, result);
            }
            {
                var other = Maybe.Just(1);
                var result = comparable.CompareTo(other);
                Assert.AreEqual(0, result);
            }
            {
                var other = Maybe.Just(0);
                var result = comparable.CompareTo(other);
                Assert.AreEqual(1, result);
            }
            {
                var other = Maybe.Just(2);
                var result = comparable.CompareTo(other);
                Assert.AreEqual(-1, result);
            }
        }

        [Test]
        public void TestAsComparableValueComparableNothing()
        {
            var instance = Maybe<int>.Nothing;
            var comparable = instance.AsComparable();
            {
                var other = Maybe<int>.Nothing;
                var result = comparable.CompareTo(other);
                Assert.AreEqual(0, result);
            }
            {
                var other = Maybe.Just(-1);
                var result = comparable.CompareTo(other);
                Assert.AreEqual(-1, result);
            }
        }

        [Test]
        public void TestAsComparableWithComparerNull()
        {
            var instance = Maybe.Just(1);
            Assert.Throws<ArgumentNullException>(() => instance.AsComparable(null));
        }

        [Test]
        public void TestAsComparableWithComparerJust()
        {
            var instance = Maybe<TestRef>.Just(new TestRef(0));

            var comparer = Comparer<TestRef>.Create((x, y) => x.Value.CompareTo(y.Value));
            var comparable = instance.AsComparable(comparer);
            {
                var other = Maybe<TestRef>.Nothing;
                var result = comparable.CompareTo(other);
                Assert.AreEqual(1, result);
            }
            {
                var other = Maybe<TestRef>.Just(new TestRef(0));
                var result = comparable.CompareTo(other);
                Assert.AreEqual(0, result);
            }
            {
                var other = Maybe<TestRef>.Just(new TestRef(-1));
                var result = comparable.CompareTo(other);
                Assert.AreEqual(1, result);
            }
            {
                var other = Maybe<TestRef>.Just(new TestRef(1));
                var result = comparable.CompareTo(other);
                Assert.AreEqual(-1, result);
            }
        }

        [Test]
        public void TestAsComparableWithComparerNothing()
        {
            var instance = Maybe<TestRef>.Nothing;

            var comparer = Comparer<TestRef>.Create((x, y) => x.Value.CompareTo(y.Value));
            var comparable = instance.AsComparable(comparer);
            {
                var other = Maybe<TestRef>.Nothing;
                var result = comparable.CompareTo(other);
                Assert.AreEqual(0, result);
            }
            {
                var other = Maybe<TestRef>.Just(new TestRef(-1));
                var result = comparable.CompareTo(other);
                Assert.AreEqual(-1, result);
            }
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
