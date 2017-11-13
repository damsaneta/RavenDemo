using System.Web.Mvc;

namespace Demo.UI.Controllers
{
    public class ProductsGroupBySubcategoryRaportController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}