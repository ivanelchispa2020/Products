using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Products.Api.Helpers;
using Products.Api.Models;
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

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIProduct(int id, ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.ProductId)
            {
                return BadRequest();
            }

            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(request.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = string.Format("{0}.jpg", guid);
                var folder = "~/Content/Images";
                var fullPath = string.Format("{0}/{1}", folder, file);
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    request.Image = fullPath;
                }
            }

            var product = ToProduct(request);
            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Index"))
                {
                    return BadRequest("There are a record with the same description.");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }

            return Ok(product);
        }

        // POST: api/IProducts
        [ResponseType(typeof(IProduct))]
        public async Task<IHttpActionResult> PostIProduct(ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(request.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = string.Format("{0}.jpg", guid);
                var folder = "~/Content/Images";
                var fullPath = string.Format("{0}/{1}", folder, file);
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    request.Image = fullPath;
                }
            }

            var product = ToProduct(request);
            db.IProducts.Add(product);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Index"))
                {
                    return BadRequest("There are a record with the same description.");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);

        }



        private IProduct ToProduct(ProductRequest request)
        {
            return new IProduct
            {
                Category = request.Category,
                CategoryId = request.CategoryId,
                Description = request.Description,
                Image = request.Image,
                IsActive = request.IsActive,
                LastPurchase = request.LastPurchase,
                Price = request.Price,
                ProductId = request.ProductId,
                Remarks = request.Remarks,
                Stock = request.Stock,
            };
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