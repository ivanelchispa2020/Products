using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Products.Domain;

namespace Products.Api.Controllers
{
    public class IProductsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/IProducts
        public IQueryable<IProduct> GetIProducts()
        {
            return db.IProducts;
        }

        // GET: api/IProducts/5
        [ResponseType(typeof(IProduct))]
        public async Task<IHttpActionResult> GetIProduct(int id)
        {
            IProduct iProduct = await db.IProducts.FindAsync(id);
            if (iProduct == null)
            {
                return NotFound();
            }

            return Ok(iProduct);
        }

        // PUT: api/IProducts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIProduct(int id, IProduct iProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != iProduct.ProductId)
            {
                return BadRequest();
            }

            db.Entry(iProduct).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/IProducts
        [ResponseType(typeof(IProduct))]
        public async Task<IHttpActionResult> PostIProduct(IProduct iProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.IProducts.Add(iProduct);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = iProduct.ProductId }, iProduct);
        }

        // DELETE: api/IProducts/5
        [ResponseType(typeof(IProduct))]
        public async Task<IHttpActionResult> DeleteIProduct(int id)
        {
            IProduct iProduct = await db.IProducts.FindAsync(id);
            if (iProduct == null)
            {
                return NotFound();
            }

            db.IProducts.Remove(iProduct);
            await db.SaveChangesAsync();

            return Ok(iProduct);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IProductExists(int id)
        {
            return db.IProducts.Count(e => e.ProductId == id) > 0;
        }
    }
}