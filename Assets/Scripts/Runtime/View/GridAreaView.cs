using Runtime.Component;
using Runtime.Controller;
using Runtime.Model;
using UnityEngine;

namespace Runtime.View
{
    public partial class GridAreaView : BaseView<GridAreaModel, GridAreaController>
    {
        [SerializeField] private GridCanvas gridCanvasPrefab;
        
        private GridCanvas _gridCanvas;

        public override void Initialize()
        {
            _gridCanvas = Instantiate(gridCanvasPrefab);
            
            ControllerSetup();
            
            InitializeGridViewArray();
        }

        public int GetGridViewCount() => controller.GetGridViewCount();

        public int GetRowCount() => controller.GetRowCount();
        
        public int GetColumnCount() => controller.GetColumnCount();
        
        private void ControllerSetup()
        {
            controller = new GridAreaController();
            controller.Setup(GetComponent<GridAreaModel>());
        }

        private void InitializeGridViewArray()
        {
            controller.InitializeGridList(_gridCanvas);
        }

        public GridView GetGrid(Vector2Int coordinate)
        {
            return controller.GetGrid(coordinate);
        }

        public void AddFloatingObjectToCanvas(Transform floatingObj)
        {
            _gridCanvas.SetFloatingObjectAsChild(floatingObj);
        }

        public InputPlane GetInputPlane()
        {
            return _gridCanvas.GetInputPlane();
        }
    }
}