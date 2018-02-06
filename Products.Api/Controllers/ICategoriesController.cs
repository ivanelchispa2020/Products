﻿using System;
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
    public class ICategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/ICategories
        public IQueryable<ICategory> GetICategories()
        {
            return db.ICategories;
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
            catch (DbUpdateConcurrencyException)
            {
                if (!ICategoryExists(id))
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

        // POST: api/ICategories
        [ResponseType(typeof(ICategory))]
        public async Task<IHttpActionResult> PostICategory(ICategory iCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ICategories.Add(iCategory);
            await db.SaveChangesAsync();

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
            await db.SaveChangesAsync();

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