using System.Web.Mvc;

namespace Demo.UI.Controllers
{
    public class LocationsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}