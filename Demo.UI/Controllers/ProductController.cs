using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Domain.Products;

namespace Demo.UI.Controllers
{
    [AllowAnonymous]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public JsonResult GetProducts()
        {
            var products = this.productRepository.GetListForGrid();
            return this.Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}