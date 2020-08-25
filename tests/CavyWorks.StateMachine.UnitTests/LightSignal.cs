using System.Threading.Tasks;

namespace CavyWorks.StateMachine.UnitTests
{
    public class LightSignal
        {
            public Signals State => _machine.State;

            private readonly StateMachine<Signals, bool> _machine = new StateMachine<Signals, bool>(Signals.Red);

            /// <summary>
            ///     Abstract time counter.
            /// </summary>
            private int _timeTick = -1;

            /// <summary>
            ///     Signals direction.
            ///     false - red -> yellow -> green.
            ///     true - green -> yellow -> red.
            /// </summary>
            private bool _reverse;

            public LightSignal()
            {
                // Reset counter after every transition.
                _machine.OnExit((transition) =>
                {
                    _timeTick = 0;
                    return Task.CompletedTask;
                });

                // Red signal settings.
                _machine.Configuration(Signals.Red)
                    .Transit(false, Signals.Yellow)
                    .Condition((state, input) => Task.FromResult(_timeTick >= 30)) // Red visible 30 time units.
                    .OnEntry(_ => Reverse()); // Reverse signal direction.

                // Yellow signal settings.
                _machine.Configuration(Signals.Yellow)
                    .Transit(false, Signals.Green)
                    .Transit(true, Signals.Red) 
                    .Condition((state, input) => Task.FromResult(_timeTick >= 5)); // Yellow visible 5 time units.

                // Green signal settings.
                _machine.Configuration(Signals.Green)
                    .Transit(true, Signals.Yellow)
                    .Condition((state, input) => Task.FromResult(_timeTick >= 30)) // Green visible 30 time units.
                    .OnEntry(_ => Reverse()); // Reverse signal direction.
            }

            /// <summary>
            ///     Abstract time tick.
            /// </summary>
            public async Task TickAsync()
            {
                _timeTick++;
                await _machine.UpdateAsync(_reverse).ConfigureAwait(false);
            }

            private Task Reverse()
            {
                _reverse = !_reverse;
                return Task.CompletedTask;
            }
        }
}