using UnityEngine;

namespace Runtime.Component.Variables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Floating Object")]
    public class FloatingObjectVariable : BaseVariable
    {
        [SerializeField] private Color green;
        [SerializeField] private Color red;
        [SerializeField] private Color yellow;
        [SerializeField] private Color orange;
        [SerializeField] private Color blue;
        
        public Color GetColor(FloatingObjectColorType colorType)
        {
            switch (colorType)
            {
                case FloatingObjectColorType.Blue:
                    return blue;
                case FloatingObjectColorType.Green:
                    return green;
                case FloatingObjectColorType.Red:
                    return red;
                case FloatingObjectColorType.Yellow:
                    return yellow;
                case FloatingObjectColorType.Orange:
                    return orange;
            }

            return Color.white;
        }
    }

    public enum FloatingObjectColorType
    {
        Red,
        Yellow,
        Blue,
        Green,
        Orange
    }
}