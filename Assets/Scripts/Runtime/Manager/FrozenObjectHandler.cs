using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Command;
using Runtime.Component;
using Runtime.Component.Variables;
using Runtime.View;
using UnityEngine;

namespace Runtime.Manager
{
    public class FrozenObjectHandler : MonoBehaviour
    {
        private FloatingManager _floatingManager;
        private readonly List<FloatingObjectView> _openList = new();
        private readonly List<FloatingObjectView> _closedList = new();
        private readonly List<FloatingObjectView> _path = new();
        private readonly List<Task> _tasks = new(); 

        public void Initialize()
        {
            _floatingManager = GetComponent<FloatingManager>();

            GameManager.Instance.CommandManager.AddCommandListener<HandleFrozenObjectsCommand>(HandleFrozenObjects);
        }

        private async void HandleFrozenObjects(HandleFrozenObjectsCommand e)
        {
            Utils.Sort(Direction.Up, _floatingManager.FrozenFloatingObjects);

            List<FloatingObjectView> frozenObjects = new(_floatingManager.FrozenFloatingObjects);

            _closedList.Clear();
            _tasks.Clear();
            
            while (frozenObjects.Count > 1)
            {
                _openList.Clear();
                _path.Clear();
                _openList.Add(frozenObjects[0]);

                while (_openList.Count > 0)
                {
                    var obj = _openList[0];
                    var color = obj.GetColor();

                    CheckObjectColor(obj, Direction.Down, color);
                    CheckObjectColor(obj, Direction.Up, color);
                    CheckObjectColor(obj, Direction.Right, color);
                    CheckObjectColor(obj, Direction.Left, color);

                    _path.Add(obj);
                    _closedList.Add(obj);
                    _openList.Remove(obj);
                }

                bool willDestroy = _path.Count > 2;
                
                for (int i = 0; i < _path.Count; i++)
                {
                    var obj = _path[i];
                    if (willDestroy)
                    {
                        _tasks.Add(obj.DestroyItself());
                        _floatingManager.ActiveFloatingObjects.Remove(obj);
                        _floatingManager.FrozenFloatingObjects.Remove(obj);
                    }
                    
                    frozenObjects.Remove(obj);
                    _closedList.Remove(obj);
                }

                try
                {
                    await Task.WhenAll(_tasks);
                    _tasks.Clear();
                }
                catch (Exception exception)
                {
                    throw new Exception("Problems when destroying objects!");
                }
            }
            
            _floatingManager.CheckFrozenObjectsFrozenStates(Direction.Down);

            GameManager.Instance.CommandManager.InvokeCommand(_floatingManager.ActiveFloatingObjects.Count > 0
                ? new HandleFloatingCommand(FloatingCommand.Shift)
                : new HandleFloatingCommand(FloatingCommand.None));
        }

        private void CheckObjectColor(FloatingObjectView obj, Direction direction, FloatingObjectColorType color)
        {
            var neighborGrid = obj.GetGrid().GetNeighborByDirection(direction);
            
            if(neighborGrid == null) return;
            
            var neighborObj = neighborGrid.GetFloatingObject();

            if(neighborObj == null) return;
            
            if (!_closedList.Contains(neighborObj) && !_path.Contains(neighborObj) && neighborObj.GetColor() == color)
                _openList.Add(neighborObj);
        }
    }
}