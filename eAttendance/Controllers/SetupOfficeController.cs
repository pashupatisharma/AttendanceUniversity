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
    public class OfficeSetupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupOffice/
        public ActionResult Index()
        {
            OfficeSetUp model = new OfficeSetUp();
            model.SetupOfficeList = new List<OfficeSetUp>();
            model.SetupOfficeList = db.OfficeSetUp.ToList();
            return View(model);
        }

        // GET: /SetupOffice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeSetUp officesetup = db.OfficeSetUp.Find(id);
            if (officesetup == null)
            {
                return HttpNotFound();
            }
            return View(officesetup);
        }

        // GET: /SetupOffice/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /SetupOffice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(OfficeSetUp officesetup)
        {
            if (ModelState.IsValid)
            {
                officesetup.CreatedDate = DateTime.Now;
                officesetup.ModifiedDate = DateTime.Now;
                officesetup.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                officesetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                officesetup.Status = 1;
                db.OfficeSetUp.Add(officesetup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(officesetup);
        }

        // GET: /SetupOffice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeSetUp officesetup = db.OfficeSetUp.Find(id);
            if (officesetup == null)
            {
                return HttpNotFound();
            }
            return View(officesetup);
        }

        // POST: /SetupOffice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( OfficeSetUp officesetup)
        {
            if (ModelState.IsValid)
            {
               // officesetup.CreatedDate = DateTime.Now;
                officesetup.ModifiedDate = DateTime.Now;
               // officesetup.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                officesetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                officesetup.Status = 1;

                db.Entry(officesetup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(officesetup);
        }

        // GET: /SetupOffice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OfficeSetUp officesetup = db.OfficeSetUp.Find(id);
            if (officesetup == null)
            {
                return HttpNotFound();
            }
            return View(officesetup);
        }

        // POST: /SetupOffice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(OfficeSetUp model)
        {
            OfficeSetUp officesetup = db.OfficeSetUp.Find(model.OfficeId);
            db.OfficeSetUp.Remove(officesetup);
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
