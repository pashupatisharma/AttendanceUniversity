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
using PagedList;

namespace eAttendance.Controllers
{
    [Authorize]
    public class VisitApplicationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /VisitApplication/
        public ActionResult Index()
        {



            VisitApplicationModel Model = new VisitApplicationModel();
            Model.VisitApplicationList = new List<VisitApplicationModel>();

            int? _officeId = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);
            var userId = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
            var empid = db.EmployeeInfo.Where(x => x.UserId == userId).FirstOrDefault().EmployeeId;
            string s = "VisitRequestList" + " " + +empid + "," + _officeId;
            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 180;

            var list = db.Database.SqlQuery<VisitApplicationModel>(s).ToList();
            Model.VisitApplicationList = list;
            return View(Model);

        }

        // GET: /VisitApplication/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitApplication visitapplication = await db.VisitApplication.FindAsync(id);
            if (visitapplication == null)
            {
                return HttpNotFound();
            }
            return View(visitapplication);
        }

        public ActionResult Add()
        {
            VisitApplication model = new VisitApplication
            {
                ApplicationDate = DateTime.Now,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            };
            return base.View(model);
        }

        [HttpPost]
        public ActionResult VisitPosting(VisitApplication model)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                model.EmployeeId = EmployeeProvider.GetEmployeeIdByUserName(User.Identity.Name);
                model.ApplicationDate = DateTime.Now;
                DateTime frmDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate));
                DateTime toDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate));
                if (this.CheckValidLeaveOrVisitDateInterval(frmDate, toDate, model.EmployeeId, 0))
                {

                    base.TempData.Add("Message", "छान्नु भएको कर्मचारी बिदा वा काज मा पहिले देखि नै छ।");
                    return base.RedirectToAction("Index", "VisitApplication");
                }
                model.FromDate = frmDate;
                model.ToDate = toDate;
                if (toDate >= frmDate)
                {
                    TimeSpan span = (TimeSpan)(toDate - frmDate);
                    model.TotalDays = span.Days + 1;
                }
                model.CreatedBy = userIdByUserName;
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.ApprovedStatus = 1;
                model.RecomenderStatus = 1;
                model.Status = 1;

                db.VisitApplication.Add(model);
                db.SaveChanges();
                base.TempData.Add("Message", "Save Sucess");
            }
            catch
            {

                base.TempData.Add("Message", "Failed");
            }
            return base.RedirectToAction("Index", "VisitApplication");
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

        // GET: /VisitApplication/Edit/5
        public ActionResult Edit(int? id)
        {

            VisitApplication model = new VisitApplication();
            if (id.HasValue)
            {

                //model = this.provider.GetAll().Where<VisitApplicationModel>(predicate).FirstOrDefault<VisitApplicationModel>();
                model = db.VisitApplication.Where(x => x.VisitApplicationId == id).FirstOrDefault();
                DateTime fromDate = model.FromDate;
                model.NApplicationDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ApplicationDate.Date), "yyyy-MM-DD");
                DateTime time2 = model.FromDate;
                model.NFromDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.FromDate.Date), "yyyy-MM-DD");
                DateTime toDate = model.ToDate;
                model.NToDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ToDate.Date), "yyyy-MM-DD");
            }
            return base.View(model);


        }

        // POST: /VisitApplication/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VisitApplication visitapplication)
        {
        
         

            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                visitapplication.ModifiedBy = userIdByUserName;
                visitapplication.ModifiedDate = DateTime.Now;
                DateTime frmDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(visitapplication.NFromDate));
                DateTime toDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(visitapplication.NToDate));

            
                if (visitapplication.NApprovedDate != null)
                    visitapplication.ApprovedDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(visitapplication.NApprovedDate));
                visitapplication.FromDate = frmDate;
                visitapplication.ToDate = toDate;
                if (visitapplication.NApplicationDate != null)
                {
                    visitapplication.ApplicationDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(visitapplication.NApplicationDate));
                }
                else
                {
                    visitapplication.ApplicationDate = DateTime.Now;
                }
                if (toDate >= frmDate)
                {
                    TimeSpan span = (TimeSpan)(toDate - frmDate);
                    visitapplication.TotalDays = span.Days + 1;
                }
                visitapplication.Type = 2;
                visitapplication.Status = 1;

                db.Entry(visitapplication).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {

            }
            return base.RedirectToAction("Index", "VisitApplication");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult VisitPosting(string nFromDate, string nToDate, string officeId, string branchId, string serviceId, string levelId, string designationId, string empId, string sortOrder, int? page)
        {

            int? _officeId = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = (sortOrder == "Date") ? "date_desc" : "Date";
           
            ViewBag.CurrentFilter = empId;
            //  IEnumerable<VisitApplication> source = db.VisitApplication.Where(x => x.ApprovedStatus != 2).ToList();
    var newsource = new List<VisitApplication>();


            IEnumerable<VisitApplication> source = db.VisitApplication.ToList();

            foreach (var item in source)
            {
                VisitApplication obj = new VisitApplication();
                obj = item;
                obj.EmployeeInfo = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                obj.EmployeeOfficeDetail = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                newsource.Add(obj);
            }
            int? officeid = 0;
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

            if (!string.IsNullOrEmpty(officeId) && officeId!="0"  )
            {
                var neweofficeId = int.Parse(officeId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.OfficeId == neweofficeId).ToList();
            }
            if (!string.IsNullOrEmpty(branchId))
            {
                int newbranchId = int.Parse(branchId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.BranchId == newbranchId).ToList();
            }
            if (!string.IsNullOrEmpty(serviceId)&& serviceId!="0" )
            {
                var newserviceId = int.Parse(serviceId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.ServiceId == newserviceId).ToList();
            }
            if (!string.IsNullOrEmpty(levelId)&& levelId!="0")
            {
                var newlevel = int.Parse(levelId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.LevelId == newlevel).ToList();
            }
            if (!string.IsNullOrEmpty(designationId)&& designationId!="0")
            {
                var newDesignationId = int.Parse(designationId.Trim());
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.DesignationId == newDesignationId).ToList();
            }


            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        newsource = from s in newsource
            //                 orderby s.EmployeeInfo.EmployeeName descending
            //                 select s;
            //        break;

            //    case "Date":
            //        newsource = from s in newsource
            //                 orderby s.ApplicationDate descending
            //                 select s;
            //        break;

            //    case "date_desc":
            //        newsource = from s in newsource
            //                 orderby s.CreatedDate descending
            //                 select s;
            //        break;

            //    default:
            //        source = from s in newsource
            //                 orderby s.EmployeeId
            //                 select s;
            //        break;
            //}
            int pageSize = 10;
            int? nullable = page;
            int pageNumber = nullable.HasValue ? nullable.GetValueOrDefault() : 1;
            return base.View(newsource.ToPagedList<VisitApplication>(pageNumber, pageSize));
        }

        public ActionResult AddVisitPosting()
        {
            VisitApplication model = new VisitApplication();
            return base.View(model);
        }


        [HttpPost]
        public ActionResult AddVisitPosting(VisitApplication model)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                VisitApplication provider = new VisitApplication();
                model.ApplicationDate = DateTime.Now;
                DateTime frmDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate));
                DateTime toDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate));
                DateTime time3 = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NApprovedDate));


                if (CheckValidLeaveOrVisitDateInterval(frmDate, toDate, model.EmployeeId, 0))
                {

                    TempData.Add("Message", "छान्नु भएको कर्मचारी बिदा वा काज मा पहिले देखि नै छ।");
                    return base.RedirectToAction("VisitPosting", "VisitApplication");
                }

                model.FromDate = frmDate;
                model.ToDate = toDate;
                model.ApprovedDate = new DateTime?(time3);
                if (toDate >= frmDate)
                {
                    TimeSpan span = (TimeSpan)(toDate - frmDate);
                    model.TotalDays = span.Days + 1;
                }
                model.CreatedBy = userIdByUserName;
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.RecomenderStatus = 1;
                model.RecomendedDate = model.ApprovedDate;
                model.ApprovedStatus = 2;
                model.ApprovedDate = model.ApprovedDate;
                model.Type = 2;
                model.Status = 1;
                // provider.Insert(model);
                db.VisitApplication.Add(model);
                db.SaveChanges();

            }
            catch
            {

            }
            return base.RedirectToAction("VisitPosting", "VisitApplication");
        }

        public ActionResult EditVisitPosting(int? id)
        {
            Func<VisitApplication, bool> predicate = null;
            VisitApplication model = new VisitApplication();
            if (id.HasValue)
            {
                if (predicate == null)
                {
                    predicate = x => x.VisitApplicationId == id;
                }
                model = db.VisitApplication.Where(x => x.VisitApplicationId == id).FirstOrDefault();
                DateTime fromDate = model.FromDate;
                model.NFromDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.FromDate.Date), "yyyy-MM-DD");
                DateTime toDate = model.ToDate;
                model.NToDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ToDate.Date), "yyyy-MM-DD");
                model.NApprovedDate = model.ApprovedDate.HasValue ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ApprovedDate.Value.Date), "yyyy-MM-DD") : " ";
                DateTime applicationDate = model.ApplicationDate;
                model.NApplicationDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ApplicationDate.Date), "yyyy-MM-DD");
                model.Type = 2;
            }
            return base.View(model);
        }

        [HttpPost]
        public ActionResult EditVisitPosting(VisitApplication model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                    model.ModifiedBy = userIdByUserName;
                    model.ModifiedDate = DateTime.Now;
                    DateTime frmDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate));
                    DateTime toDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate));
                    DateTime time3 = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NApprovedDate));


                    if (CheckValidLeaveOrVisitDateInterval(frmDate, toDate, model.EmployeeId, 0))
                    {

                        TempData.Add("Message", "छान्नु भएको कर्मचारी बिदा वा काज मा पहिले देखि नै छ।");
                        return base.RedirectToAction("VisitPosting", "VisitApplication");
                    }


                    model.ApprovedDate = new DateTime?(time3);
                    model.FromDate = frmDate;
                    model.ToDate = toDate;
                    if (toDate >= frmDate)
                    {
                        TimeSpan span = (TimeSpan)(toDate - frmDate);
                        model.TotalDays = span.Days + 1;
                    }
                    model.Type = 2;
                    model.Status = 1;
                    model.ApprovedStatus = 2;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch
                {

                }
                return base.RedirectToAction("VisitPosting", "VisitApplication");
            }
            return base.View();
        }


        public ActionResult DeleteVisitPosting(int? id)
        {
            VisitApplication model = new VisitApplication();
            model = db.VisitApplication.Where(x => x.VisitApplicationId == id).FirstOrDefault();
            return base.View(model);
        }
        [HttpPost]
        public ActionResult DeleteVisitPosting(VisitApplication model)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                model.ModifiedBy = userIdByUserName;
                model.ModifiedDate = DateTime.Now;
                model.Status = 2;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {

            }
            return base.RedirectToAction("VisitPosting", "VisitApplication");
        }






        // GET: /VisitApplication/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitApplication visitapplication = await db.VisitApplication.FindAsync(id);
            if (visitapplication == null)
            {
                return HttpNotFound();
            }
            return View(visitapplication);
        }

        // POST: /VisitApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VisitApplication visitapplication = await db.VisitApplication.FindAsync(id);
            db.VisitApplication.Remove(visitapplication);
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
