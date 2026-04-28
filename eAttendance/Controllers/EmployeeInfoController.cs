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
using eAttendance.ViewModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Data.OleDb;

namespace eAttendance.Controllers
{

    public class EmployeeInfoController : Controller
    {
        public EmployeeInfoController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public EmployeeInfoController(UserManager<ApplicationUser> userManager)
        {

            UserManager = userManager;
            UserManager.UserValidator = new Microsoft.AspNet.Identity.UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /EmployeeInfo/


        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult Index(string officeId, string branchId, string levelId, string designationId, string serviceId, string employeeid, string statusid, string sortOrder, int? pageSize, int? page)
        {
            ViewBag.officeId = officeId;
            ViewBag.BranchId = branchId;

            ViewBag.LevelId = levelId;


            ViewBag.DesignationId = designationId;


            ViewBag.ServiceId = serviceId;




            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = (sortOrder == "Date") ? "date_desc" : "Date";
            if (employeeid != null)
            {
                page = 1;
            }
            else
            {
                employeeid = designationId;
            }
            ViewBag.CurrentFilter = employeeid;




            var source = (from x in db.EmployeeInfo

                          join y in db.EmployeeOfficeDetail on x.EmployeeId equals y.EmployeeId

                          where x.Status != 2

                          select new EmployeeInfoReport
                          {

                              EmployeeId = x.EmployeeId,
                              LevelId = y.LevelId,
                              OfficeId = y.OfficeId,
                              BranchId = y.BranchId,
                              DesignationId = y.DesignationId,
                              EmployeeNo = x.EmployeeNo,
                              EmployeeName = x.EmployeeName,
                              EmailId = x.EmailId,
                              Gender = x.Gender,
                              Status = x.Status,
                              serviceId = y.ServiceId




                          });

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
                source = source.Where(x => x.OfficeId == officeid);
            }

            if (User.IsInRole("Employee"))
            {
                source = source.Where(x => x.EmployeeId == EmployeeId);
            }


            if (!string.IsNullOrEmpty(employeeid) && employeeid != "0")
            {
                int id = int.Parse(employeeid);
                source = source.Where(x => x.EmployeeId == id);
            }

            if (!string.IsNullOrEmpty(officeId) && officeId != "0")
            {
                int id = int.Parse(officeId);
                source = source.Where(x => x.OfficeId == id);
            }
            if (!string.IsNullOrEmpty(branchId) && branchId != "0")
            {
                int id = int.Parse(branchId);
                source = source.Where(x => x.BranchId == id);
            }

            if (!string.IsNullOrEmpty(designationId) && designationId != "0")
            {
                int id = int.Parse(designationId);
                source = source.Where(x => x.DesignationId == id);
            }


            if (!string.IsNullOrEmpty(levelId) && levelId != "0")
            {
                int id = int.Parse(levelId);
                source = source.Where(x => x.LevelId == id);
            }


            if (!string.IsNullOrEmpty(serviceId) && serviceId != "0")
            {
                int id = int.Parse(serviceId);
                source = source.Where(x => x.serviceId == id);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    source = from s in source
                             orderby s.EmployeeName descending
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
                             orderby s.EmployeeName
                             select s;
                    break;
            }


            int num = source.Count<EmployeeInfoReport>();

            num = source.Count<EmployeeInfoReport>();
            int num2 = 10;
            if (pageSize.HasValue)
            {
                num2 = pageSize.Value;
            }
            int? nullable = page;
            int pageNumber = nullable.HasValue ? nullable.GetValueOrDefault() : 1;

            return base.View(source.ToPagedList<EmployeeInfoReport>(pageNumber, num2));

        }

        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult Add()
        {
            EmployeeViewModel model = new EmployeeViewModel();
            List<LeaveTypeSetUp> list = db.LeaveTypeSetUp.Where(x => x.Status == 1).ToList();

            List<AssignEmployeeLeave> list2 = new List<AssignEmployeeLeave>();
            foreach (var item in list)
            {

                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);

                FiscalYearSetUp year = db.FiscalYearSetUp.Where(x => x.IsActive == true).FirstOrDefault();

                AssignEmployeeLeave obj = new AssignEmployeeLeave();

                obj.FiscalYearId = year.FiscalYearId;
                obj.LeaveTypeId = item.LeaveTypeId;
                obj.LeaveTypeName = item.LeaveTypeName;
                obj.OpenningBalance = 0M;

                obj.NoOfLeave = 0M;

                list2.Add(obj);
            }
            model.AssignEmployeeLeaveLlist = list2;
            return base.View(model);
        }

