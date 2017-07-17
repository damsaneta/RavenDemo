
using Demo.Model.Entities;
using Demo.Model.Raven;
using System.Web.Mvc;

namespace Demo.UI.Controllers
{
    public class SynchronizationController : Controller
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index1()
        {
           
            return RedirectToAction("Index");
        }
    }
}