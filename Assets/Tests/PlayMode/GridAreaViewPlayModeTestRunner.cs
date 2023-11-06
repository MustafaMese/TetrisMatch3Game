using System.Text;
using NUnit.Framework;
using Runtime.View;
using UnityEngine;

namespace Tests.PlayMode
{
    public class GridAreaViewPlayModeTestRunner
    {
        private GridAreaView CreateGridAreaView()
        {
            var gridAreaView = Object.Instantiate(Resources.Load("Prefabs/Grid Area View Variant", typeof(GridAreaView)) as GridAreaView);
            
            gridAreaView.Initialize();

            return gridAreaView;
        }

        [Test]
        public void IsControllerNull()
        {
            var gridAreaView = CreateGridAreaView();
            
            Assert.False(gridAreaView.TestIsControllerNull());
        }
        
        [Test]
        public void IsCoordinatePairsEmpty()
        {
            var gridAreaView = CreateGridAreaView();
            
            Assert.False(gridAreaView.TestIsCoordinatePairEmpty());
        }

        [Test]
        public void IsCoordinatePairCountTrue()
        {
            var gridAreaView = CreateGridAreaView();
            
            var row = gridAreaView.GetRowCount();
            var col = gridAreaView.GetColumnCount();
            
            Assert.AreEqual(row * col, gridAreaView.TestCoordinatePairCount());
        }
        
        [Test]
        public void PassCoordinatePairs()
        {
            var gridAreaView = CreateGridAreaView();

            var builder = new StringBuilder();
            
            for (int i = 0; i < gridAreaView.GetGridViewCount(); i++)
            {
                builder.AppendLine($"{gridAreaView.TestCoordinatePair(i)}");
            }
            
            Assert.Pass(builder.ToString());
        }

        [Test]
        public void IsGridNeighborsTrue()
        {
            var gridAreaView = CreateGridAreaView();
            var row = gridAreaView.GetRowCount();
            var col = gridAreaView.GetColumnCount();
            
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < col; x++)
                { 
                    var grid = gridAreaView.GetGrid(new Vector2Int(x, y));

                    var neighbors = grid.GetNeighbors();

                    if (x == 0)
                        Assert.True(neighbors.left == null, $"{grid.gameObject.name} {grid.GetCoordinate()}");

                    if (y == 0)
                        Assert.True(neighbors.upward == null, $"{grid.gameObject.name} {grid.GetCoordinate()}");
                    
                    if (x == col - 1)
                        Assert.True(neighbors.right == null, $"{grid.gameObject.name} {grid.GetCoordinate()}");
                    
                    if (y == row - 1)
                        Assert.True(neighbors.downward == null, $"{grid.gameObject.name} {grid.GetCoordinate()}");
                }
            }
        }
    }
}
