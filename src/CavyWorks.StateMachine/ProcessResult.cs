namespace CavyWorks.StateMachine
{
    public class ProcessResult
    {
        public bool CanProcess { get; }
        public string Message { get; }

        public ProcessResult(bool canProcess, string message)
        {
            CanProcess = canProcess;
            Message = message;
        }

        public ProcessResult()
        {
            Message = null;
            CanProcess = true;
        }
    }
}