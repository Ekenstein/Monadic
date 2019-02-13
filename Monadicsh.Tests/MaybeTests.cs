using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh.Tests
{
    public class MaybeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestJustNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = Maybe<TestRef>.Just(null);
            });
        }

        private static TestRef[] TestRefCases() => new []
        {
            new TestRef()
        };

        [TestCase(true)]
        [TestCase(false)]
        [TestCase(default(int))]
        [TestCase(default(bool))]
        [TestCaseSource(nameof(TestRefCases))]
        public void TestJust<T>(T value)
        {
            var instance = Maybe<T>.Just(value);
            instance.AssertJust(value);
        }
        
        [Test]
        public void TestNothing()
        {
            var instance = Maybe<TestRef>.Nothing;
            instance.AssertNothing();

            var instance2 = Maybe<int>.Nothing;
            instance2.AssertNothing();
        }

        [Test]
        public void TestDefaultValue()
        {
            var instance = default(Maybe<int>);
            instance.AssertNothing();
        }

        [Test]
        public void TestCreateNullableValue()
        {
            var value = (bool?)true;
            var instance = Maybe.Create(value);
            instance.AssertJust((bool)value);
            
            value = null;
            instance = Maybe.Create(value);
            instance.AssertNothing();
        }

        [Test]
        public void TestCreateReferenceValue()
        {
            var value = new TestRef();
            var instance = Maybe.Create(value);
            instance.AssertJust(value);

            value = null;
            instance = Maybe.Create(value);
            instance.AssertNothing();
        }

        [TestCase(default(bool))]
        [TestCase(default(int))]
        public void TestCreateDefaultValues<T>(T value)
        {
            var instance = Maybe.Create(value);
            instance.AssertJust(value);
        }

        [TestCase(default(bool))]
        [TestCase(default(int))]
        [TestCaseSource(nameof(TestRefCases))]
        public void TestTypeInferredJust<T>(T value)
        {
            var instance = Maybe.Just(value);
            instance.AssertJust(value);
        }

        [Test]
        public void TestTypeInferredJustNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = Maybe.Just(default(TestRef));
            });
        }

        [TestCase(default(int))]
        [TestCase(default(bool))]
        [TestCaseSource(nameof(TestRefCases))]
        public void TestImplicitCreateMaybe<T>(T value)
        {
            Maybe<T> instance = value;
            instance.AssertJust(value);
        }

        [Test]
        public void TestImplicitCreateMaybeNull()
        {
            Maybe<TestRef> instance = null;
            instance.AssertNothing();
        }

        [Test]
        public void TestEqualsJust()
        {
            var instance1 = Maybe.Just(1);
            var instance2 = Maybe.Just(1);

            Assert.AreEqual(instance1, instance2);

            instance1 = Maybe.Just(2);
            Assert.AreNotEqual(instance1, instance2);
        }

        [Test]
        public void TestEqualsJustReference()
        {
            var value = new TestRef();

            var instance1 = Maybe.Just(value);
            var instance2 = Maybe.Just(value);

            Assert.AreEqual(instance1, instance2);

            var value2 = new TestRef();
            instance1 = Maybe.Just(value2);
            
            Assert.AreNotEqual(instance1, instance2);
        }

        [Test]
        public void TestEqualsNothing()
        {
            var instance1 = Maybe<int>.Nothing;
            var instance2 = Maybe<int>.Nothing;

            Assert.AreEqual(instance1, instance2);
        }

        [Test]
        public void TestCreateNonEmpty()
        {
            var instance = Maybe.CreateNonEmpty("test");
            instance.AssertJust("test");
        }

        [Test]
        public void TestCreateNonEmptyNothing()
        {
            var instance = Maybe.CreateNonEmpty("");
            instance.AssertNothing();

            instance = Maybe.CreateNonEmpty(null);
            instance.AssertNothing();

            instance = Maybe.CreateNonEmpty(" ");
            instance.AssertNothing();
        }

        [Test]
        public void TestEnumerable()
        {
            var instance = Maybe.Just(1);
            Assert.AreEqual(1, instance.Count());

            instance = Maybe<int>.Nothing;
            Assert.IsEmpty(instance);

            instance = default(Maybe<int>);
            Assert.IsEmpty(instance);
        }

        [Test]
        public void TestTry()
        {
            var instance = Maybe.Try(() => "test");
            instance.AssertJust("test");

            instance = Maybe.Try(() => default(string));
            instance.AssertNothing();

            instance = Maybe.Try(() => Enumerable.Empty<string>().Single());
            instance.AssertNothing();
        }

        [Test]
        public void TestCast()
        {
            var instance = Maybe.Just(1);
            var result = instance.Cast<int?>();
            result.AssertJust(1);

            instance = Maybe<int>.Nothing;
            result = instance.Cast<int?>();
            result.AssertNothing();

            var instance2 = Maybe.Just((int?)1);
            var result2 = instance2.Cast<int>();
            result2.AssertJust(1);

            var instance3 = Maybe.Just("test");
            var result3 = instance3.Cast<int>();
            result3.AssertNothing();

            var instance4 = Maybe.Just(1);
            var result4 = instance4.Cast<string>();
            result4.AssertNothing();

            instance4 = Maybe<int>.Nothing;
            var result5 = instance4.Cast<float>();
            result5.AssertNothing();
        }

        [Test]
        public void TestComparerValueComparable()
        {
            var comparer = Maybe.Comparer<int>();

            {
                var x = Maybe<int>.Nothing;
                var y = Maybe<int>.Nothing;

                var result = comparer.Compare(x, y);
                Assert.AreEqual(0, result);
            }
            {
                var x = Maybe<int>.Nothing;
                var y = Maybe.Just(1);
                var result = comparer.Compare(x, y);
                Assert.AreEqual(-1, result);
            }
            {
                var x = Maybe<int>.Just(1);
                var y = Maybe<int>.Nothing;
                var result = comparer.Compare(x, y);
                Assert.AreEqual(1, result);
            }
            {
                var x = Maybe<int>.Just(1);
                var y = Maybe<int>.Just(1);
                var result = comparer.Compare(x, y);
                Assert.AreEqual(0, result);
            }
            {
                var x = Maybe<int>.Just(2);
                var y = Maybe<int>.Just(1);
                var result = comparer.Compare(x, y);
                Assert.AreEqual(1, result);
            }
            {
                var x = Maybe<int>.Just(1);
                var y = Maybe<int>.Just(2);
                var result = comparer.Compare(x, y);
                Assert.AreEqual(-1, result);
            }
        }

        [Test]
        public void TestComparerWithValueComparer()
        {
            Assert.Throws<ArgumentNullException>(() => Maybe.Comparer<int>(null));
            var valueComparer = Comparer<TestRef>.Create((x, y) => x.Value.CompareTo(y.Value));
            var comparer = Maybe.Comparer(valueComparer);

            {
                var x = Maybe<TestRef>.Nothing;
                var y = Maybe<TestRef>.Nothing;

                var result = comparer.Compare(x, y);
                Assert.AreEqual(0, result);
            }
            {
                var x = Maybe<TestRef>.Nothing;
                var y = Maybe.Just(new TestRef(1));
                var result = comparer.Compare(x, y);
                Assert.AreEqual(-1, result);
            }
            {
                var x = Maybe<TestRef>.Just(new TestRef(1));
                var y = Maybe<TestRef>.Nothing;
                var result = comparer.Compare(x, y);
                Assert.AreEqual(1, result);
            }
            {
                var x = Maybe<TestRef>.Just(new TestRef(1));
                var y = Maybe<TestRef>.Just(new TestRef(1));
                var result = comparer.Compare(x, y);
                Assert.AreEqual(0, result);
            }
            {
                var x = Maybe<TestRef>.Just(new TestRef(2));
                var y = Maybe<TestRef>.Just(new TestRef(1));
                var result = comparer.Compare(x, y);
                Assert.AreEqual(1, result);
            }
            {
                var x = Maybe<TestRef>.Just(new TestRef(1));
                var y = Maybe<TestRef>.Just(new TestRef(2));
                var result = comparer.Compare(x, y);
                Assert.AreEqual(-1, result);
            }
        }

        private class TestRef 
        {
            public TestRef() { }
            public TestRef(int i)
            {
                Value = i;
            }

            public int Value { get; } = 0;
        }
    }
}