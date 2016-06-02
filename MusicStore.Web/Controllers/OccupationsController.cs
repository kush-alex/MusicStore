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
using System.Net.Mail;

namespace MusicStore.Web.Controllers
{
    public class OccupationsController : Controller
    {
        private Db db = new Db();

        // GET: Occupations
        public async Task<ActionResult> Index()
        {
            var occupations = db.Occupations.Include(o => o.Album).Include(o => o.Room);
            return View(await occupations.ToListAsync());
        }

        // GET: Occupations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Occupation occupation = await db.Occupations.FindAsync(id);
            if (occupation == null)
            {
                return HttpNotFound();
            }
            return View(occupation);
        }

        // GET: Occupations/Create
        public ActionResult Create()
        {
            var alb=db.Albums.Where(a => a.ReleaseDate>DateTime.Now);
            ViewBag.AlbumId = new SelectList(alb, "AlbumId", "Name");
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Number");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OccupationId,AlbumId,RoomId,DayOfOccupation")] Occupation occupation)
        {

            if (ModelState.IsValid)
            {
                if (!(occupation.DayOfOccupation < DateTime.Now) && !(occupation.DayOfOccupation > db.Albums.Find(occupation.AlbumId).ReleaseDate))
                {
                    if (!(db.Occupations.Where(a => (a.RoomId == occupation.RoomId) && (a.DayOfOccupation == occupation.DayOfOccupation)).Count() > 0))
                    {
                        
                        db.Occupations.Add(occupation);
                        await db.SaveChangesAsync();
                        return RedirectToAction("SendEmail", occupation);
                    }
                    else { ModelState.AddModelError("Date", "Извините, данный зал занят в выбранную дату"); }
                }
                else { ModelState.AddModelError("Year", "Введите дату от сегодня до даты релиза выбранного альбома" + db.Albums.Find(occupation.AlbumId).ReleaseDate); }
            }
            
            var alb = db.Albums.Where(a => a.ReleaseDate > DateTime.Now);
            ViewBag.AlbumId = new SelectList(alb, "AlbumId", "Name", occupation.AlbumId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Number", occupation.RoomId);
            return View(occupation);
        }

        public async Task<ActionResult> SendEmail(Occupation occupation)
        {
            return View(occupation);
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(int occupationId,string email="")
        {
            string message = "";
            Occupation occupation = db.Occupations.Find(occupationId);
            if (!String.IsNullOrEmpty(email))
            {
                message = "Альбом " 
                    + db.Albums.Find(occupation.AlbumId).Name 
                    + " будет записываться в " 
                    + db.Rooms.Find(occupation.RoomId).Number 
                    + " зале, в " 
                    + db.Rooms.Find(occupation.RoomId).openingTime 
                    + "\n" 
                    + occupation.DayOfOccupation;
                SendMail("smtp.gmail.com", "kushnarenko.alex@gmail.com", "kushnar12041996", email, "Студия звукозаписи", message);
                //SendMail("smtp.mail.ru", "kushnarenko_egor@mail.ua", "12324252a", email, "Студия звукозаписи", message);
                
                return RedirectToAction("Sent");
            }
            return View(occupation);
        }

        public async Task<ActionResult> Sent()
        {
            return View();
        }

        public static void SendMail(string smtpServer, string from, string password,
string mailto, string caption, string message, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient(smtpServer);
                //client.Host = smtpServer;
                client.Port = 587;//google.com
                //client.Port = 465;//mail.ru
                client.EnableSsl = true;
                //client.Timeout = 3000000;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
                //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                //NetworkCredential credential = new NetworkCredential(from, password);
                //client.EnableSsl = true;
                //client.UseDefaultCredentials = false;
                //client.Credentials = credential;
                //client.Timeout = 300000;
                //client.Send(mail);
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

        // GET: Occupations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Occupation occupation = await db.Occupations.FindAsync(id);
            if (occupation == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Name", occupation.AlbumId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Number", occupation.RoomId);
            return View(occupation);
        }

        // POST: Occupations/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OccupationId,AlbumId,RoomId,DayOfOccupation")] Occupation occupation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(occupation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Name", occupation.AlbumId);
            ViewBag.RoomId = new SelectList(db.Rooms, "RoomId", "Number", occupation.RoomId);
            return View(occupation);
        }

        // GET: Occupations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Occupation occupation = await db.Occupations.FindAsync(id);
            if (occupation == null)
            {
                return HttpNotFound();
            }
            return View(occupation);
        }

        // POST: Occupations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Occupation occupation = await db.Occupations.FindAsync(id);
            db.Occupations.Remove(occupation);
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
