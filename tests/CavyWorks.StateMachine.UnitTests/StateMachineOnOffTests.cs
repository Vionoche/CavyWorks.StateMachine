using System.Threading.Tasks;
using Xunit;

namespace CavyWorks.StateMachine.UnitTests
{
    public class StateMachineOnOffTests
    {
        [Fact]
        public async Task OnOffTest()
        {
            var machine = new StateMachine<string, string>("off");
            bool isLightOn = false;

            machine.Configuration("off")
                .Transit("push", "on")
                .OnEntry(_ => { isLightOn = false; return Task.CompletedTask; });

            machine.Configuration("on")
                .Transit("push", "off")
                .OnEntry(_ => { isLightOn = true; return Task.CompletedTask; });

            Assert.Equal("off", machine.State);
            Assert.False(isLightOn);

            await machine.UpdateAsync("").ConfigureAwait(false);
            Assert.Equal("off", machine.State);
            Assert.False(isLightOn);

            await machine.UpdateAsync("miss").ConfigureAwait(false);
            Assert.Equal("off", machine.State);
            Assert.False(isLightOn);

            await machine.UpdateAsync("push").ConfigureAwait(false);
            Assert.Equal("on", machine.State);
            Assert.True(isLightOn);

            await machine.UpdateAsync("push").ConfigureAwait(false);
            Assert.Equal("off", machine.State);
            Assert.False(isLightOn);

            await machine.UpdateAsync("push").ConfigureAwait(false);
            Assert.Equal("on", machine.State);
            Assert.True(isLightOn);
        }
    }
}