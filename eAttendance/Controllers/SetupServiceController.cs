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
    public class SetupServiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupService/
        public ActionResult Index()
        {
            ServiceSetUp model = new ServiceSetUp();
            model.ServiceSetUpList = new List<ServiceSetUp>();
            model.ServiceSetUpList = db.ServiceSetUp.ToList();
            return View(model);
        }

        // GET: /SetupService/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceSetUp servicesetup = db.ServiceSetUp.Find(id);
            if (servicesetup == null)
            {
                return HttpNotFound();
            }
            return View(servicesetup);
        }

        // GET: /SetupService/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /SetupService/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ServiceSetUp servicesetup)
        {
            if (ModelState.IsValid)
            {
                servicesetup.CreatedDate = DateTime.Now;
                servicesetup.ModifiedDate = DateTime.Now;
                servicesetup.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                servicesetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                servicesetup.Status = 1;
                db.ServiceSetUp.Add(servicesetup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(servicesetup);
        }

        // GET: /SetupService/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceSetUp servicesetup = db.ServiceSetUp.Find(id);
            if (servicesetup == null)
            {
                return HttpNotFound();
            }
            return View(servicesetup);
        }

        // POST: /SetupService/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceSetUp servicesetup)
        {
            if (ModelState.IsValid)
            {
                servicesetup.ModifiedDate = DateTime.Now;
                servicesetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                servicesetup.Status = 1;
                db.Entry(servicesetup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(servicesetup);
        }

        // GET: /SetupService/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceSetUp servicesetup = db.ServiceSetUp.Find(id);
            if (servicesetup == null)
            {
                return HttpNotFound();
            }
            return View(servicesetup);
        }

        // POST: /SetupService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ServiceSetUp model)
        {
            ServiceSetUp servicesetup = db.ServiceSetUp.Find(model.ServiceId);
            db.ServiceSetUp.Remove(servicesetup);
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
