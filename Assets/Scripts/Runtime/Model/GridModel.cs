using Runtime.Component.Variables;
using Runtime.Controller;
using Runtime.View;
using UnityEngine;

namespace Runtime.Model
{
    [System.Serializable]
    public class GridModel : BaseModel<BaseVariable>
    {
        private Vector2Int _coordinate;

        private GridView _upView;
        private GridView _downView;
        private GridView _rightView;
        private GridView _leftView;
        
        public Vector2Int Coordinate
        {
            get => _coordinate;
            set => _coordinate = value;
        }

        public GridView UpView => _upView;
        public GridView DownView => _downView;
        public GridView RightView => _rightView;
        public GridView LeftView => _leftView;
        public FloatingObjectView FloatingObject { get; set; }

        public void SetNeighbors(GridView up, GridView down, GridView left, GridView right)
        {
            _upView = up;
            _downView = down;
            _rightView = right;
            _leftView = left;
        }
    }
}