using NUnit.Framework;
using System;

namespace Monadicsh.Tests
{
    public class StateTests
    {
        [Test]
        public void TestDoNothing()
        {
            var instance = State.DoNothing;
            var result = instance.Execute();
            result.AssertSuccess(Maybe<State>.Nothing);
        }

        [Test]
        public void TestDoThrow()
        {
            var instance = State.DoThrow(() => new Exception());
            Assert.Throws<Exception>(() => instance.Execute());
        }

        [Test]
        public void TestDoFail()
        {
            {
                var instance = State.DoFail();
                var result = instance.Execute();
                result.AssertFailed(new Error[0]);
            }
            {
                var errors = new [] { new Error { Code = "test", Description = "test" } };
                var instance = State.DoFail(errors);
                var result = instance.Execute();
                result.AssertFailed(errors);
            }
            {
                var error1 = new Error { Code = "test", Description = "test" };
                var error2 = new Error { Code = "test2", Description = "test2" };
                var instance = State.DoFail(error1, error2);
                var result = instance.Execute();
                result.AssertFailed(new [] { error1, error2 });
            }
            {
                var instance = State.DoFail(null);
                var result = instance.Execute();
                result.AssertFailed(new Error[0]);
            }
        }

        [Test]
        public void TestDo()
        {
            Assert.Throws<ArgumentNullException>(() => State.Do(default(Func<Result<Maybe<State>>>)));
            
            {
                var instance = State.Do(() => State.Finished);
                var result = instance.Execute();
                result.AssertSuccess(Maybe<State>.Nothing);
            }
            {
                var instance = State.Do(() => State.Failed());
                var result = instance.Execute();
                result.AssertFailed(new Error[0]);
            }
            {
                var continuationState = new TestState();
                var instance = State.Do(() => State.Continuation(continuationState));
                var result = instance.Execute();
                result.AssertSuccess(continuationState);
            }
        }

        [Test]
        public void TestFinished()
        {
            var result = State.Finished;
            result.AssertSuccess(Maybe<State>.Nothing);
        }

        [Test]
        public void TestFailed()
        {
            {
                var result = State.Failed();
                result.AssertFailed(new Error[0]);
            }
            {
                var error1 = new Error("test", "test");
                var error2 = new Error("test2", "test2");
                var result = State.Failed(error1, error2);
                result.AssertFailed(new [] { error1, error2 });
            }
            {
                var result = State.Failed(null);
                result.AssertFailed(new Error[0]);
            }
        }

        private class TestState : State
        {
            public override Result<Maybe<State>> Execute() => Finished;
        }
    }
}
