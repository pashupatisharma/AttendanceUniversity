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
    public class SetupDistrictController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupDistrict/
        public ActionResult Index()
        {
            DistrictSetUp model = new DistrictSetUp();
            model.SetupDistrictList = new List<DistrictSetUp>();
            model.SetupDistrictList = db.DistrictSetUp.ToList();
            return View(model);
        }

        // GET: /SetupDistrict/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DistrictSetUp districtsetup = db.DistrictSetUp.Find(id);
            if (districtsetup == null)
            {
                return HttpNotFound();
            }
            return View(districtsetup);
        }

        // GET: /SetupDistrict/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /SetupDistrict/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DistrictSetUp districtsetup)
        {
            if (ModelState.IsValid)
            {
                districtsetup.CreatedDate = DateTime.Now;
                districtsetup.ModifiedDate = DateTime.Now;
                districtsetup.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                districtsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                districtsetup.Status = 1;
                db.DistrictSetUp.Add(districtsetup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(districtsetup);
        }

        // GET: /SetupDistrict/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DistrictSetUp districtsetup = db.DistrictSetUp.Find(id);
            if (districtsetup == null)
            {
                return HttpNotFound();
            }
            return View(districtsetup);
        }

        // POST: /SetupDistrict/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DistrictSetUp districtsetup)
        {
            if (ModelState.IsValid)
            {
                districtsetup.ModifiedDate = DateTime.Now;
                districtsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
              
                db.Entry(districtsetup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(districtsetup);
        }

        // GET: /SetupDistrict/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DistrictSetUp districtsetup = db.DistrictSetUp.Find(id);
            if (districtsetup == null)
            {
                return HttpNotFound();
            }
            return View(districtsetup);
        }

        // POST: /SetupDistrict/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DistrictSetUp model)
        {
            DistrictSetUp districtsetup = db.DistrictSetUp.Find(model.DistrictId);
            db.DistrictSetUp.Remove(districtsetup);
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
