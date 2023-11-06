namespace Runtime.Command
{
    public class HandleEndGameCommand : ICommand
    {
        public bool IsWin;
        
        public HandleEndGameCommand(bool isWin)
        {
            IsWin = isWin;
        }
    }
}