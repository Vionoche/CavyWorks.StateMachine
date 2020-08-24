using System;
using System.Threading.Tasks;

namespace CavyWorks.StateMachine
{
    public static class StateConfigurationExtensions
    {
        
        /// <summary>
        ///     Func runs when performs into current state (entry into new state).
        /// </summary>
        public static StateConfiguration<TState, TInput> OnEntry<TState, TInput>(this StateConfiguration<TState, TInput> config, Action<Transition<TState, TInput>> onEntry)
        {
            return config.OnEntry(x =>
            {
                onEntry(x);
                return Task.CompletedTask;
            });
        }
        
        /// <summary>
        ///     Func runs when performs into current state (entry into new state).
        /// </summary>
        public static StateConfiguration<TState, TInput> OnEntry<TState, TInput>(this StateConfiguration<TState, TInput> config, Func<Task> onEntry)
        {
            return config.OnEntry((x) => onEntry());
        }
        
        /// <summary>
        ///     Func runs when performs into current state (entry into new state) based on concrete input value.
        /// </summary>
        public static StateConfiguration<TState, TInput> OnEntry<TState, TInput>(this StateConfiguration<TState, TInput> config, Action onEntry)
        {
            return config.OnEntry((x) =>
            {
                onEntry();
                return Task.CompletedTask;
            });
        }
        
        /// <summary>
        ///     Func runs when performs into current state (entry into new state) based on concrete input value.
        /// </summary>
        public static StateConfiguration<TState, TInput> OnEntryFrom<TState, TInput>(this StateConfiguration<TState, TInput> config, TInput input, Action<Transition<TState, TInput>> onEntry)
        {
            return config.OnEntryFrom(input, x =>
            {
                onEntry(x);
                return Task.CompletedTask;
            });
        }
        
        /// <summary>
        ///     Func runs when performs into current state (entry into new state) based on concrete input value.
        /// </summary>
        public static StateConfiguration<TState, TInput> OnEntryFrom<TState, TInput>(this StateConfiguration<TState, TInput> config, TInput input, Func<Task> onEntry)
        {
            return config.OnEntryFrom(input, (x) => onEntry());
        }
        
        /// <summary>
        ///     Func runs when performs into current state (entry into new state).
        /// </summary>
        public static StateConfiguration<TState, TInput> OnEntryFrom<TState, TInput>(this StateConfiguration<TState, TInput> config, TInput input, Action onEntry)
        {
            return config.OnEntryFrom(input, (x) =>
            {
                onEntry();
                return Task.CompletedTask;
            });
        }
        
        /// <summary>
        ///     Set common condition for validation before transition to next state.
        /// </summary>
        public static StateConfiguration<TState, TInput> Condition<TState, TInput>(this StateConfiguration<TState, TInput> config, Func<bool> condition)
        {
            return config.Condition((x, y) => Task.FromResult(condition()));
        }
        
        /// <summary>
        ///     Set common condition for validation before transition to next state.
        /// </summary>
        public static StateConfiguration<TState, TInput> Condition<TState, TInput>(this StateConfiguration<TState, TInput> config, Func<Task<bool>> condition)
        {
            return config.Condition((x, y) => condition());
        }
        
        /// <summary>
        ///     Set common condition for validation before transition to next state.
        /// </summary>
        public static StateConfiguration<TState, TInput> Condition<TState, TInput>(this StateConfiguration<TState, TInput> config, Func<TState, TInput, bool> condition)
        {
            return config.Condition((x, y) => Task.FromResult(condition(x, y)));
        }

        /// <summary>
        ///     Set condition for validation before transition to next state. This condition based on concrete input value.
        /// </summary>
        public static StateConfiguration<TState, TInput> ConditionFor<TState, TInput>(this StateConfiguration<TState, TInput> config, TInput input, Func<bool> conditionFor)
        {
            return config.ConditionFor(input, (x, y) => Task.FromResult(conditionFor()));
        }
        
        /// <summary>
        ///     Set condition for validation before transition to next state. This condition based on concrete input value.
        /// </summary>
        public static StateConfiguration<TState, TInput> ConditionFor<TState, TInput>(this StateConfiguration<TState, TInput> config, TInput input, Func<Task<bool>> conditionFor)
        {
            return config.ConditionFor(input, (x, y) => conditionFor());
        }
        
        /// <summary>
        ///     Set condition for validation before transition to next state. This condition based on concrete input value.
        /// </summary>
        public static StateConfiguration<TState, TInput> ConditionFor<TState, TInput>(this StateConfiguration<TState, TInput> config, TInput input, Func<TState, TInput, bool> conditionFor)
        {
            return config.ConditionFor(input, (x, y) => Task.FromResult(conditionFor(x, y)));
        }
        
        /// <summary>
        ///     Set condition for validation before transition to next state. This condition based on concrete input value.
        /// </summary>
        public static StateConfiguration<TState, TInput> ConditionFor<TState, TInput>(this StateConfiguration<TState, TInput> config, TInput input, Func<TState, bool> conditionFor)
        {
            return config.ConditionFor(input, (x, y) => Task.FromResult(conditionFor(x)));
        }
        
        /// <summary>
        ///     Set condition for validation before transition to next state. This condition based on concrete input value.
        /// </summary>
        public static StateConfiguration<TState, TInput> ConditionFor<TState, TInput>(this StateConfiguration<TState, TInput> config, TInput input, Func<TState, Task<bool>> conditionFor)
        {
            return config.ConditionFor(input, (x, y) => conditionFor(x));
        }

    }
}