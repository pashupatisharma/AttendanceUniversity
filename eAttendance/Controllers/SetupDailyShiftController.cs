using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using eAttendance.Models;

namespace eAttendance.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class SetupDailyShiftController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupDailyShift/
        public ActionResult Index()
        {
            var list = db.ShiftType
                         .Where(x => x.Status != 2) // not deleted
                         .OrderBy(x => x.ShiftTypeId)
                         .ToList();

            return View(list);
        }

        // GET: /SetupDailyShift/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var shift = db.ShiftType.Find(id);

            if (shift == null)
                return HttpNotFound();

            return View(shift);
        }

        // GET: /SetupDailyShift/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: /SetupDailyShift/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ShiftType shift)
        {
            if (ModelState.IsValid)
            {
                shift.IsActive = true;
                shift.Status = 1;

                db.ShiftType.Add(shift);
                db.SaveChanges();

                TempData["Message"] = "Success";
                return RedirectToAction("Index");
            }

            return View(shift);
        }

        // GET: /SetupDailyShift/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var shift = db.ShiftType.Find(id);

            if (shift == null)
                return HttpNotFound();

            return View(shift);
        }

        // POST: /SetupDailyShift/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShiftType shift)
        {
            if (ModelState.IsValid)
            {
                var existing = db.ShiftType.Find(shift.ShiftTypeId);

                if (existing == null)
                    return HttpNotFound();

                existing.Name = shift.Name;
                existing.IsActive = shift.IsActive;
                existing.Status = 1;

                db.SaveChanges();

                TempData["Message"] = "Updated";
                return RedirectToAction("Index");
            }

            return View(shift);
        }

        // GET: /SetupDailyShift/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var shift = db.ShiftType.Find(id);

            if (shift == null)
                return HttpNotFound();

            return View(shift);
        }


        // POST: /SetupDailyShift/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ShiftTypeId)
        {
            var shift = db.ShiftType.Find(ShiftTypeId);

            if (shift == null)
                return HttpNotFound();

            shift.Status = 2;
            shift.IsActive = false;

            db.SaveChanges();

            TempData["Message"] = "Deleted";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}