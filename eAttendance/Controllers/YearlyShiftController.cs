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
    public class YearlyShiftController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /YearlyShift/
        public ActionResult Index()
        {
            YearlyShift model = new YearlyShift();
            model.YearlyShiftList = new List<YearlyShift>();
            model.YearlyShiftList = db.YearlyShift.ToList();
            return View(model);
        }

        // GET: /YearlyShift/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearlyShift yearlyshift = db.YearlyShift.Find(id);
            if (yearlyshift == null)
            {
                return HttpNotFound();
            }
            return View(yearlyshift);
        }

        // GET: /YearlyShift/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /YearlyShift/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(YearlyShift yearlyshift)
        {
            if (ModelState.IsValid)
            {
                yearlyshift.CreatedDate = DateTime.Now;
                yearlyshift.ModifiedDate = DateTime.Now;
                yearlyshift.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                yearlyshift.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                yearlyshift.Status = 1;
                yearlyshift.StartDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(yearlyshift.NStartDate));
                yearlyshift.EndDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(yearlyshift.NEndDate));
                yearlyshift.flag = 0;

                db.YearlyShift.Add(yearlyshift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yearlyshift);
        }

        // GET: /YearlyShift/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearlyShift yearlyshift = db.YearlyShift.Find(id);
            if (yearlyshift == null)
            {
                return HttpNotFound();
            }
            return View(yearlyshift);
        }

        // POST: /YearlyShift/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(YearlyShift yearlyshift)
        {
            if (ModelState.IsValid)
            {
                yearlyshift.ModifiedDate = DateTime.Now;
                yearlyshift.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);

                yearlyshift.StartDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(yearlyshift.NStartDate));
                yearlyshift.EndDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(yearlyshift.NEndDate));

                db.Entry(yearlyshift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yearlyshift);
        }

        // GET: /YearlyShift/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearlyShift yearlyshift = db.YearlyShift.Find(id);
            if (yearlyshift == null)
            {
                return HttpNotFound();
            }
            return View(yearlyshift);
        }

        // POST: /YearlyShift/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(YearlyShift model)
        {
            YearlyShift yearlyshift = db.YearlyShift.Find(model.YearlyShiftId);
            db.YearlyShift.Remove(yearlyshift);
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
