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
using MusicStore.Web.Controllers;

namespace MusicStore.Web.Controllers
{
    public class ArtistsController : Controller
    {
        private Db db = new Db();

        // GET: Artists
        public async Task<ActionResult> Index(string sortOrder, string searchString, string YearFrom, string YearTo)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.Search = String.IsNullOrEmpty(sortOrder) ? searchString : "";
            ViewBag.YearFrom = String.IsNullOrEmpty(sortOrder) ? YearFrom : "";
            ViewBag.YearTo = String.IsNullOrEmpty(sortOrder) ? YearTo : "";
            var artists = from s in db.Artists
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                artists = artists.Where(s => s.Name.Contains(searchString));
            }

            Int32 yearfrom=0,yearto=Int32.MaxValue;

            if (!(String.IsNullOrEmpty(YearFrom)))
            {
                if (!Int32.TryParse(YearFrom, out yearfrom))
                {
                    ModelState.AddModelError("AccidentYear", "Неверно введен год");
                }
                else
                {
                    yearfrom = Convert.ToInt32(YearFrom);
                }
            }
            if (!(String.IsNullOrEmpty(YearTo)))
            {
                if (!Int32.TryParse(YearTo, out yearto))
                {
                    ModelState.AddModelError("AccidentYear", "Неверно введен год");
                }
                else
                {
                    yearto = Convert.ToInt32(YearTo);
                }
            }
                artists = artists.Where(s => s.YearOfBeginning >= yearfrom);
                artists = artists.Where(s => s.YearOfBeginning <= yearto);
            
            switch (sortOrder)
            {
                case "name_desc":
                    artists = artists.OrderByDescending(s => s.Name);
                    break;
                default:
                    artists = artists.OrderBy(s => s.Name);
                    break;
            }
            return View(artists.ToList());
        }

        // GET: Artists/Details/5
        public async Task<ActionResult> Details(int? id,bool createReport=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = await db.Artists.FindAsync(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            if (createReport)
            {
                Side.FileWork(db, artist);
                var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/AboutArtistOutput.html");
                return new FilePathResult(path, "text/html");
            }
            return View(artist);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ArtistId,Name,YearOfBeginning,PhoneNumber")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Artists.Add(artist);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = await db.Artists.FindAsync(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ArtistId,Name,YearOfBeginning,PhoneNumber")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = await db.Artists.FindAsync(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Artist artist = await db.Artists.FindAsync(id);
            db.Artists.Remove(artist);
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
