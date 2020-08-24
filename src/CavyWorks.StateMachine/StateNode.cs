using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CavyWorks.StateMachine
{
    /// <summary>
    ///     Node with data about state and state's behavior.
    /// </summary>
    /// <typeparam name="TState">State's type.</typeparam>
    /// <typeparam name="TInput">Input's type.</typeparam>
    public class StateNode<TState, TInput>
    {
        public StateNode(TState state)
        {
            State = state;
        }
        
        /// <summary>
        ///     State that has behavior.
        /// </summary>
        public TState State { get; }

        /// <summary>
        ///     Condition Func for transition into new state.
        /// </summary>
        public TransitCondition<TState, TInput> Condition { get; set; }

        /// <summary>
        ///     Func runs when performs into current state (entry into new state).
        /// </summary>
        public Func<Transition<TState, TInput>, Task> OnEntry { get; set; }

        /// <summary>
        ///     Runs Func when performs transition from current state success (exit from current state).
        /// </summary>
        public Func<Transition<TState, TInput>, Task> OnExit { get; set; }

        /// <summary>
        ///     All transitions that can performs from current state.
        /// </summary>
        public Dictionary<TInput, Transition<TState, TInput>> Transitions { get; } = new Dictionary<TInput, Transition<TState, TInput>>();

        /// <summary>
        ///     All functions that can performs for entry into current state.
        /// </summary>
        public Dictionary<TInput, TransitCondition<TState, TInput>> ConditionsFor { get; } = new Dictionary<TInput, TransitCondition<TState, TInput>>();

        /// <summary>
        ///     All functions that can performs for exit from current state.
        /// </summary>
        public Dictionary<TInput, Func<Transition<TState, TInput>, Task>> OnEntryFrom { get; } = new Dictionary<TInput, Func<Transition<TState, TInput>, Task>>();

        /// <summary>
        ///     All functions that can performs when successful transitions occurs (exit from current state).
        /// </summary>
        public Dictionary<TInput, Func<Transition<TState, TInput>, Task>> OnExitTo { get; } = new Dictionary<TInput, Func<Transition<TState, TInput>, Task>>();

        /// <summary>
        ///     Add or update transition.
        /// </summary>
        public void AddOrUpdateTransition(TInput input, TState destination)
        {
            Transitions[input] = new Transition<TState, TInput>(State, input, destination);
        }

        /// <summary>
        ///     Add or update condition.
        /// </summary>
        public void AddOrUpdateConditionFor(TInput input, TransitCondition<TState, TInput> conditionFor)
        {
            ConditionsFor[input] = conditionFor;
        }

        /// <summary>
        ///     Add or update Func when transition to current state occurs.  
        /// </summary>
        public void AddOrUpdateOnEntryFrom(TInput input, Func<Transition<TState, TInput>, Task> onEntryFrom)
        {
            OnEntryFrom[input] = onEntryFrom;
        }

        /// <summary>
        ///     Add or update Func when exit from current state occurs.
        /// </summary>
        public void AddOrUpdateOnExitTo(TInput input, Func<Transition<TState, TInput>, Task> onExitTo)
        {
            OnExitTo[input] = onExitTo;
        }

        /// <summary>
        ///     Run transition Func. When transition failed returns current state.
        /// </summary>
        public async Task<TState> TransitAsync(TInput input)
        {
            var conditionResult = await ProcessAllConditions(input).ConfigureAwait(false);

            if (!conditionResult.CanProcess)
            {
                return State;
            }

            if (!Transitions.TryGetValue(input, out var transition))
            {
                return State;
            }

            return transition.Destination;
        }
        
        
        /// <summary>
        ///     Check if can transit.
        /// </summary>
        public async Task<ProcessResult> CanTransitWithMessageAsync(TInput input, Func<string> exceptionMessageFactory)
        {
            if (!Transitions.TryGetValue(input, out _))
                return new ProcessResult(false, exceptionMessageFactory());
            
            var conditionResult = await ProcessAllConditions(input).ConfigureAwait(false);

            return !conditionResult.CanProcess 
                ? conditionResult : 
                new ProcessResult();
        }

        private async Task<ProcessResult> ProcessAllConditions(TInput input)
        {
            var conditionResult = await ProcessCondition(input).ConfigureAwait(false);
            var conditionForResult = await ProcessConditionFor(input).ConfigureAwait(false);

            if (conditionResult.CanProcess && conditionForResult.CanProcess)
                return new ProcessResult();

            if (!conditionResult.CanProcess)
                return conditionResult;
            
            if (!conditionForResult.CanProcess)
                return conditionForResult;
            
            return new ProcessResult();
        }

        private async Task<ProcessResult> ProcessCondition(TInput input)
        {
            if (Condition == null) 
                return new ProcessResult();
            
            var result = await Condition.Condition.Invoke(State, input).ConfigureAwait(false);
            return new ProcessResult(result, Condition.Message);
        }

        private async Task<ProcessResult> ProcessConditionFor(TInput input)
        {
            if (!ConditionsFor.TryGetValue(input, out var condition) || condition == null)
                return new ProcessResult();
            
            var result = await condition.Condition(State, input).ConfigureAwait(false);
            return new ProcessResult(result, condition.Message);

        }
    }
}