using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Runtime.Command;
using Runtime.Component;
using Runtime.View;
using UnityEngine;

namespace Runtime.Manager
{
    public class ShiftingHandler : MonoBehaviour
    {
        [SerializeField, Range(0, 1f)] private float moveDuration;

        private FloatingManager _floatingManager;
        private bool _isCancelled;
        private bool _isMoving;
        private Action _movementEnded;
        private readonly List<(FloatingObjectView obj, GridView curr, GridView next)> _gridFloatingObjPairList = new();
        private readonly List<Task> _tasks = new();

        public void Initialize()
        {
            _floatingManager = GetComponent<FloatingManager>();
            
            _movementEnded += MovementEnded;
            
            GameManager.Instance.CommandManager.AddCommandListener<StartShiftingCommand>(StartShifting);
            GameManager.Instance.CommandManager.AddCommandListener<InputResponseCommand>(InputResponse);
        }

        private void MovementEnded()
        {
            if (_isMoving) return;
            
            if (_isCancelled)
                GameManager.Instance.CommandManager.InvokeCommand(new HandleFloatingCommand(FloatingCommand.GoDown));
            else
                GameManager.Instance.CommandManager.InvokeCommand(new InputRequestCommand());
            
        }

        private async void InputResponse(InputResponseCommand e)
        {
            if (_isCancelled || _isMoving) return;

            _isMoving = true;
            
            var direction = e.Direction;
            var difference = e.Difference;
            
            try
            {
                await Move(direction, difference);

                if(GameManager.Instance.TaskExceptionHandler.IsCancellationRequested()) return;
                
                _isMoving = false;
                _movementEnded.Invoke();
            }
            catch (Exception exception)
            {
                throw new Exception("Shifting isn't working well!");
            }
        }

        
        // todo try ctach
        private async Task Move(Direction direction, int difference)
        {
            _floatingManager.CheckShiftStates(direction, difference);

            _tasks.Clear();
            _gridFloatingObjPairList.Clear();
            
            for (int i = 0; i < _floatingManager.ActiveFloatingObjects.Count; i++)
            {
                var obj = _floatingManager.ActiveFloatingObjects[i];
                var targetGrid = obj.GetGrid().GetNeighbor(direction, obj.GetShiftCount());
                _gridFloatingObjPairList.Add((obj, obj.GetGrid(), targetGrid));
                
                if(obj.CanShift())
                    _tasks.Add(obj.Shift(direction, moveDuration));
            }
            
            await Task.WhenAll(_tasks);

            SetGridFloatingObjectPairs();

            await Task.Yield();
        }

        private void SetGridFloatingObjectPairs()
        {
            StringBuilder str = new StringBuilder();
            
            for (int i = 0; i < _gridFloatingObjPairList.Count; i++)
            {
                var next = _gridFloatingObjPairList[i].next;
                var curr = _gridFloatingObjPairList[i].curr;
                var obj = _gridFloatingObjPairList[i].obj;

                if (curr == next) continue;
                
                obj.Place(next);
                next.SetFloatingObject(obj);
                if (curr.GetFloatingObject() == obj)
                    curr.SetFloatingObject(null);
            }
        }

        private async void StartShifting(StartShiftingCommand e)
        {
            try
            {
                _isCancelled = false;
                GameManager.Instance.CommandManager.InvokeCommand(new InputRequestCommand());
            
                await Task.Delay(e.ShiftingDuration, GameManager.Instance.TaskExceptionHandler.Token);

                if(GameManager.Instance.TaskExceptionHandler.IsCancellationRequested()) return;
            
                _isCancelled = true;
                GameManager.Instance.CommandManager.InvokeCommand(new InputRequestCancelledCommand());
                _movementEnded.Invoke();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
