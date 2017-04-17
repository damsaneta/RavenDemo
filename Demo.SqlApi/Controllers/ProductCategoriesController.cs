using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.SqlApi.Model.Entities;

namespace Demo.SqlApi.Controllers
{
    public class ProductCategoriesController : ApiController
    {
        private readonly EntitiesDbContext db = new EntitiesDbContext();

        // GET: api/ProductCategories
        public IList<ProductCategory> GetProductCategory()
        {
            return db.ProductCategories.ToList();
        }

        // GET: api/ProductCategories/5
        [ResponseType(typeof(ProductCategory))]
        public IHttpActionResult GetProductCategory(int id)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return Ok(productCategory);
        }

        //// PUT: api/ProductCategories/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutProductCategory(int id, ProductCategory productCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != productCategory.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(productCategory).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/ProductCategories
        //[ResponseType(typeof(ProductCategory))]
        //public IHttpActionResult PostProductCategory(ProductCategory productCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.ProductCategory.Add(productCategory);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = productCategory.ID }, productCategory);
        //}

        //// DELETE: api/ProductCategories/5
        //[ResponseType(typeof(ProductCategory))]
        //public IHttpActionResult DeleteProductCategory(int id)
        //{
        //    ProductCategory productCategory = db.ProductCategory.Find(id);
        //    if (productCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    db.ProductCategory.Remove(productCategory);
        //    db.SaveChanges();

        //    return Ok(productCategory);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool ProductCategoryExists(int id)
        //{
        //    return db.ProductCategory.Count(e => e.ID == id) > 0;
        //}
    }
}