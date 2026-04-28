using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eAttendance.Models;
using PagedList;

namespace eAttendance.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class SetupDesignationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupDesignation/
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = (sortOrder == "Date") ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            IEnumerable<DesignationSetUp> source = db.DesignationSetUp;
            if (!string.IsNullOrEmpty(searchString))
            {

                source = source.Where(x => x.DesignationName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    source = from s in source
                             orderby s.DesignationName descending
                             select s;
                    break;

                case "Date":
                    source = from s in source
                             orderby s.CreatedDate descending
                             select s;
                    break;

                case "date_desc":
                    source = from s in source
                             orderby s.CreatedDate descending
                             select s;
                    break;

                default:
                    source = from s in source
                             orderby s.DesignationName
                             select s;
                    break;
            }
            int num = 10;
            int? nullable = page;
            int num2 = nullable.HasValue ? nullable.GetValueOrDefault() : 1;
            var list = PagedListExtensions.ToPagedList<DesignationSetUp>(source, num2, num);
            return base.View(list);
        }

        // GET: /SetupDesignation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignationSetUp designationsetup = db.DesignationSetUp.Find(id);
            if (designationsetup == null)
            {
                return HttpNotFound();
            }
            return View(designationsetup);
        }

        // GET: /SetupDesignation/Create
        public ActionResult Add()
        {
            ViewBag.LevelId = new SelectList(db.LevelSetUp, "LevelId", "LevelName");
            return View();
        }

        // POST: /SetupDesignation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DesignationSetUp designationsetup)
        {
            if (ModelState.IsValid)
            {
                designationsetup.CreatedDate = DateTime.Now;
                designationsetup.ModifiedDate = DateTime.Now;
                designationsetup.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                designationsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                designationsetup.Status = 1;
                db.DesignationSetUp.Add(designationsetup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LevelId = new SelectList(db.LevelSetUp, "LevelId", "LevelName", designationsetup.LevelId);
            return View(designationsetup);
        }

        // GET: /SetupDesignation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignationSetUp designationsetup = db.DesignationSetUp.Find(id);
            if (designationsetup == null)
            {
                return HttpNotFound();
            }
          
            return View(designationsetup);
        }

        // POST: /SetupDesignation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DesignationSetUp designationsetup)
        {
            if (ModelState.IsValid)
            {
                designationsetup.ModifiedDate = DateTime.Now;

                designationsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                designationsetup.Status = 1;
                db.Entry(designationsetup).State = EntityState.Modified;
              
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LevelId = new SelectList(db.LevelSetUp, "LevelId", "LevelName", designationsetup.LevelId);
            return View(designationsetup);
        }

        // GET: /SetupDesignation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignationSetUp designationsetup = db.DesignationSetUp.Find(id);
            if (designationsetup == null)
            {
                return HttpNotFound();
            }
            return View(designationsetup);
        }

        // POST: /SetupDesignation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DesignationSetUp model)
        {
            DesignationSetUp designationsetup = db.DesignationSetUp.Find(model.DesignationId);
            db.DesignationSetUp.Remove(designationsetup);
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
