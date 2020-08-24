using System;
using System.Threading.Tasks;

namespace CavyWorks.StateMachine
{
    public class TransitCondition<TState, TInput>
    {
        public Func<TState, TInput, Task<bool>> Condition { get; }
        public string Message { get; }

        public TransitCondition(Func<TState, TInput, Task<bool>> condition)
            : this(condition, "Condition was Failed")
        {
        }

        public TransitCondition(Func<TState, TInput, Task<bool>> condition, string message)
        {
            Condition = condition;
            Message = message;
        }
    }
}