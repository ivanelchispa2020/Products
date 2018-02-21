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
using System.Web.Mvc;
using Products.Api.Models;
using Products.Domain;

namespace Products.Api.Controllers
{

    [System.Web.Http.Authorize]
    public class ICategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/ICategories
        public async Task<IHttpActionResult> GetICategories()
        {
            var categories = await db.ICategories.ToListAsync();
            var categoriesResponse=new List<CategoryResponse>();

            foreach (var category in categories)
            {
                var productsResponse = new List<ProductsResponse>();

                    foreach (var product in category.Products)  // LISTA DE PRODUCTOS
                    {
                        productsResponse.Add(new ProductsResponse
                        {
                            Description = product.Description,
                            Image=product.Image,
                            IsActive=product.IsActive,
                            LastPurchase=product.LastPurchase,
                            Price=product.Price,
                            ProductId=product.ProductId,
                            Remarks=product.Remarks,
                            Stock=product.Stock

                        });
                    }


                categoriesResponse.Add(new CategoryResponse // ADHIERE TODOS LOS PRODUCTOS A LA CATEGORIA
                {
                        CategoryId=category.CategoryId,
                        Description=category.Description,
                        Products= productsResponse  
                });
            }


            return Ok(categoriesResponse);

        }

        // GET: api/ICategories/5
        [ResponseType(typeof(ICategory))]
        public async Task<IHttpActionResult> GetICategory(int id)
        {
            ICategory iCategory = await db.ICategories.FindAsync(id);
            if (iCategory == null)
            {
                return NotFound();
            }

            return Ok(iCategory);
        }

        // PUT: api/ICategories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutICategory(int id, ICategory iCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != iCategory.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(iCategory).State = EntityState.Modified;

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
                    return BadRequest("Debes ingresar otra descrpcion..");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ICategories
        [ResponseType(typeof(ICategory))]
        public async Task<IHttpActionResult> PostICategory(ICategory iCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ICategories.Add(iCategory);

             // -------  PARA QUE NOS ENVIE LOS ERRORES
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
                    return BadRequest("Debes ingresar otra descrpcion..");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = iCategory.CategoryId }, iCategory);
        }

        // DELETE: api/ICategories/5
        [ResponseType(typeof(ICategory))]
        public async Task<IHttpActionResult> DeleteICategory(int id)
        {
            ICategory iCategory = await db.ICategories.FindAsync(id);
            if (iCategory == null)
            {
                return NotFound();
            }

            db.ICategories.Remove(iCategory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                                  ex.InnerException.InnerException != null &&
                                  ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    return BadRequest("No se puede borrar...Hay productos relacionados en esta categoria...");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }


            return Ok(iCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ICategoryExists(int id)
        {
            return db.ICategories.Count(e => e.CategoryId == id) > 0;
        }
    }
}