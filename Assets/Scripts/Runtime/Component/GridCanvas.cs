using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Component
{
    public class GridCanvas : MonoBehaviour
    {
        [SerializeField] private InputPlane inputPlane;
        [SerializeField] private RectTransform gridParent;
        [SerializeField] private Transform floatingObjectParent;
        
        private GridLayoutGroup _gridLayoutGroup;

        public void Initialize(int columnCount, int rowCount)
        {
            _gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
            _gridLayoutGroup.constraintCount = columnCount;
            
            // First, check column size
            var size = gridParent.sizeDelta.x / columnCount;
            if (gridParent.sizeDelta.y < size * rowCount)
                size = gridParent.sizeDelta.y / rowCount;
            
            _gridLayoutGroup.cellSize = new Vector2(size, size);
        }
        
        public void SetGridAsChild(Transform obj) => obj.SetParent(gridParent.transform);

        public void SetFloatingObjectAsChild(Transform obj)
        {
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(_gridLayoutGroup.cellSize.x, _gridLayoutGroup.cellSize.y);
            obj.SetParent(floatingObjectParent);
            obj.transform.localScale = Vector3.one;
        }

        public InputPlane GetInputPlane()
        {
            return inputPlane;
        }
    }
}
