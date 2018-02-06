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
            IProduct iProduct = await db.IProducts.FindAsync(id);
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
        public async Task<ActionResult> Create([Bind(Include = "ProductId,CategoryId,Description,Image,Price,IsActive,LastPurchase,Stock,Remarks")] IProduct iProduct)
        {
            if (ModelState.IsValid)
            {
                db.IProducts.Add(iProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.ICategories, "CategoryId", "Description", iProduct.CategoryId);
            return View(iProduct);
        }

        // GET: IProducts/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.CategoryId = new SelectList(db.ICategories, "CategoryId", "Description", iProduct.CategoryId);
            return View(iProduct);
        }

        // POST: IProducts/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,CategoryId,Description,Image,Price,IsActive,LastPurchase,Stock,Remarks")] IProduct iProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.ICategories, "CategoryId", "Description", iProduct.CategoryId);
            return View(iProduct);
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
