namespace CavyWorks.StateMachine
{
    /// <summary>
    ///     Transition from one state to another.
    /// </summary>
    /// <typeparam name="TState">State's type.</typeparam>
    /// <typeparam name="TInput">Input's type.</typeparam>
    public class Transition<TState, TInput>
    {
        public Transition(TState state, TInput input, TState destination)
        {
            Source = state;
            Input = input;
            Destination = destination;
        }
        
        /// <summary>
        ///     Source state.
        /// </summary>
        public TState Source { get; }

        /// <summary>
        ///     Input value.
        /// </summary>
        public TInput Input { get; }

        /// <summary>
        ///     Next state (for successful transition).
        /// </summary>
        public TState Destination { get; }
    }
}