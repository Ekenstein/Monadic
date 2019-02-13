using System;

namespace Monadicsh
{
    /// <summary>
    /// A type representing a state that can be executed and may or
    /// may not return a continuation state.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Returns a successful <see cref="Result{T}"/> containing a continuation state.
        /// </summary>
        /// <param name="state">The continuation state.</param>
        /// <returns>
        /// A successful <see cref="Result{T}"/> containing the given <paramref name="state"/>
        /// representing the continuation state.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the given <paramref name="state"/> is null.</exception>
        public static Result<Maybe<State>> Continuation(State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return Result.Create(Maybe.Just(state));
        }

        /// <summary>
        /// A successful <see cref="Result{T}"/> containing no continuation state.
        /// </summary>
        public static Result<Maybe<State>> Finished => Result.Create(Maybe<State>.Nothing);

        /// <summary>
        /// Returns an unsuccessful <see cref="Result{T}"/> describing what went wrong executing
        /// the <see cref="State"/>.
        /// </summary>
        /// <param name="errors">The errors describing what went wrong when executing the <see cref="State"/>.</param>
        /// <returns>
        /// An unsuccessful <see cref="Result{T}"/> containing the given <paramref name="errors"/> describing
        /// what went wrong executing the <see cref="State"/>.
        /// </returns>
        public static Result<Maybe<State>> Failed(params Error[] errors) => Result<Maybe<State>>.Failed(errors);

        /// <summary>
        /// Returns a state that does nothing. Just returns a successful result without
        /// a continuation state when executed.
        /// </summary>
        public static State DoNothing => Do(() => Finished);

        /// <summary>
        /// Returns a <see cref="State"/> that will always fail whenever executed.
        /// </summary>
        /// <param name="errors">The errors describing why the state should fail.</param>
        /// <returns>
        /// A <see cref="State"/> that will return a failed result containing the given <paramref name="errors"/>
        /// whenever executed.
        /// </returns>
        public static State DoFail(params Error[] errors) => Do(() => Failed(errors));

        /// <summary>
        /// Returns a <see cref="State"/> that will always throw whenever executed.
        /// </summary>
        /// <param name="exceptionSelector">The function producing the exception that should be thrown.</param>
        /// <returns>
        /// A <see cref="State"/> that will throw the exception produced by the given <paramref name="exceptionSelector"/>
        /// whenever executed.
        /// </returns>
        public static State DoThrow(Func<Exception> exceptionSelector) => Do(() => throw exceptionSelector());

        /// <summary>
        /// Returns a state that will execute the given lambda whenever executed.
        /// </summary>
        /// <param name="execute">The function to be performed when executing the state.</param>
        /// <returns>
        /// A <see cref="State"/> that executes the given lambda whenever executed.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="execute"/> is null.</exception>
        public static State Do(Func<Result<Maybe<State>>> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            return new DoState(execute);
        }

        /// <summary>
        /// Executes the state and returns whether the execution
        /// was successful. If the state was executed successfully,
        /// a continuation state may be returned or <see cref="Maybe{T}.Nothing"/>
        /// if there is no continuation state.
        /// </summary>
        /// <returns>
        /// A <see cref="Result{T}"/> describing whether the state was successfully executed
        /// or not. If the state was successfully executed, a continuation state may or may not
        /// be returned.
        /// </returns>
        public abstract Result<Maybe<State>> Execute();

        /// <summary>
        /// A state that executes its given lambda as its execution step.
        /// </summary>
        private class DoState : State
        {
            private readonly Func<Result<Maybe<State>>> _execute;

            public DoState(Func<Result<Maybe<State>>> execute)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            }

            public override Result<Maybe<State>> Execute() => _execute();
        }
    }
}
