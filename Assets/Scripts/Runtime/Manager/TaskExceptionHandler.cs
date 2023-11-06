using System.Threading;

namespace Runtime.Manager
{
    public class TaskExceptionHandler
    {
        private readonly CancellationTokenSource _tokenSource;

        public TaskExceptionHandler()
        {
            _tokenSource = new CancellationTokenSource();
            Token = _tokenSource.Token;
        }

        public CancellationToken Token { get; private set; }

        public void Cancel()
        {
            _tokenSource.Cancel();
        }

        public bool IsCancellationRequested()
        {
            return _tokenSource.IsCancellationRequested;
        }
    }
}
