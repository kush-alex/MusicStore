using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicStore.Entities;
using MusicStore.Web.DataContexts;

namespace MusicStore.Web.Controllers
{
    public class ProducersController : Controller
    {
        private Db db = new Db();

        // GET: Producers
        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var producer = from s in db.Producers
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                producer = producer.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    producer = producer.OrderByDescending(s => s.Name);
                    break;
                default:
                    producer = producer.OrderBy(s => s.Name);
                    break;
            }
            return View(producer.ToList());
        }

        // GET: Producers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = await db.Producers.FindAsync(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // GET: Producers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProducerId,Name,FeePay,PhoneNumber")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                db.Producers.Add(producer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(producer);
        }

        // GET: Producers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = await db.Producers.FindAsync(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // POST: Producers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProducerId,Name,FeePay,PhoneNumber")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(producer);
        }

        // GET: Producers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = await db.Producers.FindAsync(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Producer producer = await db.Producers.FindAsync(id);
            db.Producers.Remove(producer);
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