        public ActionResult UploadExcelFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadExcelFile(HttpPostedFileBase file)
        {
            var Fiscalyear = db.FiscalYearSetUp.Where(x => x.IsActive == true).FirstOrDefault();
            if (file != null)
            {

                var fileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(fileName).ToLower();

                string connString = "";

                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                string path1 = Path.Combine(Server.MapPath("~/UploadedFiles/"), fileName);


                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(Server.MapPath("~/UploadedFiles/"));
                }
                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    { System.IO.File.Delete(path1); }
                    file.SaveAs(path1);


                    if (extension.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                    }
                    else if (extension.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                    }


                    using (OleDbConnection connection = new OleDbConnection(connString))
                    {
                        connection.Open();
                        OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                        using (OleDbDataReader dr = command.ExecuteReader())
                        {

                            while (dr.Read())
                            {
                                try
                                {
                                    var Email = dr[13].ToString() + "gmail.com";
                                    var password = "password" + dr[13].ToString();
                                    string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                                    var user = new ApplicationUser() { UserName = Email };
                                    var result = await UserManager.CreateAsync(user, password);

                                    var userId = string.Empty;
                                    if (result.Succeeded)
                                    {
                                        userId = db.Users.Where(x => x.UserName == Email).FirstOrDefault().Id;

                                        if (!string.IsNullOrEmpty(userId))
                                        {
                                            EmployeeInfo empinfo = new EmployeeInfo();

                                            empinfo.EmployeeNo = dr[13].ToString();
                                            empinfo.EmailId = Email;
                                            empinfo.EmployeeNameNp = dr[1].ToString();
                                            empinfo.EmployeeName = dr[2].ToString();
                                            empinfo.UserId = userId;
                                            empinfo.GenerateReport = 1;
                                            empinfo.Status = 1;
                                            if (dr[18].ToString() == "Male") { empinfo.Gender = 1; } else { empinfo.Gender = 2; }

                                            empinfo.CreatedBy = userIdByUserName;
                                            empinfo.CreatedDate = DateTime.Now;
                                            empinfo.ModifiedBy = userIdByUserName;
                                            empinfo.ModifiedDate = DateTime.Now;

                                            db.EmployeeInfo.Add(empinfo);
                                            db.SaveChanges();

                                            int maxempid = db.EmployeeInfo.Max(z => z.EmployeeId);

                                            EmployeeOfficeDetail modelEmployeeOfficeInfo = new EmployeeOfficeDetail();
                                            modelEmployeeOfficeInfo.EmployeeId = maxempid;
                                            modelEmployeeOfficeInfo.OfficeId = Convert.ToInt32(dr[4].ToString());
                                            modelEmployeeOfficeInfo.DepartmentId = Convert.ToInt32(dr[6].ToString());
                                            modelEmployeeOfficeInfo.LevelId = Convert.ToInt32(dr[11].ToString());
                                            modelEmployeeOfficeInfo.DesignationId = Convert.ToInt32(dr[7].ToString()); ;
                                            modelEmployeeOfficeInfo.ServiceId = null;
                                            modelEmployeeOfficeInfo.CreatedDate = DateTime.Now;
                                            DateTime time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format("2037-12-02"));
                                            modelEmployeeOfficeInfo.EffectiveDate = ((time.Year >= 0x779) && (time.Year <= 0x7fa)) ? time : new DateTime(0x76c, 1, 1);


                                            modelEmployeeOfficeInfo.Status = 1;
                                            modelEmployeeOfficeInfo.CreatedDate = DateTime.Now;
                                            modelEmployeeOfficeInfo.ModifiedDate = DateTime.Now;
                                            db.EmployeeOfficeDetail.Add(modelEmployeeOfficeInfo);

                                            db.SaveChanges();
                                        }
                                    }
                                }
                                catch
                                {

                                }


                            }

                        }
                    }
                }

            }
            return RedirectToAction("Index");
        }


        //public ActionResult Delete(int id)
        //{

        //    var EmployeeInfo = new EmployeeInfo();
        //    EmployeeInfo = db.EmployeeInfo.Find(id);
        //    return View(EmployeeInfo);
        //}
        //[HttpPost]
        //public ActionResult Delete(EmployeeInfo model)
        //{

        //    var EmployeeInfo = new EmployeeInfo();
        //    EmployeeInfo = db.EmployeeInfo.Find(model.EmployeeId);
        //    EmployeeInfo.Status = 2;
        //    db.SaveChanges();
        //    return View(EmployeeInfo);
        //}










        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public async Task<ActionResult> Add(EmployeeViewModel model, List<AssignEmployeeLeave> list)
        {
            try
            {

                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                var user = new ApplicationUser() { UserName = model.RegisterViewModel.Email };
                var result = await UserManager.CreateAsync(user, model.RegisterViewModel.Password);

                var userId = string.Empty;
                if (result.Succeeded)
                {
                    userId = db.Users.Where(x => x.UserName == model.RegisterViewModel.Email).FirstOrDefault().Id;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        EmployeeInfo empinfo = new EmployeeInfo();

                        empinfo.EmployeeNo = model.EmployeeInfo.EmployeeNo;
                        empinfo.EmailId = model.RegisterViewModel.Email;
                        empinfo.EmployeeNameNp = model.EmployeeInfo.EmployeeNameNp;
                        empinfo.EmployeeName = model.EmployeeInfo.EmployeeName;
                        empinfo.UserId = userId;
                        empinfo.GenerateReport = 1;
                        empinfo.Status = 1;
                        empinfo.Gender = model.EmployeeInfo.Gender;
                        empinfo.CreatedBy = userIdByUserName;
                        empinfo.CreatedDate = DateTime.Now;
                        empinfo.ModifiedBy = userIdByUserName;
                        empinfo.ModifiedDate = DateTime.Now;

                        db.EmployeeInfo.Add(empinfo);
                        db.SaveChanges();

                        int maxempid = db.EmployeeInfo.Max(z => z.EmployeeId);
                        EmployeeProvider.InsertEmployeeAndOfficeDetail(empinfo, model.EmployeeOfficeInfo,
                            model.EmployeeShiftTime);
                        model.EmployeeInfo.EmployeeId = maxempid;
                        if (list != null)
                        {
                            foreach (var item in list)
                            {
                                AssignEmployeeLeave leave = new AssignEmployeeLeave();
                                int? fiscalYearId = item.FiscalYearId;
                                leave.FiscalYearId = fiscalYearId;
                                leave.EmployeeId = maxempid;
                                leave.FiscalYearId = fiscalYearId;
                                leave.LeaveTypeId = item.LeaveTypeId;
                                leave.OpenningBalance = item.OpenningBalance;
                                leave.NoOfLeave = item.NoOfLeave;
                                item.CreatedBy = EmployeeProvider.GetUserIdByUserName(@User.Identity.Name);
                                item.Status = 1;
                                item.CreatedDate = DateTime.Now;
                                db.AssignEmployeeLeave.Add(leave);
                                db.SaveChanges();
                            }
                        }


                    }
                }
            }
            catch
            {

            }

            return RedirectToAction("Index", "EmployeeInfo");
        }
        //}






        // GET: /EmployeeInfo/Edit/5
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeViewModel model = new EmployeeViewModel();
            model.EmployeeInfo = new EmployeeInfo();
            model.EmployeeInfo = db.EmployeeInfo.Find(id);


            if (model.EmployeeInfo == null)
            {
                return HttpNotFound();
            }
            if (!model.EmployeeInfo.DateOfBirth.HasValue)
            {
                model.EmployeeInfo.NDateOfBirth = new NepaliDateConverter().ToString();
            }

            model.EmployeeOfficeInfo = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == id).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();


            model.EmployeeShiftTime = db.EmployeeShiftTime.Where(x => x.EmployeeId == id).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
            return base.View(model);


        }

        public ActionResult EditProfile(int? id)
        {
            EmployeeInfo model = new EmployeeInfo();
            model = db.EmployeeInfo.Where(x => x.EmployeeId == id).FirstOrDefault();
            model.NDateOfBirth = ((model.DateOfBirth.HasValue && (model.DateOfBirth.Value.Year >= 0x779)) && (model.DateOfBirth.Value.Year <= 0x7fa)) ? NepaliDateConverter.ConvertToNepali(model.DateOfBirth.Value.Date, "yyyy-MM-DD") : "";
            model.RegisterViewModel = new RegisterViewModel();
            model.RegisterViewModel.Email = model.EmailId;
            model.NEntryDate = ((model.EntryDate.HasValue && (model.EntryDate.Value.Year >= 0x779)) && (model.EntryDate.Value.Year <= 0x7fa)) ? NepaliDateConverter.ConvertToNepali(model.EntryDate.Value.Date, "yyyy-MM-DD") : "";
            return base.View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(EmployeeInfo model)
        {

            try
            {



                var userId = string.Empty;
                userId = db.Users.Where(x => x.UserName == model.RegisterViewModel.Email).FirstOrDefault().Id;
                var user = new ApplicationUser() { UserName = model.RegisterViewModel.Email };
                var result = UserManager.RemovePassword(userId);

                if (result.Succeeded)
                {
                    UserManager.AddPassword(userId, model.RegisterViewModel.Password);

                }
            }
            catch (Exception ex)
            {

            }
            try
            {

                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                EmployeeInfo model2 = new EmployeeInfo();

                model2 = db.EmployeeInfo.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault();
                model2.EmployeeNo = model.EmployeeNo;
                model2.EmployeeName = model.EmployeeName;
                model2.EmployeeNameNp = model.EmployeeNameNp;
                if ((model.NDateOfBirth != null) && (model.NDateOfBirth.Length > 3))
                {
                    DateTime time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NDateOfBirth));
                    model2.DateOfBirth = new DateTime?(((time.Year >= 0x779) && (time.Year <= 0x7fa)) ? time : new DateTime(0x76c, 1, 1));
                }
                else
                {
                    model2.DateOfBirth = null;
                }
                model2.Gender = model.Gender;
                model2.MaritalStatus = model.MaritalStatus;
                model2.BloodGroup = model.BloodGroup;
                model2.PhoneNumber = model.PhoneNumber;
                model2.MobileNumber = model.MobileNumber;

                model2.Qualification = model.Qualification;
                model2.NationalityId = model.NationalityId;
                model2.Status = model.Status;
                model2.DisplayOrder = model.DisplayOrder;
                model2.GenerateReport = model.GenerateReport;
                if ((model.NEntryDate != null) && (model.NEntryDate.Length > 3))
                {
                    DateTime time2 = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NEntryDate));
                    model2.EntryDate = new DateTime?(((time2.Year >= 0x779) && (time2.Year <= 0x7fa)) ? time2 : new DateTime(0x76c, 1, 1));
                }
                else
                {
                    model2.EntryDate = null;
                }
                model2.ModifiedBy = userIdByUserName;
                model2.ModifiedDate = DateTime.Now;

                db.Entry(model2).State = EntityState.Modified;
                db.SaveChanges();

            }
            catch
            {
            }
            return base.RedirectToAction("Edit", "EmployeeInfo", new { Id = model.EmployeeId });
        }



        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult EditOfficeInfo(int? id)
        {
            EmployeeOfficeDetail model = new EmployeeOfficeDetail();
            model = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == id).FirstOrDefault();
            if (model == null)
            {
                model = new EmployeeOfficeDetail
                {
                    EmployeeId = id,
                    NEffectiveDate = ""

                };
                //if (model.Status == 3)
                //{
                //    model.BoolValue = false;
                //}
                //else
                //{
                //    model.BoolValue = true;
                //}
            }
            else
            {
                DateTime effectiveDate = model.EffectiveDate;
                model.NEffectiveDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.EffectiveDate.Date), "yyyy-MM-DD");
                // model.NTransferDate = model.TransferDate.HasValue ? NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.TransferDate.Value), "yyyy-MM-DD") : " ";
            }
            return base.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult EditOfficeInfo(EmployeeOfficeDetail model)
        {

            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                EmployeeOfficeDetail model2 = new EmployeeOfficeDetail();

                model2 = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault();


                if (model2 == null)
                {
                    DateTime time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NEffectiveDate));
                    model.EffectiveDate = ((time.Year >= 0x779) && (time.Year <= 0x7fa)) ? time : new DateTime(0x76c, 1, 1);
                    NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NEffectiveDate));
                    model.EmployeeId = model.EmployeeId;
                    model.CreatedBy = userIdByUserName;
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now; ;
                    model.Status = 1;
                    db.EmployeeOfficeDetail.Add(model);
                    db.SaveChanges();
                }
                else
                {
                    model2.EmployeeId = model.EmployeeId;
                    model2.EffectiveDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NEffectiveDate));
                    model2.OfficeId = model.OfficeId;
                    model2.BranchId = model.BranchId;
                    model2.DepartmentId = model.DepartmentId;
                    model2.DesignationId = model.DesignationId;
                    model2.LevelId = model.LevelId;
                    model2.ServiceId = model.ServiceId;
                    model2.Remarks = model.Remarks;


                    model2.ModifiedBy = userIdByUserName;
                    model2.ModifiedDate = DateTime.Now;

                    db.Entry(model2).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch
            {


            }
            return base.RedirectToAction("Edit", "EmployeeInfo", new { Id = model.EmployeeId });
        }


        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult EditAddressInfo(int? id)
        {
            EmployeeInfo model = new EmployeeInfo();
            model = db.EmployeeInfo.Where(x => x.EmployeeId == id).FirstOrDefault();
            return base.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult EditAddressInfo(EmployeeInfo model)
        {

            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                EmployeeInfo model2 = new EmployeeInfo();

                model2 = db.EmployeeInfo.Find(model.EmployeeId);

                model2.PZone = model.PZone;
                model2.PDistrict = model.PDistrict;
                model2.PVdc = model.PVdc;
                model2.PWardNo = model.PWardNo;
                model2.TZone = model.TZone;
                model2.TDistrict = model.TDistrict;
                model2.TVdc = model.TVdc;
                model2.TWardNo = model.TWardNo;
                model2.ModifiedBy = userIdByUserName;
                model2.ModifiedDate = DateTime.Now;
                db.Entry(model2).State = EntityState.Modified;
                db.SaveChanges();

            }
            catch
            {

            }
            return base.RedirectToAction("Edit", "EmployeeInfo", new { Id = model.EmployeeId });
        }

        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]

        public ActionResult EditShiftInfo(int? id)
        {
            EmployeeShiftTime model = new EmployeeShiftTime();
            model = db.EmployeeShiftTime.Where(x => x.EmployeeId == id).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
            if (model == null)
            {
                model = new EmployeeShiftTime
                {
                    EmployeeId = id
                };
            }
            else
            {
                DateTime effectiveDate = model.EffectiveDate;
                model.NEffectiveDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.EffectiveDate.Date), "yyyy-MM-DD");
            }
            model.OfficeId = Convert.ToInt32(db.EmployeeOfficeDetail.Where(x => x.EmployeeId == id).FirstOrDefault().OfficeId);
            return base.View(model);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult EditShiftInfo(EmployeeShiftTime model)
        {

            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                EmployeeShiftTime model2 = new EmployeeShiftTime();

                model2 = db.EmployeeShiftTime.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault();
                if (model2 == null)
                {
                    DateTime time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NEffectiveDate));
                    model.EffectiveDate = ((time.Year >= 0x779) && (time.Year <= 0x7fa)) ? time : new DateTime(0x76c, 1, 1);
                    model.CreatedBy = userIdByUserName;
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now; ;
                    model.Stauts = 1;
                    db.EmployeeShiftTime.Add(model);
                    db.SaveChanges();
                }
                else
                {
                    model2.EffectiveDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NEffectiveDate));
                    model2.ShiftId = model.ShiftId;
                    model2.Remarks = model.Remarks;
                    model2.ModifiedBy = userIdByUserName;
                    model2.ModifiedDate = DateTime.Now;
                    model2.Stauts = 1;
                    db.Entry(model2).State = EntityState.Modified;
                    db.SaveChanges();

                }

            }
            catch
            {

            }
            return base.RedirectToAction("Edit", "EmployeeInfo", new { Id = model.EmployeeId });
        }





        // POST: /EmployeeInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult Edit([Bind(Include = "EmployeeId,EmployeeNo,EmployeeNameNp,EmployeeName,DateOfBirth,Gender,MaritalStatus,BloodGroup,EmailId,PhoneNumber,MobileNumber,Qualification,UserId,PZone,PDistrict,PVdc,PWardNo,TZone,TDistrict,TVdc,TWardNo,NationalityId,EntryDate,DisplayOrder,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] EmployeeInfo employeeinfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeeinfo);
        }

        // GET: /EmployeeInfo/Delete/5
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeInfo employeeinfo = db.EmployeeInfo.Find(id);
            if (employeeinfo == null)
            {
                return HttpNotFound();
            }
            return View(employeeinfo);
        }

        // POST: /EmployeeInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult Delete(EmployeeInfo employeeinfo)
        {
            var data = db.EmployeeInfo.Find(employeeinfo.EmployeeId);
            data.Status = 2;
            // db.EmployeeInfo.Remove(employeeinfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult CheckDuplicateEmail(string Email)
        {
            bool flag = true;
            JsonResult result = new JsonResult();

            return result;


        }

        public JsonResult CheckDuplicateEmployeeNo(string EmployeeNo)
        {

            bool flag = true;

            JsonResult result = new JsonResult();

            return result;

        }


        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult AttendanceRecordDelete(string nLogDate, string officeId, string branchId, string serviceId, string levelId, string designationId, string empId, string statusId)
        {
            DateTime date = DateTime.Now.Date;
            EmployeeAttendanceLogModel model = new EmployeeAttendanceLogModel();
            int num = 0;
            int num2 = 0;
            if (!string.IsNullOrWhiteSpace(nLogDate))
            {
                string str = "";
                if (!string.IsNullOrWhiteSpace(nLogDate))
                {
                    int yy = int.Parse(nLogDate.Split(new char[] { '-' })[0]);
                    int mm = int.Parse(nLogDate.Split(new char[] { '-' })[1]);
                    int dd = int.Parse(nLogDate.Split(new char[] { '-' })[2]);
                    date = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(yy, mm, dd));
                }
                if (!string.IsNullOrEmpty(officeId) && (int.Parse(officeId.Trim()) > 0))
                {
                    int num6 = int.Parse(officeId.Trim());
                    if (num6 > 0)
                    {
                        str = str + " And eoi.OfficeId=" + num6;
                    }
                }
                if (!string.IsNullOrEmpty(branchId) && (int.Parse(branchId.Trim()) > 0))
                {
                    int num7 = int.Parse(branchId.Trim());
                    if (num7 > 0)
                    {
                        str = str + " And eoi.BranchId=" + num7;
                    }
                }
                if (!string.IsNullOrEmpty(serviceId) && (int.Parse(serviceId.Trim()) > 0))
                {
                    int num8 = int.Parse(serviceId.Trim());
                    if (num8 > 0)
                    {
                        str = str + " And eoi.ServiceId=" + num8;
                    }
                }
                if (!string.IsNullOrEmpty(levelId) && (int.Parse(levelId.Trim()) > 0))
                {
                    int num9 = int.Parse(levelId.Trim());
                    if (num9 > 0)
                    {
                        str = str + " And eoi.LevelId=" + num9;
                    }
                }
                if (!string.IsNullOrEmpty(designationId) && (int.Parse(designationId.Trim()) > 0))
                {
                    int num10 = int.Parse(designationId.Trim());
                    if (num10 > 0)
                    {
                        str = str + " And eoi.DesignationId=" + num10;
                    }
                }
                if ((!string.IsNullOrEmpty(empId) && (int.Parse(empId.Trim()) > 0)) && (int.Parse(empId.Trim()) > 0))
                {
                    str = str + " And ei.EmployeeId=" + empId;
                }
                if (!string.IsNullOrEmpty(statusId) && (int.Parse(statusId.Trim()) > 0))
                {
                    num = int.Parse(statusId.Trim());
                    switch (num)
                    {
                        case 1:
                            num2 = 4;
                            break;

                        case 2:
                            num2 = 2;
                            break;
                    }
                }

                using (ApplicationDbContext entities = new ApplicationDbContext())
                {



                    string sql = string.Concat(new object[] { "declare @result table(DeviceDataId int,ForegotEntryId int,EmployeeId int,ShiftTimeId int,EmployeeNo int, LogDate datetime,Status int);\t\r\n\r\n insert into @result(DeviceDataId,ForegotEntryId,EmployeeId,ShiftTimeId,EmployeeNo,LogDate,Status)\r\n Select wdd.DeviceDataId,0,ei.EmployeeId,wdd.ShiftTimeId,wdd.EnrollNumber as EmployeeNo,wdd.[DateTime],wdd.Status\r\n from AttendanceLogs wdd\r\n  inner join EmployeeInfoes ei on wdd.EnrollNumber=ei.EmployeeNo\r\n where wdd.Status='", num, "' and cast(wdd.[DateTime] as date)='", date.ToShortDateString(), "'\r\n\r\ninsert into @result(DeviceDataId,ForegotEntryId,EmployeeId,ShiftTimeId,EmployeeNo,LogDate,Status)\r\n  Select 0,wdd.ForegotEntryId,ei.EmployeeId,wdd.ShiftTimeId,ei.EmployeeNo,cast(ForgotDate as datetime)+cast(ForgotTime as datetime),wdd.Status\r\n from ForgotEntries wdd\r\n inner join EmployeeInfoes ei on wdd.EmployeeId=ei.EmployeeId\r\n  where wdd.Status='", num2, "' and cast(wdd.[ForgotDate] as date)='", date.ToShortDateString(), "'\r\n\r\n                                            Select r.DeviceDataId,r.ForegotEntryId,ei.EmployeeId,r.ShiftTimeId,ei.EmployeeNo,ei.EmployeeName,ei.EmployeeNameNp,\r\n\t\t                                            (Select (case Len(Alias) when 0 then DesignationName else Alias end) from DesignationSetUps where DesignationId=eoi.DesignationId) as DesignationName,\r\n\t\t                                            r.LogDate as [DateTime],r.Status \r\n                                            from @result as r\r\n                                            inner join EmployeeInfoes as ei on r.EmployeeNo=ei.EmployeeNo\r\n                                            inner join EmployeeOfficeDetails eoi on eoi.EmployeeId=ei.EmployeeId\r\n                                            where ei.Status=1 ", str, "\r\n                                            group by ei.EmployeeId,ei.EmployeeName,ei.EmployeeNameNp,ei.EmployeeNo,eoi.DesignationId,r.LogDate,r.DeviceDataId,r.ForegotEntryId,r.ShiftTimeId,r.Status" });




                    model.EmployeeAttendanceLogModelList = entities.Database.SqlQuery<EmployeeAttendanceLogModel>(sql, new object[0]).ToList<EmployeeAttendanceLogModel>();
                }
            }
            return base.View(model);
        }



        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult EmployeeAttendanceConfirmedDelete(string value = "", string nLogDate = "", string officeId = "", string branchId = "", string serviceId = "", string levelId = "", string designationid = "", string employeeId = "", string statusId = "")
        {
            EmployeeAttendanceLogModel model = new EmployeeAttendanceLogModel();
            if (!string.IsNullOrWhiteSpace(value))
            {
                string[] strArray = value.Split(new char[] { ',' });
                if (strArray.Length > 0)
                {
                    model.DeviceDataId = Convert.ToInt32(strArray[0]);
                    model.ForegotEntryId = Convert.ToInt32(strArray[1]);
                }
            }
            model.nLogDate = string.IsNullOrWhiteSpace(nLogDate) ? " " : nLogDate;
            model.OfficeId = new int?(string.IsNullOrWhiteSpace(officeId) ? 0 : Convert.ToInt32(officeId));
            model.BranchId = new int?(string.IsNullOrWhiteSpace(branchId) ? 0 : Convert.ToInt32(branchId));
            model.ServiceId = new int?(string.IsNullOrWhiteSpace(serviceId) ? 0 : Convert.ToInt32(serviceId));
            model.LevelId = new int?(string.IsNullOrWhiteSpace(levelId) ? 0 : Convert.ToInt32(levelId));
            model.DesignationId = new int?(string.IsNullOrWhiteSpace(designationid) ? 0 : Convert.ToInt32(designationid));
            model.EmployeeId = new int?(string.IsNullOrWhiteSpace(employeeId) ? 0 : Convert.ToInt32(employeeId));
            model.Status = new int?(string.IsNullOrWhiteSpace(statusId) ? 0 : Convert.ToInt32(statusId));
            return base.View(model);
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult DeleteEmployeeAttendanceConfirmed(EmployeeAttendanceLogModel model)
        {
            try
            {
                ApplicationDbContext entities = new ApplicationDbContext();
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                int deviceDataId = model.DeviceDataId;
                int forgotEntryId = model.ForegotEntryId;
                if (forgotEntryId > 0)
                {


                    var entity = db.ForgotEntry.Where(x => x.ForegotEntryId == forgotEntryId).FirstOrDefault();
                    entity.ModifiedBy = userIdByUserName;
                    entity.ModifiedDate = DateTime.Now;
                    entity.DeletedDate = new DateTime?(DateTime.Now);
                    entity.DeletedRemarks = model.Remarks;
                    entity.Status = 2;
                    model.EmployeeId = entity.EmployeeId;
                    entities.Entry<ForgotEntry>(entity).State = EntityState.Modified;
                    entities.SaveChanges();
                }
                if (deviceDataId > 0)
                {


                    var entity = db.AttendanceLog.Where(x => x.DeviceDataId == deviceDataId).FirstOrDefault();
                    entity.Status = 2;
                    entity.Remarks = model.Remarks;
                    entity.DeletedBy = userIdByUserName;
                    entity.DeletedDate = DateTime.Now;
                    model.EmployeeId = entity.EmployeeId;
                    entities.Entry<AttendanceLog>(entity).State = EntityState.Modified;
                    entities.SaveChanges();
                }

                TempData.Add("Message", "Delete");
            }
            catch
            {

                TempData.Add("Message", "eleteFailed");
            }
            return base.RedirectToAction("AttendanceRecordDelete", "EmployeeInfo", new
            {
                nLogDate = model.nLogDate,
                officeId = model.OfficeId,
                branchId = model.BranchId,
                serviceId = model.ServiceId,
                levelId = model.LevelId,
                designationId = model.DesignationId,
                empId = model.EmployeeId,
                statusId = model.Status
            });
        }



        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult EditRoles(int EmployeeId = 0)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            AspNetUserRoleModel model = new AspNetUserRoleModel();
            var data = context.EmployeeInfo.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
            model.UserId = data.UserId;
            model.AspNetUserRoleList = new List<AspNetUserRoleModel>();

            var list = context.Database
                 .SqlQuery<AspNetUserRoleModel>(" select * from AspNetUserRoles where UserId='" +
                                model.UserId + "'").ToList();

            foreach (var item in list)
            {
                AspNetUserRoleModel modelnew = new AspNetUserRoleModel();
                modelnew.RoleId = item.RoleId;
                modelnew.Name = context.Database
                    .SqlQuery<String>(" select  Name from AspNetRoles where Id='" +
                                      item.RoleId + "'").FirstOrDefault();
                model.AspNetUserRoleList.Add(modelnew);
            }




            if (User.IsInRole("SuperAdmin"))
            {



                model.AspNetRoleList = context.Roles.Select(m => new AspNetRoleModel() { Id = m.Id, Name = m.Name }).ToList();


            }
            else if (User.IsInRole("Administrator"))
            {
                model.AspNetRoleList = context.Roles.Where(x => x.Name != "Administrator").Select(m => new AspNetRoleModel() { Id = m.Id, Name = m.Name }).ToList();
            }
            else if (User.IsInRole("Admin"))
            {
                model.AspNetRoleList = context.Roles.Where(x => x.Name != "Administrator").Where(x => x.Name != "SuperAdmin").Select(m => new AspNetRoleModel() { Id = m.Id, Name = m.Name }).ToList();

            }
            else if (User.IsInRole("Employee"))
            {
                model.AspNetRoleList = context.Roles.Where(x => x.Name != "Administrator").Where(x => x.Name != "SuperAdmin").Where(x => x.Name != "Admin").Select(m => new AspNetRoleModel() { Id = m.Id, Name = m.Name }).ToList();

            }
            return base.View(model);
        }



        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]

        public ActionResult EditEmpUserRoles(AspNetUserRoleModel model)
        {
            Func<AspNetUserRoleModel, bool> predicate = null;
            string str = "";
            try
            {


                var d = db.Database
                    .SqlQuery<string>("select RoleId from AspNetUserRoles where UserId='" +
                                   model.UserId + "'").FirstOrDefault();
                if (d != null)
                {

                    db.Database
                          .SqlQuery<object>("delete from AspNetUserRoles where UserId='" +
                                         model.UserId + "'").FirstOrDefault();

                    foreach (var item in model.AspNetRoleList)
                    {
                        if (item.IsCheck)
                        {
                            UserManager.AddToRole(model.UserId, item.Name);
                            //Roles.AddUserToRole(UserName, item.Name);
                        }
                    }
                }
                else
                {
                    foreach (var item in model.AspNetRoleList)
                    {
                        if (item.IsCheck)
                        {
                            UserManager.AddToRole(model.UserId, item.Name);
                            //Roles.AddUserToRole(UserName, item.Name);
                        }
                    }
                }


            }





            catch
            {
                str = "fail";
            }

            return base.RedirectToAction("Edit", "EmployeeInfo", new { Id = model.EmployeeId });
        }


        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public PartialViewResult AssignLeave(int id)
        {
            AssignEmployeeLeave model = new AssignEmployeeLeave();
            ApplicationDbContext entities = new ApplicationDbContext();
            model.LeaveTypes = entities.LeaveTypeSetUp.ToList();
            model.EmployeeId = id;
            List<AssignEmployeeLeave>
                enumerable = db.AssignEmployeeLeave.Where(w => w.EmployeeId == id).ToList();
            model.AssignEmployeeLeaveList = enumerable;
            return PartialView("_AssignLeave", model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
        public ActionResult SaveAssignEmployeeLeave(AssignEmployeeLeave assignemployeeLeave)
        {
            FiscalYearSetUp year = db.FiscalYearSetUp.Where(x => x.IsActive == true).FirstOrDefault();
            foreach (var leave in assignemployeeLeave.LeaveTypes)
            {
                assignemployeeLeave.FiscalYearId = year.FiscalYearId;

                if (leave.LeaveApplicationId == null)
                {
                    assignemployeeLeave.CreatedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                    assignemployeeLeave.CreatedDate = DateTime.Now;
                    assignemployeeLeave.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                    assignemployeeLeave.ModifiedDate = DateTime.Now;
                    assignemployeeLeave.Status = 1;
                    assignemployeeLeave.OpenningBalance = leave.OpenningBalance;
                    assignemployeeLeave.NoOfLeave = leave.NoOfLeave;
                    assignemployeeLeave.OfficeId = Utility.GetOfficeIdByEmployeeId(assignemployeeLeave.EmployeeId);
                    assignemployeeLeave.LeaveTypeId = leave.LeaveTypeId;

                    db.AssignEmployeeLeave.Add(assignemployeeLeave);

                    db.SaveChanges();
                }
                else
                {
                    AssignEmployeeLeave byID = db.AssignEmployeeLeave.Where(x => x.LeaveApplicationId == leave.LeaveApplicationId).FirstOrDefault();
                    byID.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                    byID.ModifiedDate = DateTime.Now;
                    byID.Status = 1;
                    byID.OpenningBalance = leave.OpenningBalance;
                    byID.NoOfLeave = leave.NoOfLeave;
                    byID.OfficeId = Utility.GetOfficeIdByEmployeeId(assignemployeeLeave.EmployeeId);
                    byID.LeaveTypeId = leave.LeaveTypeId;

                    db.Entry(byID).State = EntityState.Modified;
                    db.SaveChanges();

                }

            }
            return base.RedirectToAction("Edit", "EmployeeInfo", new { Id = assignemployeeLeave.EmployeeId });
        }



        [Authorize(Roles = "SuperAdmin,Administrator,Admin,Employee")]
        public ActionResult ViewProfile(int id)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            EmployeeViewModel modelnew = new EmployeeViewModel();
            modelnew.EmployeeInfo = new EmployeeInfo();
            modelnew.EmployeeOfficeInfo = new EmployeeOfficeDetail();
            modelnew.EmployeeShiftTime = new EmployeeShiftTime();
            modelnew.EmployeeInfo = dbContext.EmployeeInfo.Where(x => x.EmployeeId == id).FirstOrDefault();
            modelnew.EmployeeOfficeInfo = dbContext.EmployeeOfficeDetail.Where(x => x.EmployeeId == id).FirstOrDefault();
            modelnew.EmployeeShiftTime = db.EmployeeShiftTime.Where(x => x.EmployeeId == id).FirstOrDefault();

            return base.View(modelnew);
        }

















        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public object List { get; set; }
    }
}
