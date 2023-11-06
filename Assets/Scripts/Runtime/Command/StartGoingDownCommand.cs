namespace Runtime.Command
{
    public class StartGoingDownCommand : ICommand
    {
        public float Duration;
        
        public StartGoingDownCommand(float duration)
        {
            Duration = duration;
        }
    }
}

