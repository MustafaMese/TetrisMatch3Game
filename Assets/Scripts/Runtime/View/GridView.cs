using System;
using Runtime.Component;
using Runtime.Controller;
using Runtime.Model;
using UnityEngine;

namespace Runtime.View
{
    public class GridView : BaseView<GridModel, GridController>
    {
        public override void Initialize()
        {
            base.Initialize();
        }
        
        public void SetGridNeighbours(GridView up, GridView down, GridView left, GridView right)
        {
            controller.SetGridNeighbors(up, down, left, right);
        }

        public Vector2Int GetCoordinate() => controller.GetCoordinate();

        public void SetCoordinate(Vector2Int coordinate)
        {
            controller.SetCoordinate(coordinate);
        }

        public (GridView upward, GridView downward, GridView left, GridView right) GetNeighbors()
        {
            return controller.GetNeighbors();
        }

        public FloatingObjectView GetFloatingObject()
        {
            return controller.GetFloatingObject();
        }

        public void SetFloatingObject(FloatingObjectView obj)
        {
            controller.SetFloatingObject(obj);
        }

        public void Place(FloatingObjectView floatingObject)
        {
            controller.Place(transform, floatingObject);
        }

        public GridView GetNeighborByDirection(Direction direction)
        {
            return controller.GetNeighborNyDirection(direction);
        }

        public bool IsEmpty()
        {
            return GetFloatingObject() == null;
        }

        public bool IsFrozen()
        {
            if (GetFloatingObject() != null)
                return GetFloatingObject().IsFrozen();

            return false;
        }

        public bool CanShift()
        {
            if (GetFloatingObject() != null)
                return GetFloatingObject().CanShift();

            return true;
        }

        public int GetShiftCount()
        {
            return GetFloatingObject().GetShiftCount();
        }

        public GridView GetNeighbor(Direction direction, int difference)
        {
            return difference == 0 ? this : controller.GetNeighbor(direction, difference);
        }
    }
}