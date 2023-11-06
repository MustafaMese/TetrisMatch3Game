using System;
using System.Text;
using Runtime.View;
using UnityEngine;

namespace Runtime.Manager
{
    public partial class FloatingManager
    {
        public bool TestScheme(FloatingObjectSchemeProducer.FloatingObjectScheme scheme)
        {
            return CheckLine(scheme.line1, 0) && CheckLine(scheme.line2, 1);
        }

        public string GetFloatingObjectsPositionAtStart(int y)
        {
            StringBuilder line = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                var grid = _gridAreaView.GetGrid(new Vector2Int(i, y));
                line.Append(grid != null && grid.GetFloatingObject() != null ? "1" : "0");
            }

            return line.ToString();
        }
        
        private bool CheckLine(string line, int y)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i].Equals('1') && CheckIsGridEmpty(i, y))
                    return false;
                
                if (!line[i].Equals('1') && !CheckIsGridEmpty(i, y))
                    return false;
            }

            return true;
        }

        private bool CheckIsGridEmpty(int x, int y)
        {
            var grid = _gridAreaView.GetGrid(new Vector2Int(x, y));
            
            if (grid != null)
            {
                return IsGridEmpty(grid);
            }

            throw new Exception("Grid is null..");
        }

        private bool IsGridEmpty(GridView grid)
        {
            return grid.GetFloatingObject() == null;
        }
    }
}