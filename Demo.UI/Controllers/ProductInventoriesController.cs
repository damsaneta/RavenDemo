using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Demo.UI.Controllers
{
    public class ProductInventoriesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}