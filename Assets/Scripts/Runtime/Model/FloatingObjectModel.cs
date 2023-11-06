using System.Threading.Tasks;
using Runtime.Component;
using Runtime.Component.Variables;
using Runtime.Manager;
using Runtime.View;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Model
{
    public class FloatingObjectModel : BaseModel<FloatingObjectVariable>
    {
        [SerializeField] private Image image;
        [SerializeField] private float dissolveDuration;
        
        private FloatingObjectColorType _colorType;
        
        [HideInInspector] public bool isFrozenState = false;
        [HideInInspector] public bool canShift = false;
        [HideInInspector] public int shiftCount;
        public GridView GridView { get; set; }
        
        public FloatingObjectColorType ColorType => _colorType;

        public bool IsFrozenState
        {
            get => isFrozenState;
            set
            {
                isFrozenState = value;
                if (isFrozenState)
                    canShift = false;
            }
        }

        public void Initialize()
        {
            _colorType = Utils.RandomEnumValue<FloatingObjectColorType>();
            
            image.color = baseVariable.GetColor(_colorType);
        }

        public async Task Dissolve()
        {
            GridView.SetFloatingObject(null);

            var startTime = Time.time;
            while (Time.time - startTime < dissolveDuration)
            {
                var t = (Time.time - startTime) / dissolveDuration;
                
                var color = image.color;
                color.a = Mathf.Lerp(1f, 0f, t);
                image.color = color;
                
                if (GameManager.Instance.TaskExceptionHandler.IsCancellationRequested())
                    return;

                await Task.Yield();
            }
            
            gameObject.SetActive(false);

            await Task.Yield();
        }
    }
}