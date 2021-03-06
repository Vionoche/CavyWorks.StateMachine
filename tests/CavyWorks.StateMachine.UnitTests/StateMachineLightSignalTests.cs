﻿using System.Threading.Tasks;
using Xunit;

namespace CavyWorks.StateMachine.UnitTests
{
    public class StateMachineLightSignalTests
    {
        [Fact]
        public async Task LightSignalTest()
        {
            LightSignal lightSignal = new LightSignal();

            for (int i = 0; i < 5; i++)
            {
                await AssertLightSignal(lightSignal, 30, Signals.Red).ConfigureAwait(false);
                await AssertLightSignal(lightSignal, 5, Signals.Yellow).ConfigureAwait(false);
                await AssertLightSignal(lightSignal, 30, Signals.Green).ConfigureAwait(false);
                await AssertLightSignal(lightSignal, 5, Signals.Yellow).ConfigureAwait(false);
            }
        }

        private async Task AssertLightSignal(LightSignal lightSignal, int time, Signals expected)
        {
            for (int i = 0; i < time; i++)
            {
                await lightSignal.TickAsync();
                Assert.Equal(expected, lightSignal.State);
            }
        }
    }
}