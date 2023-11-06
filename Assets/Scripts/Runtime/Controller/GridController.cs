using System;
using JetBrains.Annotations;
using Runtime.Component;
using Runtime.Model;
using Runtime.View;
using UnityEngine;

namespace Runtime.Controller
{
    public class GridController : BaseController<GridModel>
    {
        public void SetGridNeighbors(GridView up, GridView down, GridView left, GridView right)
        {
            model.SetNeighbors(up, down, left, right);
        }

        public Vector2Int GetCoordinate() => model.Coordinate;

        public void SetCoordinate(Vector2Int coordinate)
        {
            model.Coordinate = coordinate;
        }

        public (GridView upward, GridView downward, GridView left, GridView right) GetNeighbors()
        {
            return (model.UpView, model.DownView, model.LeftView, model.RightView);
        }

        public FloatingObjectView GetFloatingObject()
        {
            return model.FloatingObject;
        }

        public void Place(Transform parent, FloatingObjectView floatingObject)
        {
            floatingObject.transform.position = parent.position;
            
            model.FloatingObject = floatingObject;
        }

        public GridView GetNeighborNyDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return model.DownView;
                case Direction.Left:
                    return model.LeftView;
                case Direction.Right:
                    return model.RightView;
                case Direction.Up:
                    return model.UpView;
            }

            return null;
        }

        public void SetFloatingObject(FloatingObjectView floatingObjectView)
        {
            model.FloatingObject = floatingObjectView;
        }
        
        public GridView GetNeighbor(Direction direction, int difference)
        {
            int number = 1;
            GridView grid = GetNeighborNyDirection(direction);

            while (number < difference)
            {
                var next = grid.GetNeighborByDirection(direction);
                if (next == null)
                    return null;
                
                grid = next;
                number++;
            }

            return grid;
        }
    }
}