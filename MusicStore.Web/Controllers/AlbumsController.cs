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
    public class AlbumsController : Controller
    {
        private Db db = new Db();

        // GET: Albums
        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.NameeSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.Seearch = String.IsNullOrEmpty(sortOrder) ? searchString : "";
            var albums = from s in db.Albums
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    albums = albums.OrderByDescending(s => s.Name);
                    break;
                default:
                    albums = albums.OrderBy(s => s.Name);
                    break;
            }
            albums = albums.Include(a => a.Artist).Include(a => a.Producer);
            return View(await albums.ToListAsync());
        }

        public ActionResult DateChoosing(string date,Album album)
        {
            DateTime temp = DateTime.Now;
            List<DateTime> dates = new List<DateTime>();
            while (temp < album.ReleaseDate)
            { 
                dates.Add(temp);
                temp.AddDays(1);
            }

            ViewData["DateDD"] = new SelectList(dates.Distinct());

            return View();
        }

        public async Task<ActionResult> Automatisation()
        {
            var albums = from s in db.Albums
                         where s.ReleaseDate>DateTime.Now
                         select s;
            return View(await albums.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<ActionResult> Details(int? id,bool createReport=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            if (createReport)
            {
                Side.FileWork2(db,album);
                var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/AboutAlbumOutput.html");
                return new FilePathResult(path, "text/html");
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name");
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name");
            return View();
        }

        // POST: Albums/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AlbumId,Name,Circulation,Cost,ReleaseDate,ArtistId,ProducerId")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", album.ProducerId);
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", album.ProducerId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AlbumId,Name,Circulation,Cost,ReleaseDate,ArtistId,ProducerId")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "Name", album.ProducerId);
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Album album = await db.Albums.FindAsync(id);
            db.Albums.Remove(album);
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
