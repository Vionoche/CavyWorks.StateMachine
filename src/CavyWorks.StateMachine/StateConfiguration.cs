using System;
using System.Threading.Tasks;

namespace CavyWorks.StateMachine
{
    /// <summary>
    ///     State's config.
    /// </summary>
    public class StateConfiguration<TState, TInput>
    {
        public StateConfiguration(StateMachine<TState, TInput> machine, TState state)
        {
            this._machine = machine;
            this._state = state;
        }
        
        private readonly StateMachine<TState, TInput> _machine;
        private readonly TState _state;

        /// <summary>
        ///     Add transition from current state to destination state.
        /// </summary>
        public StateConfiguration<TState, TInput> Transit(TInput input, TState destination)
        {
            var node = FindOrCreateNode(_state);
            node.AddOrUpdateTransition(input, destination);
            return this;
        }

        private StateNode<TState, TInput> FindOrCreateNode(TState state)
        {
            if (!_machine.Nodes.TryGetValue(state, out var node))
            {
                node = new StateNode<TState, TInput>(state);
                _machine.Nodes.Add(state, node);
            }
            return node;
        }

        /// <summary>
        ///     Set common condition for validation before transition to next state.
        /// </summary>
        public StateConfiguration<TState, TInput> Condition(Func<TState, TInput, Task<bool>> condition)
        {
            var node = FindOrCreateNode(_state);
            node.Condition = condition;
            return this;
        }

        /// <summary>
        ///     Set condition for validation before transition to next state. This condition based on concrete input value.
        /// </summary>
        public StateConfiguration<TState, TInput> ConditionFor(TInput input, Func<TState, TInput, Task<bool>> conditionFor)
        {
            var node = FindOrCreateNode(_state);
            node.AddOrUpdateConditionFor(input, conditionFor);
            return this;
        }

        /// <summary>
        ///     Func runs when performs into current state (entry into new state).
        /// </summary>
        public StateConfiguration<TState, TInput> OnEntry(Func<Transition<TState, TInput>, Task> onEntry)
        {
            var node = FindOrCreateNode(_state);
            node.OnEntry = onEntry;
            return this;
        }

        /// <summary>
        ///     Func runs when performs into current state (entry into new state) based on concrete input value.
        /// </summary>
        public StateConfiguration<TState, TInput> OnEntryFrom(TInput input, Func<Transition<TState, TInput>, Task> onEntryFrom)
        {
            var node = FindOrCreateNode(_state);
            node.AddOrUpdateOnEntryFrom(input, onEntryFrom);
            return this;
        }

        /// <summary>
        ///      Runs Func when performs transition from current state success (exit from current state).
        /// </summary>
        public StateConfiguration<TState, TInput> OnExit(Func<Transition<TState, TInput>, Task> onExit)
        {
            var node = FindOrCreateNode(_state);
            node.OnExit = onExit;
            return this;
        }

        /// <summary>
        ///     Runs Func when performs transition from current state success (exit from current state) based on concrete input value.
        /// </summary>
        public StateConfiguration<TState, TInput> OnExitTo(TInput input, Func<Transition<TState, TInput>, Task> onExitTo)
        {
            var node = FindOrCreateNode(_state);
            node.AddOrUpdateOnExitTo(input, onExitTo);
            return this;
        }

        public StateConfiguration<TState, TInput> ThrowIfConditionFailed()
        {
            throw new NotImplementedException();
        }

        public StateConfiguration<TState, TInput> ThrowIfConditionFailed(TInput input)
        {
            throw new NotImplementedException();
        }

        public StateConfiguration<TState, TInput> ThrowIfStateNotFound()
        {
            throw new NotImplementedException();
        }

        public StateConfiguration<TState, TInput> ThrowIfTransitionNotFound()
        {
            throw new NotImplementedException();
        }

        public StateConfiguration<TState, TInput> ThrowIfTransitionNotFound(TInput input)
        {
            throw new NotImplementedException();
        }
    }
}