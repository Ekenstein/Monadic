using Monadicsh.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monadicsh.Tests
{
    public class StateExtensionsTest
    {
        [Test]
        public void TestExecuteAllArgumentNull()
        {
            var state = default(State);
            Assert.Throws<ArgumentNullException>(() => state.ExecuteAll());
        }

        private static IEnumerable<(State, IEnumerable<Error>)> StatesThatWillFail()
        {
            var error = new Error("test", "test");
            yield return (State.DoFail(error), new [] { error });
            yield return (State.Do(() => State.Continuation(State.DoFail(error))), new [] { error });
        }

        private static IEnumerable<State> StatesThatWillFinish()
        {
            yield return State.DoNothing;
            yield return State.Do(() => State.Continuation(State.DoNothing));
        }

        [Test, TestCaseSource(nameof(StatesThatWillFail))]
        public void TestExecuteAllFailed((State, IEnumerable<Error>) testcase)
        {
            var result = testcase.Item1.ExecuteAll();
            result.AssertFailed(testcase.Item2);
        }

        [Test, TestCaseSource(nameof(StatesThatWillFinish))]
        public void TestExecuteAllSuccess(State state)
        {
            var result = state.ExecuteAll();
            result.AssertSuccess();
        }
    }
}
