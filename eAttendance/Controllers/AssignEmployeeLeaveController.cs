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
    public class AssignEmployeeLeaveController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /AssignEmployeeLeave/
        public ActionResult Index()
        {
            //FiscalYearSetUp model = new FiscalYearSetUp();
            //FiscalYearSetUp year =db.FiscalYearSetUp.FirstOrDefault();
            ViewBag.FiscalYears = new SelectList(db.FiscalYearSetUp.ToList(), "FiscalYearId", "FiscalYearName");

            return base.View();
        }

        //public PartialViewResult List(string branchId, string serviceId, string levelId, string designationId, string search, int fiscalYearId = 0)
        //{
        //    var model = new AssignEmployeeLeave();

        //    int? officeId = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);
        //    if (fiscalYearId == 0)
        //    {
        //        fiscalYearId = db.FiscalYearSetUp.Where(x => x.IsActive == true).FirstOrDefault().FiscalYearId;
        //    }

        //    model.LeaveTypes = new List<LeaveTypeSetUp>();
        //    model.LeaveTypes = db.LeaveTypeSetUp.ToList();
        //    model.Employees = new List<EmployeeInfo>();
        //    model.Employees = db.EmployeeInfo.ToList();
        //    model.AssignEmployeeLeaveList = new List<AssignEmployeeLeave>();
        //    model.AssignEmployeeLeaveList = db.AssignEmployeeLeave.Where(x => x.FiscalYearId == fiscalYearId).ToList();
        //    return this.PartialView("_List", model);
        //}


        public ActionResult Add()
        {
            AssignEmployeeLeave model = new AssignEmployeeLeave();
            return base.View(model);
        }
        [HttpPost]
        public ActionResult Add(AssignEmployeeLeave model)
        {

            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                model.OfficeId = Utility.GetOfficeIdByEmployeeId(model.EmployeeId);
                model.CreatedBy = userIdByUserName;
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;

                db.AssignEmployeeLeave.Add(model);
                db.SaveChanges();

            }
            catch
            {

            }
            return base.RedirectToAction("Index", "AssignEmployeeLeave");
        }


        public ActionResult AddList(int FiscalYear = 0, int OfficeId=0)
        {
            AssignEmployeeLeave model = new AssignEmployeeLeave();
            model.FiscalYearId = FiscalYear;
            model.OfficeId = OfficeId;
            model.EmployeeInfo = new EmployeeInfo();
            var getofficeid = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);

            model.EmployeeInfo.EmoloyeeList = new List<EmployeeInfo>();

            var emplist = db.EmployeeInfo.Where(x=>x.Status!=2).ToList();
            if (OfficeId > 0)
            {
               // var emplist = db.EmployeeInfo.ToList();
                foreach (var item in emplist)
                {



                    var d = db.EmployeeOfficeDetail.Where(x=>x.EmployeeId==item.EmployeeId).Where(x => x.OfficeId == OfficeId).FirstOrDefault();

                    if (d != null)
                    {
                        
                            EmployeeInfo obj = new EmployeeInfo();
                            obj.LeaveTypes = new List<LeaveTypeSetUp>();
                            obj = item;
                            obj.LeaveTypes = db.LeaveTypeSetUp.ToList();
                            model.EmployeeInfo.EmoloyeeList.Add(obj);
                        
                    }


                }
            }
            else
            {

                foreach (var item in emplist)
                {


                    if (User.IsInRole("SuperAdmin"))
                    {

                        EmployeeInfo obj = new EmployeeInfo();
                        obj.LeaveTypes = new List<LeaveTypeSetUp>();
                        obj = item;
                        obj.LeaveTypes = db.LeaveTypeSetUp.ToList();
                        model.EmployeeInfo.EmoloyeeList.Add(obj);
                    }
                    else
                    {


                        var d = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();

                        if (d != null)
                        {
                            if (d.OfficeId == getofficeid)
                            {
                                EmployeeInfo obj = new EmployeeInfo();
                                obj.LeaveTypes = new List<LeaveTypeSetUp>();
                                obj = item;
                                obj.LeaveTypes = db.LeaveTypeSetUp.ToList();
                                model.EmployeeInfo.EmoloyeeList.Add(obj);
                            }
                        }

                    }
                }

            }

            return base.View(model);
        }



        // GET: /AssignEmployeeLeave/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignEmployeeLeave assignemployeeleave = await db.AssignEmployeeLeave.FindAsync(id);
            if (assignemployeeleave == null)
            {
                return HttpNotFound();
            }
            return View(assignemployeeleave);
        }

        // GET: /AssignEmployeeLeave/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.EmployeeInfo, "EmployeeId", "EmployeeNameNp");
            ViewBag.FiscalYearId = new SelectList(db.FiscalYearSetUp, "FiscalYearId", "FiscalYearName");
            ViewBag.LeaveTypeId = new SelectList(db.LeaveTypeSetUp, "LeaveTypeId", "LeaveTypeName");
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName");
            return View();
        }

        // POST: /AssignEmployeeLeave/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LeaveApplicationId,FiscalYearId,EmployeeId,OfficeId,LeaveTypeId,OpenningBalance,NoOfLeave,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] AssignEmployeeLeave assignemployeeleave)
        {
            if (ModelState.IsValid)
            {
                db.AssignEmployeeLeave.Add(assignemployeeleave);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.EmployeeInfo, "EmployeeId", "EmployeeNameNp", assignemployeeleave.EmployeeId);
            ViewBag.FiscalYearId = new SelectList(db.FiscalYearSetUp, "FiscalYearId", "FiscalYearName", assignemployeeleave.FiscalYearId);
            ViewBag.LeaveTypeId = new SelectList(db.LeaveTypeSetUp, "LeaveTypeId", "LeaveTypeName", assignemployeeleave.LeaveTypeId);
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", assignemployeeleave.OfficeId);
            return View(assignemployeeleave);
        }

        // GET: /AssignEmployeeLeave/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignEmployeeLeave assignemployeeleave = await db.AssignEmployeeLeave.FindAsync(id);
            if (assignemployeeleave == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.EmployeeInfo, "EmployeeId", "EmployeeNameNp", assignemployeeleave.EmployeeId);
            ViewBag.FiscalYearId = new SelectList(db.FiscalYearSetUp, "FiscalYearId", "FiscalYearName", assignemployeeleave.FiscalYearId);
            ViewBag.LeaveTypeId = new SelectList(db.LeaveTypeSetUp, "LeaveTypeId", "LeaveTypeName", assignemployeeleave.LeaveTypeId);
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", assignemployeeleave.OfficeId);
            return View(assignemployeeleave);
        }

        // POST: /AssignEmployeeLeave/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LeaveApplicationId,FiscalYearId,EmployeeId,OfficeId,LeaveTypeId,OpenningBalance,NoOfLeave,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] AssignEmployeeLeave assignemployeeleave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignemployeeleave).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.EmployeeInfo, "EmployeeId", "EmployeeNameNp", assignemployeeleave.EmployeeId);
            ViewBag.FiscalYearId = new SelectList(db.FiscalYearSetUp, "FiscalYearId", "FiscalYearName", assignemployeeleave.FiscalYearId);
            ViewBag.LeaveTypeId = new SelectList(db.LeaveTypeSetUp, "LeaveTypeId", "LeaveTypeName", assignemployeeleave.LeaveTypeId);
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", assignemployeeleave.OfficeId);
            return View(assignemployeeleave);
        }

        // GET: /AssignEmployeeLeave/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignEmployeeLeave assignemployeeleave = await db.AssignEmployeeLeave.FindAsync(id);
            if (assignemployeeleave == null)
            {
                return HttpNotFound();
            }
            return View(assignemployeeleave);
        }

        // POST: /AssignEmployeeLeave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssignEmployeeLeave assignemployeeleave = await db.AssignEmployeeLeave.FindAsync(id);
            db.AssignEmployeeLeave.Remove(assignemployeeleave);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SaveAssignLeave(AssignEmployeeLeave model)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);

    
                {
               
                        db.SaveChanges();
                    foreach (var emp in model.EmployeeInfo.EmoloyeeList)
                    {
                        foreach (var item in emp.LeaveTypes)
                        {
                            var leave = db.AssignEmployeeLeave.Where(x => x.EmployeeId == emp.EmployeeId && x.FiscalYearId == model.FiscalYearId && x.LeaveTypeId == item.LeaveTypeId).FirstOrDefault();
                            if (leave != null)
                            {
                                leave.FiscalYearId = model.FiscalYearId;
                                leave.EmployeeId = emp.EmployeeId;
                                leave.LeaveTypeId = item.LeaveTypeId;
                                leave.OfficeId = model.OfficeId;
                                leave.OpenningBalance = item.OpenningBalance;

                                leave.NoOfLeave = item.NoOfLeave;
                                leave.CreatedBy = userIdByUserName;
                                leave.CreatedDate = DateTime.Now;
                                leave.ModifiedDate = DateTime.Now;
                                leave.Status = 1;

                                db.SaveChanges();
                            }
                            else
                            {
                                leave = new AssignEmployeeLeave();
                                leave.FiscalYearId = model.FiscalYearId;
                                leave.EmployeeId = emp.EmployeeId;
                                leave.LeaveTypeId = item.LeaveTypeId;
                                leave.OfficeId = model.OfficeId;
                                leave.OpenningBalance = item.OpenningBalance;

                                leave.NoOfLeave = item.NoOfLeave;
                                leave.CreatedBy = userIdByUserName;
                                leave.CreatedDate = DateTime.Now;
                                leave.ModifiedDate = DateTime.Now;
                                leave.Status = 1;

                                db.AssignEmployeeLeave.Add(leave);
                                db.SaveChanges();
                            }
                        
                        


                        }
                    }

                }
            }

            catch (Exception ex)
            {

            }

            return base.RedirectToAction("AddList", "AssignEmployeeLeave");

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
