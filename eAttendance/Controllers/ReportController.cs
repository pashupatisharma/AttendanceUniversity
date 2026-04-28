using eAttendance.Models;
using eAttendance.ReportModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eAttendance.Controllers
{
    [Authorize]
    public partial class ReportController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public JsonResult GetEmpployeeListAccordingToOfficeAndPerDateWithAllOrSelect(int? officeId, string nLogDate, int? branchId = 0, int? serviceId = 0, int? levelId = 0, int? designationId = 0, bool GenerateReportStatus = false, bool withAll = false, bool withSelect = false)
        {


            DateTime date = DateTime.Now.Date;

            if (!string.IsNullOrWhiteSpace(nLogDate))
            {
                int yy = int.Parse(nLogDate.Split(new char[] { '-' })[0]);
                int mm = int.Parse(nLogDate.Split(new char[] { '-' })[1]);
                int dd = int.Parse(nLogDate.Split(new char[] { '-' })[2]);
                date = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(yy, mm, dd));
            }



            IEnumerable<EmployeeAttendanceList> source = db.EmployeeOfficeDetail.Where(x => x.OfficeId == officeId).Select(m => new EmployeeAttendanceList()
            {
                OfficeId = (int)m.OfficeId,
                LevelId = (int)m.LevelId,
                EmployeeId = (int)m.EmployeeId,
                BranchId = (int)m.BranchId,
                ServiceId = (int)m.ServiceId,
                DesignationId = (int)m.DesignationId,

            });
            List<SelectListItem> list = new List<SelectListItem>();
            if (branchId > 0)
            {

                source = source.Where(x => x.BranchId == branchId);
            }
            if (serviceId > 0)
            {
                source = source.Where(x => x.ServiceId == serviceId);
            }
            if (levelId > 0)
            {
                source = source.Where(x => x.LevelId == levelId);
            }

            if (designationId > 0)
            {
                source = source.Where(x => x.DesignationId == designationId);
            }
            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {

            }

            else if (User.IsInRole("Employee") && !User.IsInRole("Admin"))
            {

                var userid = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
                var employeeId = EmployeeProvider.GetEmployeeIdByUserId(userid);

                source = source.Where(x => x.EmployeeId == employeeId);



            }

            foreach (var item in source.ToList())
            {
                var nameAndCode = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                string t = "[" + nameAndCode.EmployeeNo + "]" + nameAndCode.EmployeeNameNp;
                list.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = t });

            }

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin") || User.IsInRole("Administrator"))
            {


                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "र्छान्नुहोस्";

                list.Insert(0, item);
            }

            return Json(list);
        }



        [HttpPost]
        public JsonResult GetEmployeeBy_Year_Month_OfficeIdAndFilteringWithAllOrSelect(int? year, int? month, int officeId, int branchId = 0, int serviceId = 0, int levelId = 0, int designationId = 0, bool GenerateReportStatus = false, bool withAll = false, bool withSelect = false)
        {
            DateTime[] timeArray = NepaliDateConverter.GetMonth(Convert.ToInt32(year), Convert.ToInt32(month));
            DateTime fromDate = timeArray[0];
            DateTime toDate = timeArray[1];
            List<EmployeeAttendanceList> list = ReportService.ReportService
                .GetEmployeeBy_FromDate_ToDate_OfficeIdList(fromDate, toDate, officeId, true).ToList();

            if (branchId > 0)
            {
                list = list.Where(x => x.BranchId == branchId).ToList();
            }
            if (serviceId > 0)
            {
                list = list.Where(x => x.ServiceId == serviceId).ToList();
            }
            if (levelId > 0)
            {
                list = list.Where(x => x.LevelId == levelId).ToList();
            }
            if (designationId > 0)
            {
                list = list.Where(x => x.DesignationId == designationId).ToList();
            }

            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {

            }

            else if (User.IsInRole("Employee") && !User.IsInRole("Admin"))
            {

                var userid = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
                var employeeId = EmployeeProvider.GetEmployeeIdByUserId(userid);

                list = list.Where(x => x.EmployeeId == employeeId).ToList();



            }



            List<SelectListItem> newlist = new List<SelectListItem>();
            foreach (var item in list.ToList())
            {
                var nameAndCode = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                string t = "[" + nameAndCode.EmployeeNo + "]" + nameAndCode.EmployeeNameNp;

                newlist.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = t });
            }

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin") || User.IsInRole("Administrator"))
            {


                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "र्छान्नुहोस्";

                newlist.Insert(0, item);
            }


            return Json(newlist);
        }


        [HttpPost]
        public JsonResult GetEmployeeOfSelectedOffice(int? year, int? month, int officeId, int branchId = 0, int serviceId = 0, int levelId = 0, int designationId = 0, bool GenerateReportStatus = false, bool withAll = false, bool withSelect = false)
        {
         
            List<EmployeeAttendanceList> list = ReportService.ReportService
                .GetEmployeeBy_FromDate_ToDate_OfficeIdList(DateTime.Now, DateTime.Now, officeId, true).ToList();

           
            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {

            }

            else if (User.IsInRole("Employee") && !User.IsInRole("Admin"))
            {

                var userid = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
                var employeeId = EmployeeProvider.GetEmployeeIdByUserId(userid);

                list = list.Where(x => x.EmployeeId == employeeId).ToList();



            }



            List<SelectListItem> newlist = new List<SelectListItem>();
            foreach (var item in list.ToList())
            {
                var nameAndCode = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                string t = "[" + nameAndCode.EmployeeNo + "]" + nameAndCode.EmployeeNameNp;

                newlist.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = t });
            }

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin") || User.IsInRole("Administrator"))
            {


                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "र्छान्नुहोस्";

                newlist.Insert(0, item);
            }


            return Json(newlist);
        }












        public JsonResult GetEmployeeBy_FromDate_ToDate_OfficeIdAndFilteringWithAllOrSelect(string nFromDate, string nToDate, int officeId, int branchId = 0, int serviceId = 0, int levelId = 0, int designationId = 0, bool withAll = false, bool withSelect = false)
        {
            DateTime fromDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(nFromDate));
            DateTime toDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(nToDate));
            List<SelectListItem> list = new List<SelectListItem>();
            IEnumerable<EmployeeAttendanceList> source = ReportService.ReportService.GetEmployeeBy_FromDate_ToDate_OfficeIdList(fromDate, toDate, officeId, false).ToList();
            if (branchId > 0)
            {

                source = source.Where(x => x.BranchId == branchId);
            }
            if (serviceId > 0)
            {
                source = source.Where(x => x.ServiceId == serviceId);
            }
            if (levelId > 0)
            {
                source = source.Where(x => x.LevelId == levelId);
            }

            if (designationId > 0)
            {
                source = source.Where(x => x.DesignationId == designationId);
            }



            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {

            }

            else if (User.IsInRole("Employee") && !User.IsInRole("Admin"))
            {

                var userid = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
                var employeeId = EmployeeProvider.GetEmployeeIdByUserId(userid);

                source = source.Where(x => x.EmployeeId == employeeId).ToList();



            }










            List<SelectListItem> newlist = new List<SelectListItem>();
            foreach (var item in source.ToList())
            {
                var nameAndCode = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                string t = "[" + nameAndCode.EmployeeNo + "]" + nameAndCode.EmployeeNameNp;

                newlist.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = t });
            }

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin") || User.IsInRole("Administrator"))
            {


                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "र्छान्नुहोस्";

                newlist.Insert(0, item);
            }



            return Json(newlist);
        }


        public JsonResult GetEmployeeBy_Year_OfficeWithAllOrSelect(int year, int officeId, int branchId = 0, int serviceId = 0, int levelId = 0, int designationId = 0, bool withAll = false, bool withSelect = false)
        {
            DateTime[] month = NepaliDateConverter.GetMonth(year, 1);
            DateTime[] timeArray2 = NepaliDateConverter.GetMonth(year, 12);
            DateTime fromDate = month[0];
            DateTime toDate = timeArray2[1];


            IEnumerable<EmployeeAttendanceList> source = ReportService.ReportService.GetEmployeeBy_FromDate_ToDate_OfficeIdList(fromDate, toDate, officeId, false).ToList();
            if (branchId > 0)
            {

                source = source.Where(x => x.BranchId == branchId);
            }
            if (serviceId > 0)
            {
                source = source.Where(x => x.ServiceId == serviceId);
            }
            if (levelId > 0)
            {
                source = source.Where(x => x.LevelId == levelId);
            }

            if (designationId > 0)
            {
                source = source.Where(x => x.DesignationId == designationId);
            }




            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {

            }

            else if (User.IsInRole("Employee") && !User.IsInRole("Admin"))
            {

                var userid = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
                var employeeId = EmployeeProvider.GetEmployeeIdByUserId(userid);

                source = source.Where(x => x.EmployeeId == employeeId).ToList();



            }

            List<SelectListItem> newlist = new List<SelectListItem>();
            foreach (var item in source.ToList())
            {
                var nameAndCode = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                string t = "[" + nameAndCode.EmployeeNo + "]" + nameAndCode.EmployeeNameNp;

                newlist.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = t });
            }

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin") || User.IsInRole("Administrator"))
            {


                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "र्छान्नुहोस्";

                newlist.Insert(0, item);
            }
            return Json(newlist);

        }


        public JsonResult GetEmployeeBy_FiscalYear_OfficeWithAllOrSelect(int fiscalYearId, int officeId, int branchId = 0, int serviceId = 0, int levelId = 0, int designationId = 0, bool withAll = false, bool withSelect = false)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            FiscalYearSetUp year = db.FiscalYearSetUp.Where(x => x.FiscalYearId == fiscalYearId).FirstOrDefault();
            DateTime? fromDate = year.FromDate;
            DateTime? toDate = year.ToDate;

            IEnumerable<EmployeeAttendanceList> source = ReportService.ReportService.GetEmployeeBy_FromDate_ToDate_OfficeIdList(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), officeId, false).ToList();
            if (branchId > 0)
            {

                source = source.Where(x => x.BranchId == branchId);
            }
            if (serviceId > 0)
            {
                source = source.Where(x => x.ServiceId == serviceId);
            }
            if (levelId > 0)
            {
                source = source.Where(x => x.LevelId == levelId);
            }

            if (designationId > 0)
            {
                source = source.Where(x => x.DesignationId == designationId);
            }


            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {

            }

            else if (User.IsInRole("Employee") && !User.IsInRole("Admin"))
            {

                var userid = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
                var employeeId = EmployeeProvider.GetEmployeeIdByUserId(userid);

                source = source.Where(x => x.EmployeeId == employeeId).ToList();



            }

            List<SelectListItem> newlist = new List<SelectListItem>();
            foreach (var item in source.ToList())
            {
                var nameAndCode = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                string t = "[" + nameAndCode.EmployeeNo + "]" + nameAndCode.EmployeeNameNp;

                newlist.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = t });
            }

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin") || User.IsInRole("Administrator"))
            {


                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "र्छान्नुहोस्";

                newlist.Insert(0, item);
            }


            return base.Json(newlist, 0);
        }




        public JsonResult FromDateToDateCompareRemark(string nFromDate, string nToDate)
        {
            DateTime time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(nFromDate));
            DateTime time2 = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(nToDate));
            int num = 2;
            if (time2 >= time)
            {
                num = 1;
            }
            else
            {
                num = 2;
            }
            return base.Json(num, 0);
        }











    }
}