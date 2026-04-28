using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eAttendance.Models;

namespace eAttendance.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class SetupShiftController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupShift/
        public ActionResult Index()
        {
            Shift model = new Shift();
            model.ShiftList = new List<Shift>();
            model.ShiftList = db.Shift.ToList();
            return View(model);
        }

        // GET: /SetupShift/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shift.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // GET: /SetupShift/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /SetupShift/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Shift shift)
        {
            if (ModelState.IsValid)
            {
                db.Shift.Add(shift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shift);
        }

        // GET: /SetupShift/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shift.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: /SetupShift/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShiftId,ShiftName,Status,DisplayOrder")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shift);
        }

        // GET: /SetupShift/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shift.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: /SetupShift/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Shift shiftmodel)
        {
            Shift shift = db.Shift.Find(shiftmodel.ShiftId);
            db.Shift.Remove(shift);
            db.SaveChanges();
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
