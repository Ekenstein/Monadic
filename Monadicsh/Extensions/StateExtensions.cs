using System;

namespace Monadicsh.Extensions
{
    /// <summary>
    /// Provides a set of static extensions for the type <see cref="State"/>.
    /// </summary>
    public static class StateExtensions
    {
        /// <summary>
        /// Executes the given <paramref name="state"/> and all its continuation
        /// states until either all the states are finished or until one of the
        /// states returns a failed result.
        /// </summary>
        /// <param name="state">The start state to start execute.</param>
        /// <returns>
        /// A <see cref="Result"/> that describes either that all states were
        /// successfully executed or what went wrong when executing one of the states.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="state"/> is null.</exception>
        public static Result ExecuteAll(this State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            Maybe<State> currentState = state;
            do
            {
                var result = currentState.Value.Execute();
                if (!result.Succeeded)
                {
                    return result.Left;
                }

                currentState = result.Right;
            } while (currentState.IsJust);

            return Result.Success;
        }
    }
}
