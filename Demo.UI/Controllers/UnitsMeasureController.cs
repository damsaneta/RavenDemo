using System.Web.Mvc;

namespace Demo.UI.Controllers
{
    public class UnitsMeasureController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}