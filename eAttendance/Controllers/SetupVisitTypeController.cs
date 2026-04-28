using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eAttendance.Models;

namespace eAttendance.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class SetupVisitTypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupVisitType/
        public ActionResult Index()
        {
            VisitTypeSetUp visittypesetup = new VisitTypeSetUp();
            visittypesetup.VisitTypeSetUpList = db.VistTypeSetUp.Include(v => v.OfficeSetUp);

            return View(visittypesetup);
        }

        // GET: /SetupVisitType/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitTypeSetUp visittypesetup = await db.VistTypeSetUp.FindAsync(id);
            if (visittypesetup == null)
            {
                return HttpNotFound();
            }
            return View(visittypesetup);
        }

        // GET: /SetupVisitType/Create
        public ActionResult Add()
        {
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName");
            return View();
        }

        // POST: /SetupVisitType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "VisitTypeId,OfficeId,VisitTypeName,Alias,Description,Status,flag")] VisitTypeSetUp visittypesetup)
        {
            if (ModelState.IsValid)
            {
                db.VistTypeSetUp.Add(visittypesetup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", visittypesetup.OfficeId);
            return View(visittypesetup);
        }

        // GET: /SetupVisitType/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitTypeSetUp visittypesetup = await db.VistTypeSetUp.FindAsync(id);
            if (visittypesetup == null)
            {
                return HttpNotFound();
            }
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", visittypesetup.OfficeId);
            return View(visittypesetup);
        }

        // POST: /SetupVisitType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VisitTypeId,OfficeId,VisitTypeName,Alias,Description,Status,flag")] VisitTypeSetUp visittypesetup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visittypesetup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", visittypesetup.OfficeId);
            return View(visittypesetup);
        }

        // GET: /SetupVisitType/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitTypeSetUp visittypesetup = await db.VistTypeSetUp.FindAsync(id);
            if (visittypesetup == null)
            {
                return HttpNotFound();
            }
            return View(visittypesetup);
        }

        // POST: /SetupVisitType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VisitTypeSetUp visittypesetup = await db.VistTypeSetUp.FindAsync(id);
            db.VistTypeSetUp.Remove(visittypesetup);
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
