using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Runtime.Component;
using Runtime.Component.Variables;
using Runtime.Manager;
using Runtime.Model;
using Runtime.View;
using TMPro;
using UnityEngine;

namespace Runtime.Controller
{
    public class FloatingObjectController : BaseController<FloatingObjectModel>
    {
        public override void Setup(FloatingObjectModel model)
        {
            base.Setup(model);
            
            model.Initialize();
        }

        public void SetGrid(GridView grid)
        {
            model.GridView = grid;
        }

        public GridView GetGrid()
        {
            return model.GridView;
        }
        
        public Vector2Int GetCoordinate()
        {
            return model.GridView.GetCoordinate();
        }

        public void CheckFrozenState(Direction direction)
        {
            var grid = model.GridView.GetNeighborByDirection(direction);

            if (grid != null)
            {
                if (grid.IsEmpty() || !grid.IsFrozen())
                    model.IsFrozenState = false;
                else
                    model.IsFrozenState = true;
            }
            else
                model.IsFrozenState = true;
        }

        public void CheckShiftState(Direction direction, int difference)
        {
            model.shiftCount = 0;

            SetShiftCount(direction, difference);
        }

        private void SetShiftCount(Direction direction, int difference)
        {
            while (model.shiftCount < difference)
            {
                var next = GetGrid().GetNeighbor(direction, model.shiftCount + 1);

                if (next == null || next.IsFrozen())
                    return;

                if (next.IsEmpty())
                {
                    model.shiftCount++;
                    continue;
                }
                
                if (next.CanShift())
                    model.shiftCount = Mathf.Clamp(model.shiftCount + next.GetShiftCount(), 0, difference);
                
                break;
            }
        }
        
        public bool IsFrozen()
        {
            return model.IsFrozenState;
        }

        public async Task GoDown(float duration)
        {
            var grid = model.GridView.GetNeighborByDirection(Direction.Down);

            await Move(duration, grid);
        }
        
        public async Task Shift(Direction direction, float moveDuration)
        {
            var targetGrid = model.GridView.GetNeighbor(direction, model.shiftCount);

            await Move(moveDuration, targetGrid);
        }

        private async Task Move(float duration, GridView grid)
        {
            var startPosition = model.transform.position;
            var targetPosition = grid.transform.position;
            var startTime = Time.time;

            while (Time.time - startTime < duration)
            {
                var t = (Time.time - startTime) / duration;
                model.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                if (GameManager.Instance.TaskExceptionHandler.IsCancellationRequested())
                    return;

                await Task.Yield();
            }

            model.transform.position = targetPosition;

            await Task.Yield();
        }

        public bool CanShift()
        {
            return model.shiftCount > 0;
        }

        public int GetShiftCount()
        {
            return model.shiftCount;
        }

        public FloatingObjectColorType GetColor()
        {
            return model.ColorType;
        }

        public async Task DestroyItself()
        {
            await model.Dissolve();
        }
    }
}