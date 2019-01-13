using Monadic.Extensions;
using NUnit.Framework;

namespace Monadic.Tests
{
    public class ResultExtensionsTest
    {
        [Test]
        public void TestOrFailed()
        {
            var error1 = new Error("code1", "desc1");
            var error2 = new Error("code2", "desc2");

            var instance1 = Result.Failed(error1);
            var instance2 = Result.Failed(error2);

            var result = instance1.Or(instance2);
            Assert.False(result.Succeeded);
            Assert.That(result.Errors, Is.EquivalentTo(new [] { error1, error2 }));

            instance1 = Result.Failed();
            result = instance1.Or(instance2);
            Assert.False(result.Succeeded);
            Assert.That(result.Errors, Is.EquivalentTo(new [] { error2 }));

            instance2 = Result.Failed();
            result = instance1.Or(instance2);
            Assert.False(result.Succeeded);
            Assert.That(result.Errors, Is.EquivalentTo(new Error[0]));
        }

        [Test]
        public void TestOrSuccess()
        {
            var instance1 = Result.Success;
            var instance2 = Result.Failed();

            var result = instance1.Or(instance2);
            Assert.True(result.Succeeded);
            Assert.IsEmpty(result.Errors);

            result = instance2.Or(instance1);
            Assert.True(result.Succeeded);
            Assert.IsEmpty(result.Errors);

            instance2 = Result.Failed(new Error("test", "test"));
            result = instance1.Or(instance2);
            Assert.True(result.Succeeded);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void TestAndFailed()
        {
            var instance1 = Result.Success;
            var instance2 = Result.Failed();

            var result = instance1.And(instance2);
            Assert.False(result.Succeeded);
            Assert.IsEmpty(result.Errors);

            var error = new Error("test", "desc");
            instance2 = Result.Failed(error);

            result = instance1.And(instance2);
            Assert.False(result.Succeeded);
            Assert.That(result.Errors, Is.EquivalentTo(new [] { error }));
        }
    }
}
