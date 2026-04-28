using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eAttendance.Models;

namespace eAttendance.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator")]
    public class SetupLeaveTypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupLeaveType/
        public ActionResult Index()
        {
            var leavetypesetup = db.LeaveTypeSetUp.Include(l => l.OfficeSetUp);
            LeaveTypeSetUp model = new LeaveTypeSetUp();
            model.SetupLeaveTypeList = new List<LeaveTypeSetUp>();
            model.SetupLeaveTypeList = leavetypesetup.ToList();
            return View(model);
        }

        // GET: /SetupLeaveType/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveTypeSetUp leavetypesetup = await db.LeaveTypeSetUp.FindAsync(id);
            if (leavetypesetup == null)
            {
                return HttpNotFound();
            }
            return View(leavetypesetup);
        }

        // GET: /SetupLeaveType/Create
        public ActionResult Add()
        {
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName");
            return View();
        }

        // POST: /SetupLeaveType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(LeaveTypeSetUp leavetypesetup)
        {
            if (ModelState.IsValid)
            {
                leavetypesetup.CreatedDate = DateTime.Now;
                leavetypesetup.Status = 1;
                leavetypesetup.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                leavetypesetup.ModifiedDate = DateTime.Now;        
                leavetypesetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                db.LeaveTypeSetUp.Add(leavetypesetup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", leavetypesetup.OfficeId);
            return View(leavetypesetup);
        }

        // GET: /SetupLeaveType/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveTypeSetUp leavetypesetup = await db.LeaveTypeSetUp.FindAsync(id);
            if (leavetypesetup == null)
            {
                return HttpNotFound();
            }
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", leavetypesetup.OfficeId);
            return View(leavetypesetup);
        }

        // POST: /SetupLeaveType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveTypeSetUp leavetypesetup)
        {
            if (ModelState.IsValid)
            {
                leavetypesetup.ModifiedDate = DateTime.Now;
                leavetypesetup.Status = 1;
                leavetypesetup.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                db.Entry(leavetypesetup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", leavetypesetup.OfficeId);
            return View(leavetypesetup);
        }

        // GET: /SetupLeaveType/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveTypeSetUp leavetypesetup = await db.LeaveTypeSetUp.FindAsync(id);
            if (leavetypesetup == null)
            {
                return HttpNotFound();
            }
            return View(leavetypesetup);
        }

        // POST: /SetupLeaveType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LeaveTypeSetUp leavetypesetup = await db.LeaveTypeSetUp.FindAsync(id);
            db.LeaveTypeSetUp.Remove(leavetypesetup);
            await db.SaveChangesAsync();
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
