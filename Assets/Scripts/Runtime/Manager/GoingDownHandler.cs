using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Command;
using Runtime.Component;
using Runtime.View;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Manager
{
    public class StopGoingDownUnityEvent : UnityEvent {}
    
    public class GoingDownHandler : MonoBehaviour
    {
        private readonly StopGoingDownUnityEvent _stopGoingDownUnityEvent = new();

        private FloatingManager _floatingManager;
        
        private readonly List<(FloatingObjectView obj, GridView curr, GridView next)> _gridFloatingObjPairList = new();
        private Task[] _tasks;

        public void Initialize()
        {
            _floatingManager = GetComponent<FloatingManager>();
            
            _stopGoingDownUnityEvent.AddListener(StopGoingDown);
            GameManager.Instance.CommandManager.AddCommandListener<StartGoingDownCommand>(StartGoingDownTriggered);
        }

        private async void StartGoingDownTriggered(StartGoingDownCommand e)
        {
            _floatingManager.CheckActiveObjectsFrozenStates(Direction.Down);

            if (_floatingManager.ActiveFloatingObjects.Count < 1)
            {
                GameManager.Instance.CommandManager.InvokeCommand(new HandleFloatingCommand(FloatingCommand.None));
                return;
            }

            _tasks = new Task[_floatingManager.ActiveFloatingObjects.Count];
            _gridFloatingObjPairList.Clear();
            
            for (int i = 0; i < _tasks.Length; i++)
            {
                var obj = _floatingManager.ActiveFloatingObjects[i];
                var targetGrid = obj.GetGrid().GetNeighborByDirection(Direction.Down);
                _gridFloatingObjPairList.Add((obj, obj.GetGrid(), targetGrid));
                
                _tasks[i] = obj.GoDown(e.Duration);
            }

            try
            {
                await Task.WhenAll(_tasks);
                
                SetGridFloatingObjectPairs();
                
                if(GameManager.Instance.TaskExceptionHandler.IsCancellationRequested()) return;

                _stopGoingDownUnityEvent.Invoke();
            }
            catch (Exception exception)
            {
                throw new Exception("Going down isn't working properly!");
            }
        }
        
        private void SetGridFloatingObjectPairs()
        {
            for (int i = 0; i < _gridFloatingObjPairList.Count; i++)
            {
                var next = _gridFloatingObjPairList[i].next;
                var curr = _gridFloatingObjPairList[i].curr;
                var obj = _gridFloatingObjPairList[i].obj;

                obj.Place(next);
                next.SetFloatingObject(obj);
                if (curr.GetFloatingObject() == obj)
                    curr.SetFloatingObject(null);
            }
        }
        
        private void StopGoingDown()
        {
            if(_floatingManager.FrozenFloatingObjects.Count > 2)
                GameManager.Instance.CommandManager.InvokeCommand(new HandleFloatingCommand(FloatingCommand.Frozen));
            else
                GameManager.Instance.CommandManager.InvokeCommand(new HandleFloatingCommand(FloatingCommand.Shift));
        }

    }
}
