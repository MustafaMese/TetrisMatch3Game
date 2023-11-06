namespace Runtime.Command
{
    public class FloatingObjectSchemeProducedCommand : ICommand
    {
        public string line1;
        public string line2;
        
        public FloatingObjectSchemeProducedCommand(string schemeLine1, string schemeLine2)
        {
            line1 = schemeLine1;
            line2 = schemeLine2;
        }
    }
}