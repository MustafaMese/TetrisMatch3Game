namespace Runtime.Command
{
    public class StartShiftingCommand : ICommand
    {
        public int ShiftingDuration;
        
        public StartShiftingCommand(int shiftingDuration)
        {
            ShiftingDuration = shiftingDuration;
        }
    }
}