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
            var result = instance.GetLeftOrDefault(0);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestLeftOrDefault()
        {
            {
                var instance = new Either<int, string>("test");
                var result = instance.GetLeftOrDefault();
                Assert.AreEqual(0, result);
            }
            {
                var instance = new Either<int, string>(1);
                var result = instance.GetLeftOrDefault();
                Assert.AreEqual(1, result);
            }
            {
                Either<int, string> instance = null;
                Assert.Throws<ArgumentNullException>(() => instance.GetLeftOrDefault());
            }
        }

        [Test]
        public void TestRightOrDefault()
        {
            {
                var instance = new Either<string, int>(1);
                var result = instance.GetRightOrDefault();
                Assert.AreEqual(1, result);
            }
            {
                var instance = new Either<string, int>("test");
                var result = instance.GetRightOrDefault();
                Assert.AreEqual(0, result);
            }
            {
                Either<string, int> instance = null;
                Assert.Throws<ArgumentNullException>(() => instance.GetRightOrDefault());
            }
        }

        [Test]
        public void TestLeftOrDefaultDefaultValue()
        {
            var instance = new Either<int, string>("test");
            var result = instance.GetLeftOrDefault(1);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestFromLeftArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetLeftOrDefault(1));
        }

        [Test]
        public void TestFromLeftFunc()
        {
            var instance = new Either<int, string>(1);
            var result = instance.GetLeftOrDefault(() => 0);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestFromLeftFuncDefaultValue()
        {
            var instance = new Either<int, string>("test");
            var result = instance.GetLeftOrDefault(() => 1);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestFromLeftFuncArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).GetLeftOrDefault(() => 1));

            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetLeftOrDefault(default(Func<int>)));

            var instance = new Either<int, string>(1);
            Assert.Throws<ArgumentNullException>(() => instance.GetLeftOrDefault(default(Func<int>)));
        }

        [Test]
        public void TestFromRight()
        {
            var instance = new Either<int, TimeSpan>(TimeSpan.FromDays(1));
            var result = instance.GetRightOrDefault(TimeSpan.FromDays(0));
            Assert.AreEqual(TimeSpan.FromDays(1), result);
        }

        [Test]
        public void TestFromRightDefaultValue()
        {
            var instance = new Either<int, TimeSpan>(1);
            var result = instance.GetRightOrDefault(TimeSpan.FromDays(1));
            Assert.AreEqual(TimeSpan.FromDays(1), result);
        }

        [Test]
        public void TestFromRightArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetRightOrDefault("test"));
        }

        [Test]
        public void TestFromRightFunc()
        {
            var instance = new Either<int, TimeSpan>(TimeSpan.FromDays(1));
            var result = instance.GetRightOrDefault(() => TimeSpan.FromDays(0));
            Assert.AreEqual(TimeSpan.FromDays(1), result);
        }

        [Test]
        public void TestFromRightFuncDefaultValue()
        {
            var instance = new Either<int, TimeSpan>(1);
            var result = instance.GetRightOrDefault(() => TimeSpan.FromDays(1));
            Assert.AreEqual(TimeSpan.FromDays(1), result);
        }

        [Test]
        public void TestFromRightFuncArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetRightOrDefault(() => "test"));
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetRightOrDefault(default(Func<string>)));
            Assert.Throws<ArgumentNullException>(() =>
                new Either<int, string>("test").GetRightOrDefault(default(Func<string>)));
        }

        [Test]
        public void TestMaybeLeftJust()
        {
            var instance = new Either<int, string>(1);
            var result = instance.GetLeftOrNothing();
            result.AssertJust(1);
        }

        [Test]
        public void TestMaybeLeftNothing()
        {
            var instance = new Either<int, string>("test");
            var result = instance.GetLeftOrNothing();
            result.AssertNothing();
        }

        [Test]
        public void TestMaybeLeftArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetLeftOrNothing());
        }

        [Test]
        public void TestMaybeRightJust()
        {
            var instance = new Either<int, string>("test");
            var result = instance.GetRightOrNothing();
            result.AssertJust("test");
        }

        [Test]
        public void TestMaybeRightNothing()
        {
            var instance = new Either<int, string>(1);
            var result = instance.GetRightOrNothing();
            result.AssertNothing();
        }

        [Test]
        public void TestMaybeRightArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetRightOrNothing());
        }

        [Test]
        public void TestLeftOrThrow()
        {
            var instance = new Either<int, string>(1);
            var result = instance.GetLeftOrThrow(_ => new Exception("test"));
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestLeftOrThrowRight()
        {
            var instance = new Either<int, string>("test");
            Assert.Throws<Exception>(() => instance.GetLeftOrThrow(r => new Exception(r)));
        }

        [Test]
        public void TestLeftOrThrowArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetLeftOrThrow(_ => new Exception()));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).GetLeftOrThrow(default(Func<string, Exception>)));
            Assert.Throws<ArgumentNullException>(() =>
                new Either<int, string>(1).GetLeftOrThrow(default(Func<string, Exception>)));
        }

        [Test]
        public void TestRightOrThrow()
        {
            var instance = new Either<int, string>("test");
            var result = instance.GetRightOrThrow(_ => new Exception());
            Assert.AreEqual("test", result);
        }

        [Test]
        public void TestRightOrThrowLeft()
        {
            var instance = new Either<int, string>(1);
            Assert.Throws<Exception>(() => instance.GetRightOrThrow(_ => new Exception()));
        }

        [Test]
        public void TestRightOrThrowArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetRightOrThrow(_ => new Exception()));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).GetRightOrThrow(default(Func<int, Exception>)));
            Assert.Throws<ArgumentNullException>(() =>
                new Either<int, string>(1).GetRightOrThrow(default(Func<int, Exception>)));
        }

        [Test]
        public void TestMapEitherLeft()
        {
            var instance = new Either<int, string>(1);
            var result = instance.MapEither(l => l + 1, r => 0);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void TestMapEitherRight()
        {
            var instance = new Either<int, string>("test");
            var result = instance.MapEither(l => l.ToString(), r => r);
            Assert.AreEqual("test", result);
        }

        [Test]
        public void TestMapEitherArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).MapEither(_ => 0, _ => 0));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).MapEither(default(Func<int, int>), _ => 0));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).MapEither(default(Func<int, int>), default(Func<string, int>)));
            Assert.Throws<ArgumentNullException>(() =>
                default(Either<int, string>).MapEither(_ => 0, default(Func<string, int>)));

            var instance = new Either<int, string>(1);
            Assert.Throws<ArgumentNullException>(() =>
                instance.MapEither(default(Func<int, int>), default(Func<string, int>)));
            Assert.Throws<ArgumentNullException>(() => instance.MapEither(default(Func<int, int>), _ => 0));
            Assert.Throws<ArgumentNullException>(() => instance.MapEither(_ => 0, default(Func<string, int>)));
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

            Assert.AreEqual(2, lefts.Count());
            Assert.AreEqual(2, rights.Count());
            Assert.True(new [] {1,2}.SequenceEqual(lefts));
            Assert.True(new[] {"test", "test2"}.SequenceEqual(rights));
        }

        [Test]
        public void TestPartitionEmpty()
        {
            var instance = new Either<int, string>[0];
            var (lefts, rights) = instance.Partition();
            Assert.AreEqual(0, lefts.Count());
            Assert.AreEqual(0, rights.Count());
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

            var result = instance.GetLefts();
            Assert.AreEqual(2, result.Count());
            Assert.True(new [] {1,2}.SequenceEqual(result));
        }

        [Test]
        public void TestLeftsEmpty()
        {
            var instance = new Either<int, string>[0];
            var result = instance.GetLefts();
            Assert.AreEqual(0, result.Count());

            instance = new[]
            {
                new Either<int, string>("test1"),
                new Either<int, string>("test2")
            };

            result = instance.GetLefts();
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void TestLeftsArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>[]).GetLefts());
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

            var result = instance.GetRights();
            Assert.AreEqual(2, result.Count());
            Assert.True(new[] { "test", "test2" }.SequenceEqual(result));
        }

        [Test]
        public void TestRightsEmpty()
        {
            var instance = new Either<int, string>[0];
            var result = instance.GetRights();
            Assert.AreEqual(0, result.Count());

            instance = new[]
            {
                new Either<int, string>(1),
                new Either<int, string>(2)
            };

            result = instance.GetRights();
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void TestRightsArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(Either<int, string>[]).GetRights());
        }

        [Test]
        public void TestDefaultIfRight()
        {
            {
                var instance = new Either<int, string>(1);
                var result = instance.DefaultIfRight();
                result.AssertLeft(1);
            }
            {
                var instance = new Either<int, string>("test");
                var result = instance.DefaultIfRight();
                result.AssertLeft(default(int));
            }
            {
                var instance = new Either<int, string>("test");
                var result = instance.DefaultIfRight(1);
                result.AssertLeft(1);
            }
            {
                var instance = new Either<int, string>(1);
                var result = instance.DefaultIfRight(2);
                result.AssertLeft(1);
            }
            {
                Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).DefaultIfRight());
                Assert.Throws<ArgumentNullException>(() => default(Either<string, int>).DefaultIfRight(null));
                Assert.Throws<ArgumentNullException>(() => new Either<string, int>("test").DefaultIfRight(null));
            }
        }

        public void TestDefaultIfLeft()
        {
            {
                var instance = new Either<string, int>(1);
                var result = instance.DefaultIfLeft();
                result.AssertRight(1);
            }
            {
                var instance = new Either<string, int>("test");
                var result = instance.DefaultIfLeft();
                result.AssertRight(default(int));
            }
            {
                var instance = default(Either<string, int>);
                Assert.Throws<ArgumentNullException>(() => instance.DefaultIfLeft());
            }
            {
                var instance = new Either<int, string>(1);
                var result = instance.DefaultIfLeft("test");
                result.AssertRight("test");
            }
            {
                var instance = new Either<int, string>("test");
                var result = instance.DefaultIfLeft("testing");
                result.AssertRight("test");
            }
            {
                var instance = default(Either<int, string>);
                Assert.Throws<ArgumentNullException>(() => instance.DefaultIfLeft("test"));
                Assert.Throws<ArgumentNullException>(() => instance.DefaultIfLeft(null));
                Assert.Throws<ArgumentNullException>(() => new Either<int, string>("test").DefaultIfLeft(null));
            }
        }

        [Test]
        public void TestReverse()
        {
            {
                var instance = new Either<int, string>(1);
                var result = instance.Reverse();
                result.AssertRight(1);
            }
            {
                var instance = new Either<int, string>("test");
                var result = instance.Reverse();
                result.AssertLeft("test");
            }
            {
                Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).Reverse());
            }
        }

        [Test]
        public void TestLeftOrEmpty()
        {
            {
                var instance = new Either<int, string>(1);
                var result = instance.GetLeftOrEmpty().ToArray();
                Assert.Contains(1, result);
                Assert.AreEqual(1, result.Length);
            }
            {
                var instance = new Either<int, string>("test");
                var result = instance.GetLeftOrEmpty().ToArray();
                Assert.IsEmpty(result);
            }
            {
                Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetLeftOrEmpty());
            }
        }

        [Test]
        public void TestRightOrEmpty()
        {
            {
                var instance = new Either<int, string>(1);
                var result = instance.GetRightOrEmpty().ToArray();
                Assert.IsEmpty(result);
            }
            {
                var instance = new Either<int, string>("test");
                var result = instance.GetRightOrEmpty().ToArray();
                Assert.Contains("test", result);
                Assert.AreEqual(1, result.Length);
            }
            {
                Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).GetRightOrEmpty());
            }
        }

        [Test]
        public void TestMapLeft()
        {
            {
                var instance = new Either<int, string>(1);
                var result = instance.MapLeft(v => $"{v}");
                result.AssertLeft("1");
            }
            {
                var instance = new Either<int, string>("test");
                var result = instance.MapLeft(v => $"{v}");
                result.AssertRight("test");
            }
            {
                Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).MapLeft(v => v + 1));
                Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).MapLeft(default(Func<int, string>)));
                Assert.Throws<ArgumentNullException>(() => new Either<int, string>(1).MapLeft(default(Func<int, string>)));
            }
        }

        [Test]
        public void TestMapRight()
        {
            {
                var instance = new Either<int, string>("1");
                var result = instance.MapRight(s => int.Parse(s));
                result.AssertRight(1);
            }
            {
                var instance = new Either<int, string>(1);
                var result = instance.MapRight(s => int.Parse(s));
                result.AssertLeft(1);
            }
            {
                Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).MapRight(v => v + 1));
                Assert.Throws<ArgumentNullException>(() => default(Either<int, string>).MapRight(default(Func<string, int>)));
                Assert.Throws<ArgumentNullException>(() => new Either<int, string>(1).MapRight(default(Func<string, int>)));
            }
        }
    }
}
