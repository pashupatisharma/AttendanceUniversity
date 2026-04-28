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
    public class SetupOfficeDeviceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupOfficeDevice/
        public ActionResult Index()
        {
            //var officedevicesetup = db.OfficeDeviceSetUp.Include(o => o.OfficeSetUp);
            //return View(officedevicesetup.ToList());

            OfficeDeviceSetUp model = new OfficeDeviceSetUp();
            model.SetupOfficeDeviceList = new List<OfficeDeviceSetUp>();
            model.SetupOfficeDeviceList = db.OfficeDeviceSetUp.ToList();
            return View(model);
        }

        // GET: /SetupOfficeDevice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeDeviceSetUp officedevicesetup = db.OfficeDeviceSetUp.Find(id);
            if (officedevicesetup == null)
            {
                return HttpNotFound();
            }
            return View(officedevicesetup);
        }

        // GET: /SetupOfficeDevice/Create
        public ActionResult Add()
        {
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName");
            return View();
        }

        // POST: /SetupOfficeDevice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(OfficeDeviceSetUp officedevicesetup)
        {
            if (ModelState.IsValid)
            {
                officedevicesetup.LastImportDate = DateTime.Now;
                officedevicesetup.Status = 1;
                db.OfficeDeviceSetUp.Add(officedevicesetup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", officedevicesetup.OfficeId);
            return View(officedevicesetup);
        }

        // GET: /SetupOfficeDevice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeDeviceSetUp officedevicesetup = db.OfficeDeviceSetUp.Find(id);
            if (officedevicesetup == null)
            {
                return HttpNotFound();
            }
         
            return View(officedevicesetup);
        }

        // POST: /SetupOfficeDevice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OfficeDeviceSetUp officedevicesetup)
        {
            if (ModelState.IsValid)
            {
                officedevicesetup.LastImportDate = DateTime.Now;
                officedevicesetup.Status = 1;
                db.Entry(officedevicesetup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", officedevicesetup.OfficeId);
            return View(officedevicesetup);
        }

        // GET: /SetupOfficeDevice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeDeviceSetUp officedevicesetup = db.OfficeDeviceSetUp.Find(id);
            if (officedevicesetup == null)
            {
                return HttpNotFound();
            }
            return View(officedevicesetup);
        }

        // POST: /SetupOfficeDevice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(OfficeDeviceSetUp model)
        {
            OfficeDeviceSetUp officedevicesetup = db.OfficeDeviceSetUp.Find(model.OfficeDeviceId);
            db.OfficeDeviceSetUp.Remove(officedevicesetup);
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
