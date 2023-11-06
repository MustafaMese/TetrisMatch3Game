using System.Linq;

namespace Runtime.Model
{
    public partial class GridAreaModel
    {
        public string PrintCoordinatePairs(int index)
        {
            if (IsGridCoordinatePairEmpty())
                return null;

            var item = _gridCoordinatePairs.ElementAt(index);

            return $"{index} {item.Key} {item.Value.gameObject.name}";
        }
    }
}