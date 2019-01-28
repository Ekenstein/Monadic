using NUnit.Framework;
using System;

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

        private class TestRef 
        {     
        }
    }
}