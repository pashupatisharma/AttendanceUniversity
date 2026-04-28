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
    public class SetupBranchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

       
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
            IEnumerable<BranchSetUp> source = db.BranchSetUp;
            if (!string.IsNullOrEmpty(searchString))
            {

                source = source.Where(x => x.BranchName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    source = from s in source
                             orderby s.BranchName descending
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
                             orderby s.BranchName
                             select s;
                    break;
            }
            int num = 10;
            int? nullable = page;
            int num2 = nullable.HasValue ? nullable.GetValueOrDefault() : 1;
            ViewBag.OnePageOfBranches = PagedListExtensions.ToPagedList<BranchSetUp>(source, num2, num);
            return base.View();
        }


        // GET: /SetupBranch/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchSetUp branchsetup = db.BranchSetUp.Find(id);
            if (branchsetup == null)
            {
                return HttpNotFound();
            }
            return View(branchsetup);
        }

        // GET: /SetupBranch/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /SetupBranch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(BranchSetUp branchsetup)
        {
            if (ModelState.IsValid)
            {
                branchsetup.CreatedDate = DateTime.Now;
                branchsetup.ModifiedDate = DateTime.Now;
                branchsetup.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                branchsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                db.BranchSetUp.Add(branchsetup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(branchsetup);
        }

        // GET: /SetupBranch/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchSetUp branchsetup = db.BranchSetUp.Find(id);
            if (branchsetup == null)
            {
                return HttpNotFound();
            }
            return View(branchsetup);
        }

        // POST: /SetupBranch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BranchSetUp branchsetup)
        {
            if (ModelState.IsValid)
            {
             
                branchsetup.ModifiedDate = DateTime.Now;
               
                branchsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                db.Entry(branchsetup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branchsetup);
        }

        // GET: /SetupBranch/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchSetUp branchsetup = db.BranchSetUp.Find(id);
            if (branchsetup == null)
            {
                return HttpNotFound();
            }
            return View(branchsetup);
        }

        // POST: /SetupBranch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(BranchSetUp model)
        {
            BranchSetUp branchsetup = db.BranchSetUp.Find(model.BranchId);
            db.BranchSetUp.Remove(branchsetup);
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
