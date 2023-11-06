using System;
using Runtime.Command;
using Runtime.Component;
using UnityEngine;

namespace Runtime.Manager
{
    public class InputManager : MonoBehaviour
    {
        private InputPlane _inputPlane;
        
        public Action<int> OnActivate;
        public Action<int> OnInputResponse;
        public Action OnCancelled;

        private int _currentIndex = 2;
        private bool _isCancelled = true;
        
        public void Initialize(InputPlane inputPlane, int colCount)
        {
            _inputPlane = inputPlane;
            _inputPlane.Initialize(this, colCount);
            
            OnInputResponse += InputResponse;
            OnCancelled += Cancelled;
            
            GameManager.Instance.CommandManager.AddCommandListener<InputRequestCancelledCommand>(CancelCommand);
            GameManager.Instance.CommandManager.AddCommandListener<InputRequestCommand>(InputRequest);
        }

        private void CancelCommand(InputRequestCancelledCommand e)
        {
            OnCancelled.Invoke();
        }

        private void Cancelled()
        {
            _isCancelled = true;
        }

        private void InputRequest(InputRequestCommand e)
        {
            _isCancelled = false;
            
            OnActivate.Invoke(_currentIndex);
        }

        private void InputResponse(int newIndex)
        {
            if(_isCancelled) return;
            
            Direction direction = _currentIndex > newIndex ? Direction.Left : Direction.Right;
            var difference = Mathf.Abs(_currentIndex - newIndex);
            
            _currentIndex = newIndex;
            
            GameManager.Instance.CommandManager.InvokeCommand(new InputResponseCommand(difference, direction));
        }
    }
}