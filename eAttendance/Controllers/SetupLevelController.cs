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
    public class SetupLevelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupLevel/
        public ActionResult Index()
        {
            LevelSetUp model = new LevelSetUp();
            model.SetupLevelList = new List<LevelSetUp>();
            model.SetupLevelList = db.LevelSetUp.ToList();
            return View(model);
           
        }

        // GET: /SetupLevel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelSetUp levelsetup = db.LevelSetUp.Find(id);
            if (levelsetup == null)
            {
                return HttpNotFound();
            }
            return View(levelsetup);
        }

        // GET: /SetupLevel/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /SetupLevel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(LevelSetUp levelsetup)
        {
            if (ModelState.IsValid)
            {
                levelsetup.CreatedDate = DateTime.Now;
                levelsetup.ModifiedDate = DateTime.Now;
                levelsetup.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                levelsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                db.LevelSetUp.Add(levelsetup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(levelsetup);
        }

        // GET: /SetupLevel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelSetUp levelsetup = db.LevelSetUp.Find(id);
            if (levelsetup == null)
            {
                return HttpNotFound();
            }
            return View(levelsetup);
        }

        // POST: /SetupLevel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LevelSetUp levelsetup)
        {
            if (ModelState.IsValid)
            {
               
                levelsetup.ModifiedDate = DateTime.Now;
              
                levelsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                db.Entry(levelsetup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(levelsetup);
        }

        // GET: /SetupLevel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelSetUp levelsetup = db.LevelSetUp.Find(id);
            if (levelsetup == null)
            {
                return HttpNotFound();
            }
            return View(levelsetup);
        }

        // POST: /SetupLevel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(LevelSetUp model)
        {
            LevelSetUp levelsetup = db.LevelSetUp.Find(model.LevelId);
            db.LevelSetUp.Remove(levelsetup);
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
