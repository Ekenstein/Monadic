using NUnit.Framework;

namespace Monadicsh.Tests
{
    public class ErrorTests
    {
        [Test]
        public void TestCode()
        {
            const string code = "test";
            var instance = new Error
            {
                Code = code
            };

            Assert.AreEqual(code, instance.Code);

            const string code2 = "test2";
            instance.Code = code2;

            Assert.AreEqual(code2, instance.Code);
        }

        [Test]
        public void TestDescription()
        {
            const string description = "description";
            var instance = new Error
            {
                Description = description
            };

            Assert.AreEqual(description, instance.Description);

            const string description2 = "description2";
            instance.Description = description2;
            Assert.AreEqual(description2, instance.Description);
        }

        [TestCase("code1", "description1", "code1", "description1", true)]
        [TestCase("code1", "description1", "code1", "description2", false)]
        [TestCase("code1", "description1", "code2", "description2", false)]
        public void TestEquals(string code1, string desc1, string code2, string desc2, bool equivalent)
        {
            var error1 = new Error
            {
                Code = code1,
                Description = desc1
            };

            var error2 = new Error
            {
                Code = code2,
                Description = desc2
            };

            var areEqual = error1.Equals(error2);
            Assert.AreEqual(equivalent, areEqual);

            areEqual = error2.Equals(error1);
            Assert.AreEqual(equivalent, areEqual);
        }
        
        [Test]
        public void TestFailedWhereErrorContainsNull()
        {
            {
                var error = new Error("test", "testdesc");
                var errors = new[]
                {
                    error,
                    default(Error)
                };

                var result = Result.Failed(errors);
                result.AssertFailed(new[] { error });
            }
            {
                var errors = new [] {default(Error)};
                var result = Result.Failed(errors);
                result.AssertFailed(new Error[0]);
            }
        }
    }
}
