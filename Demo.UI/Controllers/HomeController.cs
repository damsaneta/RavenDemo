using System.Web.Mvc;

namespace Demo.UI.Controllers
{
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            return this.RedirectToAction("Index", "Product");
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
