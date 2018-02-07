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
    [Authorize]
    public class ICategoriesController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: ICategories
        public async Task<ActionResult> Index()
        {
            return View(await db.ICategories.ToListAsync());
        }

        // GET: ICategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var iCategory = await db.ICategories.FindAsync(id);
            if (iCategory == null)
            {
                return HttpNotFound();
            }
            return View(iCategory);
        }

        // GET: ICategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ICategories/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CategoryId,Description")] ICategory iCategory)
        {
            if (ModelState.IsValid)
            {
                db.ICategories.Add(iCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(iCategory);
        }

        // GET: ICategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ICategory iCategory = await db.ICategories.FindAsync(id);
            if (iCategory == null)
            {
                return HttpNotFound();
            }
            return View(iCategory);
        }

        // POST: ICategories/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CategoryId,Description")] ICategory iCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(iCategory);
        }

        // GET: ICategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ICategory iCategory = await db.ICategories.FindAsync(id);
            if (iCategory == null)
            {
                return HttpNotFound();
            }
            return View(iCategory);
        }

        // POST: ICategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ICategory iCategory = await db.ICategories.FindAsync(id);
            db.ICategories.Remove(iCategory);
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
