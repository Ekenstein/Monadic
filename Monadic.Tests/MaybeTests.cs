using Monadic;
using NUnit.Framework;
using System;
using System.Linq;

namespace Tests
{
    public class Tests
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
            AssertJust(instance, value);
        }
        
        [Test]
        public void TestNothing()
        {
            var instance = Maybe<TestRef>.Nothing;
            AssertNothing(instance);

            var instance2 = Maybe<int>.Nothing;
            AssertNothing(instance);
        }

        [Test]
        public void TestDefaultValue()
        {
            var instance = default(Maybe<int>);
            AssertNothing(instance);
        }

        [Test]
        public void TestCreateNullableValue()
        {
            var value = (bool?)true;
            var instance = Maybe.Create(value);
            AssertJust(instance, (bool)value);
            
            value = null;
            instance = Maybe.Create(value);
            AssertNothing(instance);
        }

        [Test]
        public void TestCreateReferenceValue()
        {
            var value = new TestRef();
            var instance = Maybe.Create(value);
            AssertJust(instance, value);

            value = null;
            instance = Maybe.Create(value);
            AssertNothing(instance);
        }

        [TestCase(default(bool))]
        [TestCase(default(int))]
        public void TestCreateDefaultValues<T>(T value)
        {
            var instance = Maybe.Create(value);
            AssertJust(instance, value);
        }

        [TestCase(default(bool))]
        [TestCase(default(int))]
        [TestCaseSource(nameof(TestRefCases))]
        public void TestTypeInferredJust<T>(T value)
        {
            var instance = Maybe.Just(value);
            AssertJust(instance, value);
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
            AssertJust(instance, value);
        }

        [Test]
        public void TestImplicitCreateMaybeNull()
        {
            Maybe<TestRef> instance = null;
            AssertNothing(instance);
        }

        [TestCase(default(int))]
        [TestCase(default(bool))]
        [TestCaseSource(nameof(TestRefCases))]
        public void TestImplicitFromMaybe<T>(T value)
        {
            Maybe<T> instance = value;
            AssertJust(instance, value);
            AssertEqual(instance, value);
        }

        [Test]
        public void TestImplicitFromMaybeNull()
        {
            Maybe<TestRef> instance = null;
            AssertNothing(instance);
            AssertEqual(instance, default(TestRef));

            var instance2 = Maybe<int>.Nothing;
            AssertNothing(instance2);
            AssertEqual(instance2, default(int));
        }

        private static void AssertEqual<T>(Maybe<T> t1, T t2)
        {
            Assert.AreEqual((T)t1, t2);
        }

        private static void AssertNothing<T>(Maybe<T> instance)
        {
            Assert.True(instance.IsNothing);
            Assert.False(instance.IsJust);
            Assert.Throws<InvalidOperationException>(() => 
            {
                var value = instance.Value;
            });
        }

        private static void AssertJust<T>(Maybe<T> instance, T value)
        {
            Assert.False(instance.IsNothing);
            Assert.True(instance.IsJust);
            Assert.AreEqual(value, instance.Value);
        }

        private class TestRef 
        {     
        }
    }
}