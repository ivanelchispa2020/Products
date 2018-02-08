using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Products.Backend.Models;
using Products.Domain;
using Products.Backend.Helpers;

namespace Products.Backend.Controllers
{
    public class IProductsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: IProducts
        public async Task<ActionResult> Index()
        {
            var iProducts = db.IProducts.Include(i => i.Category);
            return View(await iProducts.ToListAsync());
        }

        // GET: IProducts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var  iProduct = await db.IProducts.FindAsync(id);
            if (iProduct == null)
            {
                return HttpNotFound();
            }
            return View(iProduct);
        }

        // GET: IProducts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.ICategories, "CategoryId", "Description");
            return View();
        }

        // POST: IProducts/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelpers.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                    
                }
                var product = ToProduct(view);
                product.Image = pic;
         
                db.IProducts.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.ICategories, "CategoryId", "Description", view.CategoryId);
            return View(view);
        }

        private IProduct ToProduct(ProductView view)
        {
            return new IProduct {
                ProductId = view.ProductId,
                CategoryId = view.CategoryId,
                Description = view.Description,
                Image = view.Image,
                Price = view.Price,
                IsActive =view.IsActive,
                LastPurchase=view.LastPurchase,
                Stock = view.Stock,
                Remarks =view.Remarks,
                Category=view.Category
            }; 
        }

        // GET: IProducts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var iProduct = await db.IProducts.FindAsync(id);
            if (iProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.ICategories, "CategoryId", "Description", iProduct.CategoryId);

            var view = ToView(iProduct); // METODO QUE CONVIERTE EL PRODUCTO A VIEW
            return View(view);  /// MANDAMOS LA VIEW
        }

        private ProductView ToView(IProduct iProduct)
        {
            return new ProductView
            {
                ProductId = iProduct.ProductId,
                CategoryId = iProduct.CategoryId,
                Description = iProduct.Description,
                Image = iProduct.Image,
                Price = iProduct.Price,
                IsActive = iProduct.IsActive,
                LastPurchase = iProduct.LastPurchase,
                Stock = iProduct.Stock,
                Remarks = iProduct.Remarks,
                Category = iProduct.Category
            };

        }

        // POST: IProducts/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Image;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelpers.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);

                }
                var product = ToProduct(view);
                product.Image = pic;

                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.ICategories, "CategoryId", "Description", view.CategoryId);
            return View(view);
        }

        // GET: IProducts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IProduct iProduct = await db.IProducts.FindAsync(id);
            if (iProduct == null)
            {
                return HttpNotFound();
            }
            return View(iProduct);
        }

        // POST: IProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            IProduct iProduct = await db.IProducts.FindAsync(id);
            db.IProducts.Remove(iProduct);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
