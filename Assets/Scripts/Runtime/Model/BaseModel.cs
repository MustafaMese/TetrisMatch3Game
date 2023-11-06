using Runtime.Component;
using Runtime.Component.Variables;
using UnityEngine;

namespace Runtime.Model
{
    public class BaseModel<T> : MonoBehaviour where T : BaseVariable
    {
        public T baseVariable;
    }
}