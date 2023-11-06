namespace Runtime.View
{
    public partial class GridAreaView
    {
        public string TestCoordinatePair(int index)
        {
            return controller.PrintCoordinatePair(index);
        }

        public bool TestIsCoordinatePairEmpty()
        {
            return controller.IsCoordinatePairEmpty();
        }

        public int TestCoordinatePairCount()
        {
            return controller.GetGridViewCount();
        }

        public bool TestIsControllerNull()
        {
            return controller == null;
        }
    }
}