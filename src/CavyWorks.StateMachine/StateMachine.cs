using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CavyWorks.StateMachine
{
    /// <summary>
    ///     Main state machine's class.
    ///     TODO: Add async functions support.
    /// </summary>
    /// <typeparam name="TState">State's type.</typeparam>
    /// <typeparam name="TInput">Input's type.</typeparam>
    public class StateMachine<TState, TInput>
    {
        public StateMachine(TState initialState)
        {
            State = initialState;
        }
        
        /// <summary>
        ///     Current state.
        /// </summary>
        public TState State { get; private set; }

        /// <summary>
        ///     List of all states that has transitions.
        /// </summary>
        public Dictionary<TState, StateNode<TState, TInput>> Nodes { get; } = new Dictionary<TState, StateNode<TState, TInput>>();

        private Func<Transition<TState, TInput>, Task> _onEntry;
        private Func<Transition<TState, TInput>, Task> _onExit;


        /// <summary>
        ///     State configuration.
        /// </summary>
        public StateConfiguration<TState, TInput> Configuration(TState state)
        {
            return new StateConfiguration<TState, TInput>(this, state);
        }

        /// <summary>
        ///     Func runs when performs into current state (entry into new state).
        /// </summary>
        public StateMachine<TState, TInput> OnEntry(Func<Transition<TState, TInput>, Task> onEntry)
        {
            this._onEntry = onEntry;
            return this;
        }

        /// <summary>
        ///     Runs Func when performs transition from current state success (exit from current state).
        /// </summary>
        public StateMachine<TState, TInput> OnExit(Func<Transition<TState, TInput>, Task> onExit)
        {
            this._onExit = onExit;
            return this;
        }

        /// <summary>
        ///     Update machine's state.
        /// </summary>
        public async Task UpdateAsync(TInput input)
        {
            TState oldState = State;
            if (Nodes.TryGetValue(State, out var node))
            {
                State = await node.TransitAsync(input);
            }

            if (!State.Equals(oldState))
            {
                await ProcessExit(oldState, input, State);
                await ProcessEntry(oldState, input, State);
            }
        }

        private async Task ProcessEntry(TState source, TInput input, TState destination)
        {
            var transition = new Transition<TState, TInput>(source, input, destination);

            if (Nodes.TryGetValue(destination, out var node))
            {
                if (node.OnEntry != null)
                {
                    await node.OnEntry(transition);
                }
                
                if (node.OnEntryFrom.TryGetValue(input, out var onEntryFrom))
                {
                    await onEntryFrom(transition);
                }
            }

            if (_onEntry != null)
            {
                await _onEntry(transition);
            }
        }

        private async Task ProcessExit(TState source, TInput input, TState destination)
        {
            var transition = new Transition<TState, TInput>(source, input, destination);

            if (Nodes.TryGetValue(source, out var node))
            {
                if (node.OnExit != null)
                {
                    await node.OnExit(transition);
                }

                if (node.OnExitTo.TryGetValue(input, out var onExitTo))
                {
                    await onExitTo(transition);
                }
            }

            if (_onExit != null)
            {
                await _onExit(transition);
            }
        }
    }
}