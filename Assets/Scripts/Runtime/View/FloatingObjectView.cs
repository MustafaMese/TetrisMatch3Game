using System;
using System.Threading.Tasks;
using Runtime.Component;
using Runtime.Component.Variables;
using Runtime.Controller;
using Runtime.Model;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

namespace Runtime.View
{
    public class FloatingObjectView : BaseView<FloatingObjectModel, FloatingObjectController>
    {
        public void Place(GridView grid)
        {
            controller.SetGrid(grid);
        }

        public GridView GetGrid()
        {
            return controller.GetGrid();
        }

        public Vector2Int GetCoordinate()
        {
            return controller.GetCoordinate();
        }

        public void CheckFrozenState(Direction direction)
        {
            controller.CheckFrozenState(direction);
        }

        public async Task GoDown(float duration)
        {
            await controller.GoDown(duration);
        }

        public async Task Shift(Direction direction, float moveDuration)
        {
            await controller.Shift(direction, moveDuration);
        }
        
        public bool IsFrozen()
        {
            return controller.IsFrozen();
        }
        
        public void CheckShiftState(Direction direction, int difference)
        {
            controller.CheckShiftState(direction, difference);
        }

        public bool CanShift()
        {
            return controller.CanShift();
        }

        public int GetShiftCount()
        {
            return controller.GetShiftCount();
        }

        public FloatingObjectColorType GetColor()
        {
            return controller.GetColor();
        }

        public async Task DestroyItself()
        {
            await controller.DestroyItself();
        }
    }
}