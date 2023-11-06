using Runtime.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Runtime.Component
{
    public class InputPlane : MonoBehaviour, IDragHandler
    {
        [SerializeField] private RectTransform targetRect;

        private bool _isActive = false;
        private InputManager _inputManager;
        private int _colCount;
        private int _prevIndex;

        private void Awake()
        {
            targetRect = GetComponent<RectTransform>();
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if(!_isActive) return;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRect, eventData.position, null, out Vector2 localPoint);
      
            var index = (localPoint.x / (targetRect.rect.width / _colCount));

            int fixedIndex = Mathf.Clamp((int)(index + (_colCount / 2f)), 0, _colCount - 1);

            if (fixedIndex != _prevIndex)
            {
                _isActive = false;
                _inputManager.OnInputResponse.Invoke(fixedIndex);
            }
        }

        public void Initialize(InputManager inputManager, int colCount)
        {
            _inputManager = inputManager;
            _colCount = colCount;
        
            inputManager.OnActivate += OnActivate;
            inputManager.OnCancelled += Cancelled;
        }

        private void Cancelled()
        {
            _isActive = false;
        }

        private void OnActivate(int prevIndex)
        {
            _prevIndex = prevIndex;
            _isActive = true;
        }
    }
}
