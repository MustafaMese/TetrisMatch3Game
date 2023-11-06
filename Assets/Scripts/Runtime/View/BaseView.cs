using Runtime.Component.Variables;
using Runtime.Controller;
using Runtime.Model;
using UnityEngine;

namespace Runtime.View
{
    public class BaseView<M, C> : MonoBehaviour 
        where C : BaseController<M>, new()
    {
        protected C controller;

        public virtual void Initialize()
        {
            controller = new C();

            controller.Setup(GetComponent<M>());
        }
    }
}