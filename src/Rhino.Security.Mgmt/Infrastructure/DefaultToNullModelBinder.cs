using System.Web.Mvc;

namespace Rhino.Security.Mgmt.Infrastructure
{
	public class DefaultToNullModelBinder : IModelBinder
	{
		private readonly IModelBinder _modelBinder;

		public DefaultToNullModelBinder(IModelBinder modelBinder)
		{
			_modelBinder = modelBinder;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			/*
			 * Alwais use prefix (action parameter name) when binding model. 
			 * Without this workaround, an 'undefined' parameter in the json result in an empty object instead of null
			 */
			bindingContext.FallbackToEmptyPrefix = false;
			return _modelBinder.BindModel(controllerContext, bindingContext);
		}
	}
}