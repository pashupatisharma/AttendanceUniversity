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
    public class SetupFiscalYearController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupFiscalYear/
        public ActionResult Index()
        {
            FiscalYearSetUp model = new FiscalYearSetUp();
            model.SetupFiscalYearList = new List<FiscalYearSetUp>();
            model.SetupFiscalYearList = db.FiscalYearSetUp.ToList();
            return View(model);
        }

        // GET: /SetupFiscalYear/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FiscalYearSetUp fiscalyearsetup = db.FiscalYearSetUp.Find(id);
            if (fiscalyearsetup == null)
            {
                return HttpNotFound();
            }
            return View(fiscalyearsetup);
        }

        // GET: /SetupFiscalYear/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: /SetupFiscalYear/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FiscalYearSetUp fiscalyearsetup)
        {
            if (ModelState.IsValid)
            {
                fiscalyearsetup.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                fiscalyearsetup.Status = 1;
                fiscalyearsetup.CreatedDate = DateTime.Now;
                fiscalyearsetup.FromDate =
                    NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(fiscalyearsetup.NFromDate));
                fiscalyearsetup.ToDate =
                    NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(fiscalyearsetup.NToDate));
                fiscalyearsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                fiscalyearsetup.ModifiedDate = DateTime.Now;
                db.FiscalYearSetUp.Add(fiscalyearsetup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fiscalyearsetup);
        }

        // GET: /SetupFiscalYear/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FiscalYearSetUp fiscalyearsetup = db.FiscalYearSetUp.Find(id);
            fiscalyearsetup.NFromDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(fiscalyearsetup.FromDate), "yyyy-MM-DD");
            fiscalyearsetup.NToDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(fiscalyearsetup.ToDate), "yyyy-MM-DD");
            if (fiscalyearsetup == null)
            {
                return HttpNotFound();
            }
            return View(fiscalyearsetup);
        }

        // POST: /SetupFiscalYear/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FiscalYearSetUp fiscalyearsetup)
        {
            if (ModelState.IsValid)
            {
                fiscalyearsetup.Status = 1;
                fiscalyearsetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                fiscalyearsetup.FromDate =
                    NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(fiscalyearsetup.NFromDate));
                fiscalyearsetup.ToDate =
                    NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(fiscalyearsetup.NToDate));
                fiscalyearsetup.ModifiedDate = DateTime.Now;
                db.Entry(fiscalyearsetup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fiscalyearsetup);
        }

        // GET: /SetupFiscalYear/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FiscalYearSetUp fiscalyearsetup = db.FiscalYearSetUp.Find(id);
            if (fiscalyearsetup == null)
            {
                return HttpNotFound();
            }
            return View(fiscalyearsetup);
        }

        // POST: /SetupFiscalYear/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(FiscalYearSetUp model)
        {
            FiscalYearSetUp fiscalyearsetup = db.FiscalYearSetUp.Find(model.FiscalYearId);
            db.FiscalYearSetUp.Remove(fiscalyearsetup);
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
