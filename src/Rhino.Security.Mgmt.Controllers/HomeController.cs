using System.Web.Mvc;

namespace Rhino.Security.Mgmt.Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}