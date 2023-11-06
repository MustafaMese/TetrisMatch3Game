using Runtime.Component;

namespace Runtime.Command
{
    public class HandleFloatingCommand : ICommand
    {
        public FloatingCommand NextCommand;

        public HandleFloatingCommand(FloatingCommand nextCommand)
        {
            NextCommand = nextCommand;
        }
    }
}