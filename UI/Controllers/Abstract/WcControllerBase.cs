using BLL.Abstract;

namespace UI.Controllers.Abstract
{
	public enum CreateMode
	{
		WcExample,
		Collocation
	}

    public abstract class WcControllerBase<T> : CommonControllerBase where T:WcBase
    {
    } 
}