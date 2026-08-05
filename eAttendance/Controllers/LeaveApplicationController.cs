using eAttendance.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace eAttendance.Controllers
{
    [Authorize]
    public class LeaveApplicationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /LeaveApplication/
        public ActionResult Index()
        {
            LeaveApplicationModel Model = new LeaveApplicationModel();
            Model.LeaveApplicationList = new List<LeaveApplicationModel>();

            int? _officeId = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);
            var userId = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
            var empid = db.EmployeeInfo.Where(x => x.UserId == userId).FirstOrDefault().EmployeeId;
            string s = "LeaveRequestList" + " " + "'" + _officeId + "'" + "," + empid;
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 180;

            var list = db.Database.SqlQuery<LeaveApplicationModel>(s).ToList();
            Model.LeaveApplicationList = list;
            return View(Model);
        }

        public ActionResult Add()
        {
            LeaveApplication model = new LeaveApplication
            {
                EmployeeId = EmployeeProvider.GetEmployeeIdByUserName(User.Identity.Name)
            };
            return base.View(model);
        }

        [HttpPost]
        public ActionResult Add(LeaveApplication model)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);

                model.EmployeeId = EmployeeProvider.GetEmployeeIdByUserName(User.Identity.Name);
                model.ApplicationDate = new DateTime?(DateTime.Now);
                DateTime frmDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate));
                DateTime toDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate));
                if (CheckValidLeaveOrVisitDateInterval(frmDate, toDate, model.EmployeeId, 0))
                {
                    TempData.Add("Message", "छान्नु भएको कर्मचारी बिदा वा काज मा पहिले देखि नै छ।");
                    return base.RedirectToAction("Index", "LeaveApplication");
                }
                model.FromDate = new DateTime?(frmDate);
                model.ToDate = new DateTime?(toDate);
                if (toDate >= frmDate)
                {
                    TimeSpan span = (TimeSpan)(toDate - frmDate);
                    model.TotalDays = span.Days + 1;
                }
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.CreatedBy = userIdByUserName;
                model.RecomenderStatus = 1;
                model.ApprovedStatus = 1;
                model.Type = 1;
                model.Status = 1;
                db.LeaveApplication.Add(model);
                db.SaveChanges();
                TempData["Message"] = "Leave application created successfully.";
            }
            catch
            {
                base.TempData.Add("Message", "Failed");
            }
            return base.RedirectToAction("Index", "LeaveApplication");
        }

        private bool CheckValidLeaveOrVisitDateInterval(DateTime frmDate, DateTime toDate, int? empId, int leaveApplicationId)
        {
            bool exist = false;
            for (DateTime day = frmDate.Date; day.Date <= toDate.Date; day = day.AddDays(1.0))
            {
                var leave = db.LeaveApplication.Where(x => x.EmployeeId == empId).Where(x => x.ApplicationDate == day).FirstOrDefault();
                if (leave != null)
                {
                    exist = true;
                    return exist;
                }

                var Visit = db.VisitApplication.Where(x => x.EmployeeId == empId).Where(x => x.ApplicationDate == day).FirstOrDefault();
                if (Visit != null)
                {
                    exist = true;
                    return exist;
                }
            }
            return exist;
        }

        // GET: /LeaveApplication/Edit/5
        public ActionResult Edit(int? id)
        {
            LeaveApplication model = new LeaveApplication();
            if (id.HasValue)
            {
                model = db.LeaveApplication.Where(x => x.LeaveApplicationId == id).FirstOrDefault();
                model.NApplicationDate = model.ApplicationDate.HasValue ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ApplicationDate.Value.Date), "yyyy-MM-DD") : " ";
                model.NFromDate = model.FromDate.HasValue ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.FromDate.Value.Date), "yyyy-MM-DD") : " ";
                model.NToDate = model.ToDate.HasValue ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ToDate.Value.Date), "yyyy-MM-DD") : " ";
                model.NApprovedDate = model.ApprovedDate.HasValue ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ApprovedDate.Value.Date), "yyyy-MM-DD") : " ";
            }
            return base.View(model);
        }

        // POST: /LeaveApplication/Edit/5
        [HttpPost]
        public ActionResult Edit(LeaveApplication model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                    model.ModifiedBy = userIdByUserName;
                    model.ModifiedDate = DateTime.Now;
                    model.EmployeeId = EmployeeProvider.GetEmployeeIdByUserName(User.Identity.Name);
                    DateTime frmDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate));
                    DateTime toDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate));
                    if (CheckValidLeaveOrVisitDateInterval(frmDate, toDate, model.EmployeeId, 0))
                    {
                        TempData.Add("Message", "छान्नु भएको कर्मचारी बिदा वा काज मा पहिले देखि नै छ।");
                        return base.RedirectToAction("Index", "LeaveApplication");
                    }
                    model.FromDate = new DateTime?(frmDate);
                    model.ToDate = new DateTime?(toDate);
                    if (toDate >= frmDate)
                    {
                        TimeSpan span = (TimeSpan)(toDate - frmDate);
                        model.TotalDays = span.Days + 1;
                    }
                    model.ModifiedDate = DateTime.Now;
                    model.RecomenderStatus = 1;
                    model.ApprovedStatus = 1;
                    model.Status = 1;
                    model.Type = 1;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Leave application updated successfully.";
                }
                catch
                {
                }
                return base.RedirectToAction("Index", "LeaveApplication");
            }
            return base.View();
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult LeavePosting(
    string nFromDate,
    string nToDate,
    string officeId,
    string branchId,
    string serviceId,
    string levelId,
    string designationId,
    string empId,
    string sortOrder,
    int? pageSize,
    int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = (sortOrder == "Date") ? "date_desc" : "Date";

            page = page ?? 1;
            int pageNumber = page.Value;
            int pageLength = pageSize ?? 10;

            int? currentOfficeId = null;

            if (User.IsInRole("Admin"))
            {
                var userName = User.Identity.Name;

                currentOfficeId =
                    (from u in db.Users
                     join e in db.EmployeeInfo on u.Id equals e.UserId
                     join o in db.EmployeeOfficeDetail on e.EmployeeId equals o.EmployeeId
                     where u.UserName == userName
                     select (int?)o.OfficeId).FirstOrDefault();
            }

            var query =
                from leave in db.LeaveApplication
                join emp in db.EmployeeInfo
                    on leave.EmployeeId equals emp.EmployeeId
                join offices in db.EmployeeOfficeDetail
                    on leave.EmployeeId equals offices.EmployeeId into officeJoin
                from offices in officeJoin.DefaultIfEmpty()
                select new
                {
                    Leave = leave,
                    Employee = emp,
                    Office = offices
                };

            //=========================
            // Filters
            //=========================

            if (User.IsInRole("Admin") && currentOfficeId.HasValue)
            {
                query = query.Where(x => x.Office.OfficeId == currentOfficeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(nFromDate))
            {
                DateTime fromDate = NepaliDateConverter.ConvertToEnglish(
                    NepaliDateConverter.Format(nFromDate));

                query = query.Where(x => x.Leave.FromDate >= fromDate);
            }

            if (!string.IsNullOrWhiteSpace(nToDate))
            {
                DateTime toDate = NepaliDateConverter.ConvertToEnglish(
                    NepaliDateConverter.Format(nToDate));

                query = query.Where(x => x.Leave.ToDate <= toDate);
            }

            if (int.TryParse(empId, out int employeeId) && employeeId > 0)
            {
                query = query.Where(x => x.Leave.EmployeeId == employeeId);
            }

            if (int.TryParse(officeId, out int office) && office > 0)
            {
                query = query.Where(x => x.Office.OfficeId == office);
            }

            if (int.TryParse(branchId, out int branch) && branch > 0)
            {
                query = query.Where(x => x.Office.BranchId == branch);
            }

            if (int.TryParse(serviceId, out int service) && service > 0)
            {
                query = query.Where(x => x.Office.ServiceId == service);
            }

            if (int.TryParse(levelId, out int level) && level > 0)
            {
                query = query.Where(x => x.Office.LevelId == level);
            }

            if (int.TryParse(designationId, out int designation) && designation > 0)
            {
                query = query.Where(x => x.Office.DesignationId == designation);
            }

            //=========================
            // Sorting
            //=========================

            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(x => x.Employee.EmployeeName);
                    break;

                case "Date":
                    query = query.OrderBy(x => x.Leave.ApplicationDate);
                    break;

                case "date_desc":
                    query = query.OrderByDescending(x => x.Leave.ApplicationDate);
                    break;

                default:
                    query = query.OrderByDescending(x => x.Leave.LeaveApplicationId);
                    break;
            }

            // Execute only once
            var list = query.ToList();

            // Assign objects required by View
            foreach (var item in list)
            {
                item.Leave.EmployeeInfo = item.Employee;
                item.Leave.EmployeeOfficeDetail = item.Office;
            }

            var result = list.Select(x => x.Leave).ToPagedList(pageNumber, pageLength);

            return View(result);
        }

        public ActionResult AddLeavePosting()
        {
            LeaveApplication model = new LeaveApplication();
            return base.View(model);
        }

        [HttpPost]
        public ActionResult AddLeavePosting(LeaveApplication model)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                LeaveApplication provider = new LeaveApplication();
                model.ApplicationDate = new DateTime?(DateTime.Now);
                DateTime frmDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate));
                DateTime toDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate));

                if (CheckValidLeaveOrVisitDateInterval(frmDate, toDate, model.EmployeeId, 0))
                {
                    TempData.Add("Message", "छान्नु भएको कर्मचारी बिदा वा काज मा पहिले देखि नै छ।");
                    return base.RedirectToAction("Index", "LeaveApplication");
                }

                model.FromDate = new DateTime?(frmDate);
                model.ToDate = new DateTime?(toDate);
                if (toDate >= frmDate)
                {
                    TimeSpan span = (TimeSpan)(toDate - frmDate);
                    model.TotalDays = span.Days + 1;
                }
                if (model.TotalDays == 1M)
                {
                    if (model.IsHalfDay)
                    {
                        model.TotalDays = Convert.ToDecimal((double)0.5);
                    }
                }
                else if (model.TotalDays > 1M)
                {
                    model.IsHalfDay = false;
                }
                model.CreatedBy = userIdByUserName;
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.ApprovedStatus = 2;
                model.RecomenderStatus = 2;
                model.ApprovedDate = new DateTime?(NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NApprovedDate)));
                model.Status = 1;
                model.Type = 2;

                db.LeaveApplication.Add(model);
                db.SaveChanges();
                TempData["Message"] = "Leave application created successfully.";
            }
            catch
            {
            }
            return base.RedirectToAction("LeavePosting", "LeaveApplication");
        }

        public ActionResult EditLeavePosting(int? id)
        {
            Func<LeaveApplication, bool> predicate = null;
            LeaveApplication model = new LeaveApplication();
            if (id.HasValue)
            {
                if (predicate == null)
                {
                    predicate = x => x.LeaveApplicationId == id;
                }
                model = db.LeaveApplication.Where(x => x.LeaveApplicationId == id).FirstOrDefault();
                model.NApplicationDate = (model.ApplicationDate.HasValue && (model.ApplicationDate.Value.Year > 0x7d0)) ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ApplicationDate.Value.Date), "yyyy-MM-DD") : " ";
                model.NFromDate = (model.FromDate.HasValue && (model.FromDate.Value.Year > 0x7d0)) ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.FromDate.Value.Date), "yyyy-MM-DD") : "";
                model.NToDate = (model.ToDate.HasValue && (model.ToDate.Value.Year > 0x7d0)) ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ToDate.Value.Date), "yyyy-MM-DD") : "";
                model.NApprovedDate = model.ApprovedDate.HasValue ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ApprovedDate.Value.Date), "yyyy-MM-DD") : " ";
                model.Type = 2;
            }
            return base.View(model);
        }

        [HttpPost]
        public ActionResult EditLeavePosting(LeaveApplication model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = db.LeaveApplication.Where(x => x.LeaveApplicationId == model.LeaveApplicationId)
                        .FirstOrDefault();
                    string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                    data.ModifiedBy = userIdByUserName;
                    data.ModifiedDate = DateTime.Now;
                    DateTime frmDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate));
                    DateTime toDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate));

                    if (CheckValidLeaveOrVisitDateInterval(frmDate, toDate, model.EmployeeId, 0))
                    {
                        TempData.Add("Message", "छान्नु भएको कर्मचारी बिदा वा काज मा पहिले देखि नै छ।");
                        return base.RedirectToAction("Index", "LeaveApplication");
                    }

                    data.ApprovedDate = new DateTime?(((model.NApplicationDate != null) && (model.NApplicationDate.Length > 8)) ? NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NApplicationDate)) : DateTime.Now);
                    data.FromDate = new DateTime?(frmDate);
                    data.ToDate = new DateTime?(toDate);
                    if (toDate >= frmDate)
                    {
                        TimeSpan span = (TimeSpan)(toDate - frmDate);
                        data.TotalDays = span.Days + 1;
                    }
                    if (model.TotalDays == 1M)
                    {
                        if (model.IsHalfDay)
                        {
                            data.TotalDays = Convert.ToDecimal((double)0.5);
                        }
                    }
                    else if (model.TotalDays > 1M)
                    {
                        data.IsHalfDay = false;
                    }
                    data.Type = 2;
                    data.Status = 1;
                    data.ApprovedStatus = 2;
                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Leave application updated successfully.";
                }
                catch
                {
                }
                return base.RedirectToAction("LeavePosting", "LeaveApplication");
            }
            return base.View();
        }

        public ActionResult DeleteLeavePosting(int? id)
        {
            LeaveApplication model = new LeaveApplication();
            model = db.LeaveApplication.Where(x => x.LeaveApplicationId == id).FirstOrDefault();
            return base.View(model);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveApplication leaveapplication = await db.LeaveApplication.FindAsync(id);
            if (leaveapplication == null)
            {
                return HttpNotFound();
            }
            return View(leaveapplication);
        }

        // POST: /LeaveApplication/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(LeaveApplication model)
        {
            LeaveApplication leaveapplication = await db.LeaveApplication.FindAsync(model.LeaveApplicationId);
            db.LeaveApplication.Remove(leaveapplication);
            await db.SaveChangesAsync();
            TempData["Message"] = "Leave application deleted successfully.";
            return RedirectToAction("LeavePosting");
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
