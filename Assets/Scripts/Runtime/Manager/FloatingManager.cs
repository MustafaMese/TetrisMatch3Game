using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Command;
using Runtime.Component;
using Runtime.View;
using UnityEngine;

namespace Runtime.Manager
{
    public partial class FloatingManager : MonoBehaviour
    {
        [SerializeField] private FloatingObjectView floatingObjectViewPrefab;
        [SerializeField, Range(0, 100f)] private float shiftingDuration;
        [SerializeField, Range(0, 5f)] private float goingDownDuration;
        
        private GridAreaView _gridAreaView;
        private bool _end = false;
        
        public readonly List<FloatingObjectView> ActiveFloatingObjects = new();
        [HideInInspector] public List<FloatingObjectView> FrozenFloatingObjects = new();

        public void Initialize(GridAreaView gridAreaView)
        {
            GameManager.Instance.CommandManager.AddCommandListener<HandleFloatingCommand>(HandleFloating);
            GameManager.Instance.CommandManager.AddCommandListener<FloatingObjectSchemeProducedCommand>(StartProcess);

            _gridAreaView = gridAreaView;
        }

        private void HandleFloating(HandleFloatingCommand e)
        {
            switch (e.NextCommand)
            {
                case FloatingCommand.Shift:
                    StartShifting();
                    break;
                case FloatingCommand.GoDown:
                    StartGoingDown();
                    break;
                case FloatingCommand.Frozen:
                    HandleFrozenObjects();
                    break;
                case FloatingCommand.None:
                    ProduceObject();
                    break;
            }
        }

        private void ProduceObject()
        {
            GameManager.Instance.CommandManager.InvokeCommand(new ProduceFloatingObjectCommand());
        }

        private void HandleFrozenObjects()
        {
           GameManager.Instance.CommandManager.InvokeCommand(new HandleFrozenObjectsCommand());
        }
        
        private void StartGoingDown()
        {
            GameManager.Instance.CommandManager.InvokeCommand(new StartGoingDownCommand(goingDownDuration));
        }

        private void StartShifting()
        {
            GameManager.Instance.CommandManager.InvokeCommand(new StartShiftingCommand((int)(shiftingDuration * 1000f)));
        }
        
        private void StartProcess(FloatingObjectSchemeProducedCommand e)
        {
            Produce(e.line1, e.line2);

            if (_end)
                return;
            
            HandleFloating(new HandleFloatingCommand(FloatingCommand.GoDown));
        }
        
        private void Produce(string line1, string line2)
        {
            for (var i = 0; i < line1.Length && i < _gridAreaView.GetColumnCount(); i++)
            {
                if(_end)
                    return;
                
                var obj = AddObjectToGrid(line1, i, 0);
                if (obj != null)
                    ActiveFloatingObjects.Add(obj);
            }
            
            for (var i = 0; i < line2.Length && i < _gridAreaView.GetColumnCount(); i++)
            {
                if(_end)
                    return;
                
                var obj = AddObjectToGrid(line2, i, 1);
                if (obj != null)
                    ActiveFloatingObjects.Add(obj);
            }
        }

        private FloatingObjectView AddObjectToGrid(string line, int x, int y)
        {
            if (line[x].Equals('1'))
            {
                var grid = _gridAreaView.GetGrid(new Vector2Int(x, y));

                if (grid == null) throw new Exception($"Wrong coordinate {x} {y}");

                if (grid.GetFloatingObject() != null)
                {
                    FailCommand();
                    return null;
                }
                
                var obj = Instantiate(floatingObjectViewPrefab);
                obj.Initialize();
                    
                _gridAreaView.AddFloatingObjectToCanvas(obj.transform);
                grid.Place(obj);
                obj.Place(grid);

                return obj;
            }

            return null;
        }

        private void FailCommand()
        {
            _end = true;
            GameManager.Instance.CommandManager.InvokeCommand(new HandleEndGameCommand(false));
        }

        public void CheckActiveObjectsFrozenStates(Direction direction)
        {
            CheckFrozenState(direction, ActiveFloatingObjects, FrozenFloatingObjects, true);
        }

        public void CheckFrozenObjectsFrozenStates(Direction direction)
        {
            CheckFrozenState(direction, FrozenFloatingObjects, ActiveFloatingObjects, false);
        }

        private void CheckFrozenState(Direction direction, List<FloatingObjectView> targetList, List<FloatingObjectView> oppositeList, bool checkIsFrozen)
        {
            /*
             * 1 - Sorting.
             * 2 - Frozen State Check.
             * 3 - Sorting Again.
             */
            
            Utils.Sort(direction, targetList);

            for (int i = 0; i < targetList.Count; i++)
            {
                var obj = targetList[i];
                obj.CheckFrozenState(direction);

                var isFrozen = checkIsFrozen ? obj.IsFrozen() : !obj.IsFrozen();
                
                if (isFrozen)
                    oppositeList.Add(obj);
            }

            targetList.RemoveAll((view => checkIsFrozen ? view.IsFrozen() : !view.IsFrozen()));

            Utils.Sort(direction, targetList);
        }

        public void CheckShiftStates(Direction direction, int difference)
        {
            /*
             * 1 - Sorting.
             * 2 - Shift State Check.
             */
            
            Utils.Sort(direction, ActiveFloatingObjects);
            
            for (int i = 0; i < ActiveFloatingObjects.Count; i++)
            {
                var obj = ActiveFloatingObjects[i];
                obj.CheckShiftState(direction, difference);
            }
        }
    }
}


