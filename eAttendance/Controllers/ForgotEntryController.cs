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
using System.Globalization;

namespace eAttendance.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
    public class ForgotEntryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ForgotEntry/
        public ActionResult Index()
        {
            ForgotEntry model = new ForgotEntry();
            var newsource = new List<ForgotEntry>();
            var source = db.ForgotEntry.Include(f => f.EmployeeInfo).Where(x => x.Status != 2).ToList();
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

            foreach (var item in source)
            {
                ForgotEntry obj = new ForgotEntry();
                obj = item;
                obj.EmployeeInfo = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                obj.EmployeeOfficeDetail = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                newsource.Add(obj);
            }

            if (User.IsInRole("Admin"))
            {
                int? a = officeid;
                newsource = newsource.Where(x => x.EmployeeOfficeDetail.OfficeId == a).ToList();
            }



            model.ForgotEntryList = newsource;

            return View(model);
        }

        // GET: /ForgotEntry/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForgotEntry forgotentry = await db.ForgotEntry.FindAsync(id);
            if (forgotentry == null)
            {
                return HttpNotFound();
            }
            return View(forgotentry);
        }

        // GET: /ForgotEntry/Create
        public ActionResult Add()
        {
            string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
            ForgotEntry model = new ForgotEntry
            {
                EmployeeId = EmployeeProvider.GetEmployeeIdByUserName(User.Identity.Name),
                ForgotDate = DateTime.Now
            };
            List<StatusListModel> list = new List<StatusListModel>();
            if (User.IsInRole("Admin"))
            {
                StatusListModel model2 = new StatusListModel
                {
                    Id = 4,
                    Name = "Approved"
                };
                list.Add(model2);
                StatusListModel model3 = new StatusListModel
                {
                    Id = 5,
                    Name = "Reject"
                };
                list.Add(model3);
            }
            StatusListModel item = new StatusListModel
            {
                Id = 3,
                Name = "Applyed"
            };
            list.Add(item);
            model.StatusModelList = list;
            return base.View(model);
        }
        // POST: /ForgotEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Add(ForgotEntry model)
        {
            try
            {

                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                ApplicationDbContext entities = new ApplicationDbContext();

                model.ForgotDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NForgotDate));
                model.ForgotTime = TimeSpan.ParseExact(model.NForgotTime, @"h\:m", CultureInfo.InvariantCulture);

                model.CreatedBy = userIdByUserName;
                model.ShiftTimeId = Convert.ToInt32(Utility.GetShiftTimeByEmployeeId(model.EmployeeId, DateTime.Now));
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.ModifiedBy = userIdByUserName;
                model.Status = 4;
                entities.ForgotEntry.Add(model);

                AttendanceLog data = new AttendanceLog();
                data.DateTime = new DateTime?(model.ForgotDate + model.ForgotTime);
                data.OfficeId = 1;
                data.OfficeDeviceId = 0;
                data.EmployeeId = model.EmployeeId;
                data.ShiftTimeId = model.ShiftTimeId;
                data.IpAddress = "";
                data.EnrollNumber = db.EmployeeInfo.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault().EmployeeNo;
                data.VerifyMode = "1"; ;
                data.InOutMode = model.CheckType.ToString();
                data.Status = 1;
                data.Remarks=model.Remarks;
             
                entities.AttendanceLog.Add(data);
                entities.SaveChanges();

            }
            catch
            {

            }
            return base.RedirectToAction("Index", "ForgotEntry");
        }
        [HttpPost]
        public ActionResult Edit(ForgotEntry model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationDbContext entities = new ApplicationDbContext();
                    string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                    model.ModifiedBy = userIdByUserName;
                    model.ForgotDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NForgotDate));
                    model.ForgotTime = TimeSpan.ParseExact(model.NForgotTime, @"h\:m", CultureInfo.InvariantCulture);
                    model.ModifiedDate = (DateTime.Now);

                    if (model.Status == 4)
                    {

                        AttendanceLog data = new AttendanceLog();
                        data.DateTime = new DateTime?(model.ForgotDate + model.ForgotTime);
                        data.OfficeId = 1;
                        data.OfficeDeviceId = 0;
                        data.EmployeeId = model.EmployeeId;
                        data.ShiftTimeId = model.ShiftTimeId;
                        data.IpAddress = "";
                        data.EnrollNumber = db.EmployeeInfo.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault().EmployeeNo;
                        data.VerifyMode = "1";
                        data.InOutMode = model.CheckType.ToString();
                        data.Status = 1;
                        data.Remarks=model.Remarks;
                        entities.AttendanceLog.Add(data);
                        entities.SaveChanges();
                    }

                }
                catch
                {

                }
                return base.RedirectToAction("Index", "ForgotEntry");
            }
            return base.View();
        }

        public ActionResult Edit(int? id)
        {
            Func<ForgotEntry, bool> predicate = null;
            ForgotEntry model = new ForgotEntry();
            if (id.HasValue)
            {
                if (predicate == null)
                {
                    predicate = x => x.ForegotEntryId == id;
                }
                model = db.ForgotEntry.Where(x => x.ForegotEntryId == id).FirstOrDefault();
                DateTime forgotDate = model.ForgotDate;
                model.NForgotDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ForgotDate), "yyyy-MM-DD");
                TimeSpan forgotTime = model.ForgotTime;
                model.NForgotTime = model.ForgotTime.ToString(@"hh\:mm");
                List<StatusListModel> list = new List<StatusListModel>();
                if ((User.IsInRole("Admin") || User.IsInRole("SuperAdmin")) || User.IsInRole("Administrator"))
                {
                    StatusListModel item = new StatusListModel
                    {
                        Id = 4,
                        Name = "Approved"
                    };
                    list.Add(item);
                    StatusListModel model3 = new StatusListModel
                    {
                        Id = 5,
                        Name = "Reject"
                    };
                    list.Add(model3);
                }
                if (User.IsInRole("Employee"))
                {
                    StatusListModel item = new StatusListModel
                    {
                        Id = 3,
                        Name = "Applyed"
                    };
                    list.Add(item);
                }
                model.StatusModelList = list;
            }
            return base.View(model);
        }


        // GET: /ForgotEntry/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForgotEntry forgotentry = await db.ForgotEntry.FindAsync(id);
            if (forgotentry == null)
            {
                return HttpNotFound();
            }
            return View(forgotentry);
        }


        [HttpPost]
        public ActionResult Delete(ForgotEntry model)
        {
            try
            {

                ApplicationDbContext entities = new ApplicationDbContext();
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                model.ModifiedBy = userIdByUserName;
                model.ModifiedDate = DateTime.Now;
                model.Status = 2;

                DateTime forgotDateTime = model.ForgotDate + model.ForgotTime;
                List<AttendanceLog> list = entities.AttendanceLog.Where(x => x.EmployeeId == model.EmployeeId).Where(x => x.DateTime == forgotDateTime).ToList();
                foreach (AttendanceLog item in list)
                {
                    AttendanceLog data = entities.AttendanceLog.Where(x => x.DeviceDataId == item.DeviceDataId).FirstOrDefault();
                    data.Status = 2;
                    data.DeletedBy = userIdByUserName;
                    data.DeletedDate = DateTime.Now;

                    ForgotEntry frgt = entities.ForgotEntry.Where(x => x.ForegotEntryId == item.DeviceDataId).FirstOrDefault();
                    frgt.Status = 2;
                    frgt.DeletedDate = DateTime.Now;
                    entities.SaveChanges();
                }

            }
            catch
            {

            }
            return base.RedirectToAction("Index", "ForgotEntry");
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
