using System;
using System.Linq;
using Monadicsh.Extensions;
using NUnit.Framework;

namespace Monadicsh.Tests
{
    public class EitherExtensionsTest
    {
        [Test]
        public void TestFromLeft()
        {
            var instance = new Either<int, string>(1);
            var result = instance.FromLeft(0);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestFromLeftDefaultValue()
        {
            var instance = new Either<int, string>("test");
            var result = instance.FromLeft(1);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestFromLeftArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).FromLeft(1));
        }

        [Test]
        public void TestFromLeftFunc()
        {
            var instance = new Either<int, string>(1);
            var result = instance.FromLeft(() => 0);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestFromLeftFuncDefaultValue()
        {
            var instance = new Either<int, string>("test");
            var result = instance.FromLeft(() => 1);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestFromLeftFuncArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).FromLeft(() => 1));

            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).FromLeft(default(Func<int>)));

            var instance = new Either<int, string>(1);
            Assert.Throws<ArgumentNullException>(() => instance.FromLeft(default(Func<int>)));
        }

        [Test]
        public void TestFromRight()
        {
            var instance = new Either<int, TimeSpan>(TimeSpan.FromDays(1));
            var result = instance.FromRight(TimeSpan.FromDays(0));
            Assert.AreEqual(TimeSpan.FromDays(1), result);
        }

        [Test]
        public void TestFromRightDefaultValue()
        {
            var instance = new Either<int, TimeSpan>(1);
            var result = instance.FromRight(TimeSpan.FromDays(1));
            Assert.AreEqual(TimeSpan.FromDays(1), result);
        }

        [Test]
        public void TestFromRightArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).FromRight("test"));
        }

        [Test]
        public void TestFromRightFunc()
        {
            var instance = new Either<int, TimeSpan>(TimeSpan.FromDays(1));
            var result = instance.FromRight(() => TimeSpan.FromDays(0));
            Assert.AreEqual(TimeSpan.FromDays(1), result);
        }

        [Test]
        public void TestFromRightFuncDefaultValue()
        {
            var instance = new Either<int, TimeSpan>(1);
            var result = instance.FromRight(() => TimeSpan.FromDays(1));
            Assert.AreEqual(TimeSpan.FromDays(1), result);
        }

        [Test]
        public void TestFromRightFuncArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).FromRight(() => "test"));
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).FromRight(default(Func<string>)));
            Assert.Throws<ArgumentNullException>(() =>
                new Either<int, string>("test").FromRight(default(Func<string>)));
        }

        [Test]
        public void TestMaybeLeftJust()
        {
            var instance = new Either<int, string>(1);
            var result = instance.MaybeLeft();
            result.AssertJust(1);
        }

        [Test]
        public void TestMaybeLeftNothing()
        {
            var instance = new Either<int, string>("test");
            var result = instance.MaybeLeft();
            result.AssertNothing();
        }

        [Test]
        public void TestMaybeLeftArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).MaybeLeft());
        }

        [Test]
        public void TestMaybeRightJust()
        {
            var instance = new Either<int, string>("test");
            var result = instance.MaybeRight();
            result.AssertJust("test");
        }

        [Test]
        public void TestMaybeRightNothing()
        {
            var instance = new Either<int, string>(1);
            var result = instance.MaybeRight();
            result.AssertNothing();
        }

        [Test]
        public void TestMaybeRightArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).MaybeRight());
        }

        [Test]
        public void TestLeftOrThrow()
        {
            var instance = new Either<int, string>(1);
            var result = instance.LeftOrThrow(_ => new Exception("test"));
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestLeftOrThrowRight()
        {
            var instance = new Either<int, string>("test");
            Assert.Throws<Exception>(() => instance.LeftOrThrow(r => new Exception(r)));
        }

        [Test]
        public void TestLeftOrThrowArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).LeftOrThrow(_ => new Exception()));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).LeftOrThrow(default(Func<string, Exception>)));
            Assert.Throws<ArgumentNullException>(() =>
                new Either<int, string>(1).LeftOrThrow(default(Func<string, Exception>)));
        }

        [Test]
        public void TestRightOrThrow()
        {
            var instance = new Either<int, string>("test");
            var result = instance.RightOrThrow(_ => new Exception());
            Assert.AreEqual("test", result);
        }

        [Test]
        public void TestRightOrThrowLeft()
        {
            var instance = new Either<int, string>(1);
            Assert.Throws<Exception>(() => instance.RightOrThrow(_ => new Exception()));
        }

        [Test]
        public void TestRightOrThrowArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).RightOrThrow(_ => new Exception()));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).RightOrThrow(default(Func<int, Exception>)));
            Assert.Throws<ArgumentNullException>(() =>
                new Either<int, string>(1).RightOrThrow(default(Func<int, Exception>)));
        }

        [Test]
        public void TestFromEitherLeft()
        {
            var instance = new Either<int, string>(1);
            var result = instance.FromEither(l => l + 1, r => 0);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void TestFromEitherRight()
        {
            var instance = new Either<int, string>("test");
            var result = instance.FromEither(l => l.ToString(), r => r);
            Assert.AreEqual("test", result);
        }

        [Test]
        public void TestFromEitherArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).FromEither(_ => 0, _ => 0));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).FromEither(default(Func<int, int>), _ => 0));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).FromEither(default(Func<int, int>), default(Func<string, int>)));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).FromEither(_ => 0, default(Func<string, int>)));

            var instance = new Either<int, string>(1);
            Assert.Throws<ArgumentNullException>(() =>
                instance.FromEither(default(Func<int, int>), default(Func<string, int>)));
            Assert.Throws<ArgumentNullException>(() => instance.FromEither(default(Func<int, int>), _ => 0));
            Assert.Throws<ArgumentNullException>(() => instance.FromEither(_ => 0, default(Func<string, int>)));
        }

        [Test]
        public void TestPartition()
        {
            var either1 = new Either<int, string>(1);
            var either2 = new Either<int, string>("test");
            var either3 = new Either<int, string>("test2");
            var either4 = new Either<int, string>(2);

            var instance = new[] {either1, either2, either3, either4};
            var (lefts, rights) = instance.Partition();

            Assert.AreEqual(2, lefts.Length);
            Assert.AreEqual(2, rights.Length);
            Assert.True(new [] {1,2}.SequenceEqual(lefts));
            Assert.True(new[] {"test", "test2"}.SequenceEqual(rights));
        }

        [Test]
        public void TestPartitionEmpty()
        {
            var instance = new Either<int, string>[0];
            var (lefts, rights) = instance.Partition();
            Assert.AreEqual(0, lefts.Length);
            Assert.AreEqual(0, rights.Length);
        }

        [Test]
        public void TestPartitionArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((Either<int, string>[]) null).Partition());
        }

        [Test]
        public void TestLefts()
        {
            var either1 = new Either<int, string>(1);
            var either2 = new Either<int, string>("test");
            var either3 = new Either<int, string>("test2");
            var either4 = new Either<int, string>(2);

            var instance = new[]
            {
                either1, either2, either3, either4
            };

            var result = instance.Lefts();
            Assert.AreEqual(2, result.Length);
            Assert.True(new [] {1,2}.SequenceEqual(result));
        }

        [Test]
        public void TestLeftsEmpty()
        {
            var instance = new Either<int, string>[0];
            var result = instance.Lefts();
            Assert.AreEqual(0, result.Length);

            instance = new[]
            {
                new Either<int, string>("test1"),
                new Either<int, string>("test2")
            };

            result = instance.Lefts();
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void TestLeftsArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>[]).Lefts());
        }

        [Test]
        public void TestRights()
        {
            var either1 = new Either<int, string>(1);
            var either2 = new Either<int, string>("test");
            var either3 = new Either<int, string>("test2");
            var either4 = new Either<int, string>(2);

            var instance = new[]
            {
                either1, either2, either3, either4
            };

            var result = instance.Rights();
            Assert.AreEqual(2, result.Length);
            Assert.True(new[] { "test", "test2" }.SequenceEqual(result));
        }

        [Test]
        public void TestRightsEmpty()
        {
            var instance = new Either<int, string>[0];
            var result = instance.Rights();
            Assert.AreEqual(0, result.Length);

            instance = new[]
            {
                new Either<int, string>(1),
                new Either<int, string>(2)
            };

            result = instance.Rights();
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void TestRightsArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>[]).Rights());
        }

        [Test]
        public void TestDoEitherLeft()
        {
            var instance = new Either<int, string>(1);
            instance.DoEither(l =>
            {
                Assert.AreEqual(1, l);
                Assert.Pass("Left was executed");
            }, r => Assert.Fail("Right was executed"));
        }

        [Test]
        public void TestDoEitherRight()
        {
            var instance = new Either<int, string>("test");
            instance.DoEither(l => { Assert.Fail("Left was executed."); }, r =>
            {
                Assert.AreEqual("test", r);
                Assert.Pass("Right was executed.");
            });
        }

        [Test]
        public void TestDoEitherArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).DoEither(_ => {}, _ => {}));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).DoEither(default(Action<int>), _ => {}));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).DoEither(default(Action<int>), default(Action<string>)));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).DoEither(_ => { }, default(Action<string>)));

            var instance = new Either<int, string>(1);
            Assert.Throws<ArgumentNullException>(() =>
                instance.DoEither(default(Action<int>), default(Action<string>)));
            Assert.Throws<ArgumentNullException>(() => instance.DoEither(default(Action<int>), _ => {}));
            Assert.Throws<ArgumentNullException>(() => instance.DoEither(_ => { }, default(Action<string>)));
        }
    }
}
