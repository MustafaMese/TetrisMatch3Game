namespace Runtime.Controller
{
    public class BaseController<BaseModel>
    {
        protected BaseModel model;

        public virtual void Setup(BaseModel model)
        {
            this.model = model;
        }
    }
}