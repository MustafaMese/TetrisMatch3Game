using System.Collections.Generic;
using Runtime.Component;
using Runtime.Component.Variables;
using Runtime.View;
using UnityEngine;

namespace Runtime.Model
{
    [System.Serializable]
    public partial class GridAreaModel : BaseModel<BaseVariable>
    {
        [SerializeField] private GridView gridViewPrefab;
        [SerializeField] private int rowCount;
        [SerializeField] private int columnCount;

        private Dictionary<Vector2Int, GridView> _gridCoordinatePairs = new();
        private GridView[] _gridViews;

        public int RowCount => rowCount;
        public int ColumnCount => columnCount;

        public GridView GetGridViewByIndex(int index) => _gridViews[index];
        public int GetGridViewCount => _gridViews.Length;

        public void Initialize(GridCanvas gridCanvas)
        {
            gridCanvas.Initialize(columnCount, rowCount);
            
            var gridViewCount = rowCount * columnCount;
            _gridViews = new GridView[gridViewCount];

            for (int i = 0; i < gridViewCount; i++)
            {
                var view = Instantiate(gridViewPrefab);
                
                view.Initialize();

                view.gameObject.name = $"Grid View {i}";
                gridCanvas.SetGridAsChild(view.transform);
                view.transform.localScale = Vector3.one;
                
                _gridViews[i] = view;
            }
        }

        public bool IsGridCoordinatePairEmpty() => _gridCoordinatePairs.Count < 1;

        public void AddItemToGridCoordinatePairs(Vector2Int coordinate, GridView gridView)
        {
            gridView.SetCoordinate(coordinate);

            _gridCoordinatePairs.Add(coordinate, gridView);
        }

        public GridView TryGetValueFromGridCoordinatePairs(Vector2Int coordinate)
        {
            return _gridCoordinatePairs.TryGetValue(coordinate, out var gridView) ? gridView : null;
        }
    }
}