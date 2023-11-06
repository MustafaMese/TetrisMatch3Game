using Runtime.Component;
using Runtime.Model;
using Runtime.View;
using UnityEngine;

namespace Runtime.Controller
{
    public class GridAreaController : BaseController<GridAreaModel>
    {
        public void InitializeGridList(GridCanvas gridCanvas)
        {
            model.Initialize(gridCanvas);
            FillCoordinateGridPairs();
            SetAllGrids();
            InitializeGridViews();
        }

        private void InitializeGridViews()
        {
            for (int i = 0; i < model.GetGridViewCount; i++)
                model.GetGridViewByIndex(i).Initialize();
        }

        private void SetAllGrids()
        {
            for (int i = 0; i < model.GetGridViewCount; i++)
            {
                var grid = model.GetGridViewByIndex(i);
                var coordinate = grid.GetCoordinate();

                var left = GetGridView(coordinate + Vector2Int.left);
                var right = GetGridView(coordinate + Vector2Int.right);
                var upward = GetGridView(coordinate + Vector2Int.down);
                var downward = GetGridView(coordinate + Vector2Int.up);
                
                grid.SetGridNeighbours(upward, downward, left, right);
            }
        }

        private GridView GetGridView(Vector2Int coordinate)
        {
            return model.TryGetValueFromGridCoordinatePairs(coordinate);
        }

        private void FillCoordinateGridPairs()
        {
            var rowCount = model.RowCount;
            var colCount = model.ColumnCount;
            Vector2Int coordinate = Vector2Int.zero;
            
            for (int x = 0, i = 0; x < rowCount; x++)
            {
                for (int y = 0; y < colCount; y++, i++)
                {
                    var grid = model.GetGridViewByIndex(i);
                    coordinate.x = y;
                    coordinate.y = x;
                    model.AddItemToGridCoordinatePairs(coordinate, grid);
                }
            }
        }

        public string PrintCoordinatePair(int index)
        {
            return model.PrintCoordinatePairs(index);
        }

        public bool IsCoordinatePairEmpty()
        {
            return model.IsGridCoordinatePairEmpty();
        }

        public int GetGridViewCount()
        {
            return model.GetGridViewCount;
        }

        public GridView GetGrid(Vector2Int coordinate)
        {
            return model.TryGetValueFromGridCoordinatePairs(coordinate);
        }

        public int GetRowCount()
        {
            return model.RowCount;
        }

        public int GetColumnCount()
        {
            return model.ColumnCount;
        }
    }
}