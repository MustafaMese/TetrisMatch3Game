using Runtime.Component;

namespace Runtime.Command
{
    public class InputResponseCommand : ICommand
    {
        public readonly int Difference;
        public readonly Direction Direction;
        
        public InputResponseCommand(int difference, Direction direction)
        {
            Difference = difference;
            Direction = direction;
        }
    }
}