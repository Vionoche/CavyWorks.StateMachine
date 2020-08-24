using System.Threading.Tasks;
using Xunit;

namespace CavyWorks.StateMachine.UnitTests
{
    public class StateMachineWorkflowTests
    {
        [Fact]
        public async Task WorkflowTest()
        {
            bool entryAgreementFromDraft = false;
            bool entryAgreementFromEditing = false;
            bool exitAgreementToEditing = false;
            bool exitAgreementToAccepted = false;
            bool entrySent = false;
            bool allowSend = false;

            var machine = new StateMachine<DocumentStatus, DocumentAction>(DocumentStatus.Draft);

            machine.Configuration(DocumentStatus.Draft)
                .Transit(DocumentAction.Save, DocumentStatus.Agreement);

            machine.Configuration(DocumentStatus.Agreement)
                .Transit(DocumentAction.NotAccept, DocumentStatus.Editing)
                .Transit(DocumentAction.Accept, DocumentStatus.Accepted)
                .OnEntryFrom(DocumentAction.Save, (transition) =>
                {
                    switch (transition.Source)
                    {
                        case DocumentStatus.Draft:
                            entryAgreementFromDraft = true;
                            break;

                        case DocumentStatus.Editing:
                            entryAgreementFromEditing = true;
                            break;
                    }
                    return Task.CompletedTask;
                })
                .OnExitTo(DocumentAction.NotAccept, (_) =>
                {
                    exitAgreementToEditing = true;
                    return Task.CompletedTask;
                })
                .OnExitTo(DocumentAction.Accept, (_) =>
                {
                    exitAgreementToAccepted = true;
                    return Task.CompletedTask;
                });

            machine.Configuration(DocumentStatus.Editing)
                .Transit(DocumentAction.Save, DocumentStatus.Agreement);

            machine.Configuration(DocumentStatus.Accepted)
                .Transit(DocumentAction.Send, DocumentStatus.Sent)
                .ConditionFor(DocumentAction.Send, (state, input) => Task.FromResult(allowSend));

            machine.Configuration(DocumentStatus.Sent)
                .OnEntry((_) => { entrySent = true; return Task.CompletedTask; });

            Assert.Equal(DocumentStatus.Draft, machine.State);

            await machine.UpdateAsync(DocumentAction.Save).ConfigureAwait(false);
            Assert.Equal(DocumentStatus.Agreement, machine.State);
            Assert.True(entryAgreementFromDraft);
            Assert.False(entryAgreementFromEditing);

            await machine.UpdateAsync(DocumentAction.NotAccept).ConfigureAwait(false);
            Assert.Equal(DocumentStatus.Editing, machine.State);
            Assert.True(exitAgreementToEditing);
            Assert.False(exitAgreementToAccepted);

            await machine.UpdateAsync(DocumentAction.Save).ConfigureAwait(false);
            Assert.Equal(DocumentStatus.Agreement, machine.State);
            Assert.True(entryAgreementFromDraft);
            Assert.True(entryAgreementFromEditing);

            await machine.UpdateAsync(DocumentAction.Accept).ConfigureAwait(false);
            Assert.Equal(DocumentStatus.Accepted, machine.State);

            await machine.UpdateAsync(DocumentAction.Send).ConfigureAwait(false);
            Assert.Equal(DocumentStatus.Accepted, machine.State);
            Assert.False(entrySent);

            allowSend = true;
            await machine.UpdateAsync(DocumentAction.Send).ConfigureAwait(false);
            Assert.Equal(DocumentStatus.Sent, machine.State);
            Assert.True(entrySent);
        }
    }
}