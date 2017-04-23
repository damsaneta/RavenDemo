using System.Web.Mvc;

namespace Demo.UI.Controllers
{
    public class ProductCategoriesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}