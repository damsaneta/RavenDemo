using System.Web.Mvc;

namespace Demo.UI.Controllers
{
    public class ProductInventoriesGroupByProdctIdController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
