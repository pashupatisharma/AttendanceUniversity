using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eAttendance.Models;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Web.WebPages;
using eAttendance.ReportModel;
using PagedList;

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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

                }
                catch
                {

                }
                return base.RedirectToAction("Index", "LeaveApplication");
            }
            return base.View();

        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult LeavePosting(string nFromDate, string nToDate, string officeId, string branchId, string serviceId, string levelId, string designationId, string empId, string sortOrder, int? pageSize, int? page)
        {
            int? _officeId = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = (sortOrder == "Date") ? "date_desc" : "Date";
            if (empId != null)
            {
                page = 1;
            }
            else
            {
                empId = designationId;
            }
            ViewBag.CurrentFilter = empId;
            //  IEnumerable<LeaveApplication> source = db.LeaveApplication.Where(x => x.ApprovedStatus != 2).ToList();
            var newsource = new List<LeaveApplication>();

            //var newsource = new List<LeaveApplication>();


            IEnumerable<LeaveApplication> source = db.LeaveApplication.ToList();
           foreach (var item in source)
            {
                LeaveApplication obj = new LeaveApplication();
                obj = item;
                obj.EmployeeInfo = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                obj.EmployeeOfficeDetail = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                newsource.Add(obj);
            }





            int ? officeid = 0;
            int EmployeeId = 0;
            try
            {
                var userId = User.Identity.Name;
                var userid = db.Users.Where(x => x.UserName == userId).FirstOrDefault().Id;

                var employee = db.EmployeeInfo.Where(x => x.UserId == userid).FirstOrDefault();
                if (employee != null)
                {
                    EmployeeId = employee.EmployeeId;
                    officeid = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == employee.EmployeeId).FirstOrDefault().OfficeId;
                }
            }
            catch (Exception ex)
            {
            }

            if (User.IsInRole("Admin"))
            {
                int? a = officeid;
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.OfficeId == a).ToList();
            }

            if (!string.IsNullOrEmpty(nFromDate))
            {
                DateTime frmDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(nFromDate));
                newsource = newsource.Where(x => x.FromDate >= frmDate).ToList();
            }


            if (!string.IsNullOrEmpty(nToDate))
            {
                DateTime Todate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(nToDate));
                newsource = newsource.Where(x => x.ToDate <= Todate).ToList();

            }
            if (!string.IsNullOrWhiteSpace(empId)&&empId!="0")
            {
                int newempid = int.Parse(empId.Trim());
                newsource = newsource.Where(x => x.EmployeeId == newempid).ToList();
            }

            if (!string.IsNullOrEmpty(officeId) && officeId != "0")
            {
                var neweofficeId = int.Parse(officeId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.OfficeId == neweofficeId).ToList();
            }
            if (!string.IsNullOrEmpty(branchId))
            {
                int newbranchId = int.Parse(branchId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.BranchId == newbranchId).ToList();
            }
            if (!string.IsNullOrEmpty(serviceId) && serviceId != "0")
            {
                var newserviceId = int.Parse(serviceId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.ServiceId == newserviceId).ToList();
            }
            if (!string.IsNullOrEmpty(levelId) && levelId != "0")
            {
                var newlevel = int.Parse(levelId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.LevelId == newlevel).ToList();
            }
            if (!string.IsNullOrEmpty(designationId) && designationId != "0")
            {
                var newDesignationId = int.Parse(designationId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.DesignationId == newDesignationId).ToList();
            }

            //if (!string.IsNullOrEmpty(nFromDate))
            //{

            //}
            //else if (!(string.IsNullOrWhiteSpace(empId) || (int.Parse(empId.Trim()) != 0)))
            //{
            //}
            //if (!string.IsNullOrEmpty(nToDate))
            //{
            //}
            //else if (!(string.IsNullOrWhiteSpace(empId) || (int.Parse(empId.Trim()) != 0)))
            //{
            //}
            //if ((!string.IsNullOrEmpty(officeId) && (int.Parse(officeId.Trim()) > 0)) && (_officeId > 0))
            //{
            //}
            //if (!string.IsNullOrEmpty(branchId) && (int.Parse(branchId.Trim()) > 0))
            //{

            //}
            //if (!string.IsNullOrEmpty(serviceId) && (int.Parse(serviceId.Trim()) > 0))
            //{

            //}
            //if (!string.IsNullOrEmpty(levelId) && (int.Parse(levelId.Trim()) > 0))
            //{

            //}
            //if (!string.IsNullOrEmpty(designationId) && (int.Parse(designationId.Trim()) > 0))
            //{

            //}
            //if (!string.IsNullOrEmpty(empId) && (int.Parse(empId.Trim()) > 0))
            //{

            //}


            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        newsource = from s in newsource
            //                    orderby s.EmployeeInfo.EmployeeName descending
            //                    select s;
            //        break;

            //    case "Date":
            //        newsource = from s in newsource
            //                    orderby s.ApplicationDate descending
            //                 select s;
            //        break;

            //    case "date_desc":
            //        newsource = from s in newsource
            //                    orderby s.CreatedDate descending
            //                 select s;
            //        break;

            //    default:
            //        newsource = from s in newsource
            //                 orderby s.EmployeeId
            //                 select s;
            //        break;
            //}


            int num = newsource.Count<LeaveApplication>();

            num = newsource.Count<LeaveApplication>();
            int num2 = 10;
            if (pageSize.HasValue)
            {
                num2 = pageSize.Value;
            }
            int? nullable = page;
            int pageNumber = nullable.HasValue ? nullable.GetValueOrDefault() : 1;
            return base.View(newsource.ToPagedList<LeaveApplication>(pageNumber, num2));

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
