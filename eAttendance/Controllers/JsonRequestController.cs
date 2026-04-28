using eAttendance.Models;
using eAttendance.ReportModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace eAttendance.Controllers
{
    public class JsonRequestController : Controller


    {

        [HttpPost]
        public JsonResult GetEmployeeBy_Year_Month_OfficeIdAndFilteringWithAllOrSelect(int year, int month, int officeId, int branchId = 0, int serviceId = 0, int levelId = 0, int designationId = 0, bool GenerateReportStatus = false, bool withAll = false, bool withSelect = false)
        {
            ApplicationDbContext db = new ApplicationDbContext();
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




        public JsonResult LoadLeaveTypeByEmployeeId(int employeeId)
        {



            ApplicationDbContext entities = new ApplicationDbContext();
            FiscalYearSetUp fiscalYear = entities.FiscalYearSetUp.Where(x => x.IsActive == true).FirstOrDefault();

            string s = "GetLeaveTypeAssignByEmployee" + " " + "'" + fiscalYear.FiscalYearId + "'" + "," + employeeId;
            ((IObjectContextAdapter)entities).ObjectContext.CommandTimeout = 180;

            var list = entities.Database.SqlQuery<LeaveTypeSetUpModel>(s).ToList();
            List<SelectListItem> selectListlist = new List<SelectListItem>();
            foreach (var item in list)
            {

                selectListlist.Add(
                    new SelectListItem() { Value = item.LeaveTypeId.ToString(), Text = item.LeaveTypeName });

            }

            SelectListItem itemnew = new SelectListItem
            {
                Value = "0",
                Text = "छान्नुहोस"
            };
            selectListlist.Insert(0, itemnew);

            return Json(selectListlist);
        }

        public JsonResult RecommendLeaveApplicationById(int leaveApplicationId)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            int num = 0;

            var model = entities.LeaveApplication
                 .Where(x => x.RecomenderStatus == 1 && x.LeaveApplicationId == leaveApplicationId && x.Status == 1)
                 .FirstOrDefault();

            if (model != null)
            {
                model.RecomenderStatus = 2;
                model.RecomendedDate = new DateTime?(DateTime.Now);
                num = entities.SaveChanges();
            }
            int num2 = num;
            return base.Json(num2, 0);
        }

       

        public JsonResult RecommendLeaveApplicationRejectById(int leaveApplicationId)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            int num = 0;

            var model = entities.LeaveApplication
                .Where(x => x.RecomenderStatus == 1 && x.LeaveApplicationId == leaveApplicationId && x.Status == 1)
                .FirstOrDefault();

            if (model != null)
            {
                model.RecomenderStatus = 3;
                model.RecomendedDate = new DateTime?(DateTime.Now);
                num = entities.SaveChanges();
            }
            int num2 = num;
            return base.Json(num2, 0);
        }

        public ActionResult ApproveLeaveApplicationById(int leaveApplicationId)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            int num = 0;

            var model = entities.LeaveApplication
                .Where(x => x.ApprovedStatus == 1 && x.LeaveApplicationId == leaveApplicationId && x.Status == 1 && x.RecomenderStatus == 2)
                .FirstOrDefault();

            if (model != null)
            {
                model.ApprovedStatus = 2;
                model.ApprovedDate = new DateTime?(DateTime.Now);
                num = entities.SaveChanges();
            }
            int num2 = num;
            return base.Json(num2, 0);
        }


        public ActionResult ApproveLeaveApplicationRejectById(int leaveApplicationId)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            int num = 0;

            var model = entities.LeaveApplication
                .Where(x => x.ApprovedStatus == 1 && x.LeaveApplicationId == leaveApplicationId && x.Status == 1 && x.RecomenderStatus == 2)
                .FirstOrDefault();

            if (model != null)
            {
                model.ApprovedStatus = 3;
                model.ApprovedDate = new DateTime?(DateTime.Now);
                num = entities.SaveChanges();
            }
            int num2 = num;
            return base.Json(num2, 0);
        }





        public JsonResult RecommendVisitApplicationById(int visitApplicationId)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            int num = 0;

            var model = entities.VisitApplication
                 .Where(x => x.RecomenderStatus == 1 && x.VisitApplicationId == visitApplicationId && x.Status == 1)
                 .FirstOrDefault();

            if (model != null)
            {
                model.RecomenderStatus = 2;
                model.RecomendedDate = new DateTime?(DateTime.Now);
                num = entities.SaveChanges();
            }
            int num2 = num;
            return base.Json(num2, 0);
        }

        public JsonResult RecommendVisitApplicationRejectById(int visitApplicationId)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            int num = 0;

            var model = entities.VisitApplication
                .Where(x => x.RecomenderStatus == 1 && x.VisitApplicationId == visitApplicationId && x.Status == 1)
                .FirstOrDefault();

            if (model != null)
            {
                model.RecomenderStatus = 3;
                model.RecomendedDate = new DateTime?(DateTime.Now);
                num = entities.SaveChanges();
            }
            int num2 = num;
            return base.Json(num2, 0);
        }

        public ActionResult ApproveVisitApplicationById(int visitApplicationId)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            int num = 0;

            var model = entities.VisitApplication
                .Where(x => x.ApprovedStatus == 1 && x.VisitApplicationId == visitApplicationId && x.Status == 1 && x.RecomenderStatus == 2)
                .FirstOrDefault();

            if (model != null)
            {
                model.ApprovedStatus = 2;
                model.ApprovedDate = DateTime.Now;
                num = entities.SaveChanges();
            }
            int num2 = num;
            return base.Json(num2, 0);
        }


        public ActionResult ApproveVisitApplicationRejectById(int visitApplicationId)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            int num = 0;

            var model = entities.VisitApplication
                .Where(x => x.ApprovedStatus == 1 && x.VisitApplicationId == visitApplicationId && x.Status == 1 && x.RecomenderStatus == 2)
                .FirstOrDefault();

            if (model != null)
            {
                model.ApprovedStatus = 3;
                model.ApprovedDate = DateTime.Now;
                num = entities.SaveChanges();
            }
            int num2 = num;
            return base.Json(num2, 0);
        }










        public JsonResult LoadLeaveBalance(int employeeId, int leaveTypeId, int LeaveApplicationId = 0)
        {
            ApplicationDbContext entities = new ApplicationDbContext();

            Func<LeaveTypeSetUp, bool> predicate = null;
            decimal num = 0M;
            decimal result = 0M;
            decimal num3 = 0M;
            decimal num4 = 0M;
            decimal num5 = 0M;
            decimal num6 = 0M;

            if ((employeeId >= 0) && (leaveTypeId > 0))
            {
                FiscalYearSetUp fiscalYear = entities.FiscalYearSetUp.Where(x => x.IsActive == true).FirstOrDefault();


                DateTime? FromDate = fiscalYear.FromDate;
                DateTime? ToDate = fiscalYear.ToDate;
                AssignEmployeeLeave model = new AssignEmployeeLeave();
                //AssignEmployeeLeaveProvider provider = new AssignEmployeeLeaveProvider();
                //LeaveApplicationProvider provider2 = new LeaveApplicationProvider();
                //SetupLeaveTypeProvider provider3 = new SetupLeaveTypeProvider();

                if (predicate == null)
                {
                    predicate = x => (x.LeaveTypeId == leaveTypeId) && (x.Status == 1);
                }
                LeaveTypeSetUp model2 = new LeaveTypeSetUp();
                model2 = entities.LeaveTypeSetUp.Where(x => x.LeaveTypeId == leaveTypeId).Where(x => x.Status == 1).FirstOrDefault();
                List<LeaveApplication> list = new List<LeaveApplication>();
                list = entities.LeaveApplication.Where(x => x.EmployeeId == employeeId).Where(x => x.Status == 1).Where(x => x.LeaveTypeId == leaveTypeId).ToList();
                //list = provider2.GetAll().Where<LeaveApplicationModel>(delegate(LeaveApplicationModel x)
                //{
                //    int? approvedStatus;
                //    DateTime? nullable2;
                //    DateTime time;
                //    if (((x.EmployeeId == employeeId) && (x.Status == 1)) && (x.LeaveTypeId == leaveTypeId))
                //    {
                //        approvedStatus = x.ApprovedStatus;
                //    }
                //    return (((approvedStatus.GetValueOrDefault() == 2) && approvedStatus.HasValue) && (((nullable2 = x.FromDate).HasValue ? (nullable2.GetValueOrDefault() >= (time = FromDate)) : false) || ((nullable2 = x.ToDate).HasValue ? (nullable2.GetValueOrDefault() <= (time = ToDate)) : false)));
                //}).ToList<LeaveApplicationModel>();
                LeaveApplication model3 = new LeaveApplication();
                if (LeaveApplicationId > 0)
                {
                    //if (func2 == null)
                    //{
                    //    func2 = delegate(LeaveApplicationModel x)
                    //    {
                    //        int? nullable;
                    //        return ((x.LeaveApplicationId == LeaveApplicationId) && (x.Status == 1)) && (((nullable = x.ApprovedStatus).GetValueOrDefault() == 2) && nullable.HasValue);
                    //    };
                    //}
                    model3 = entities.LeaveApplication.Where(x => x.LeaveApplicationId == LeaveApplicationId).Where(x => x.Status == 1).Where(x => x.ApprovedStatus == 2).FirstOrDefault();
                    if (model3 == null)
                    {
                        model3 = new LeaveApplication();
                    }
                }

                //model = provider.GetAll().Where<AssignEmployeeLeaveModel>(delegate(AssignEmployeeLeaveModel x)
                //{
                //    int? nullable;
                //    return ((((x.EmployeeId == employeeId) && (x.LeaveTypeId == leaveTypeId)) && (x.Status == 1)) && (((nullable = x.FiscalYearId).GetValueOrDefault() == fiscalYear.FiscalYearId) && nullable.HasValue));
                //}).FirstOrDefault<AssignEmployeeLeaveModel>();

                model = entities.AssignEmployeeLeave.Where(x => x.EmployeeId == employeeId).Where(x => x.LeaveTypeId == leaveTypeId).Where(x => x.Status == 1).Where(x => x.FiscalYearId == fiscalYear.FiscalYearId).FirstOrDefault();

                if (model != null)
                {
                    if (model.OpenningBalance != 0M)
                    {
                        decimal.TryParse(model.OpenningBalance.ToString(), out result);
                    }
                    if (model.NoOfLeave != 0M)
                    {
                        decimal.TryParse(model.NoOfLeave.ToString(), out num3);
                    }
                }
                if ((list.Count > 0) && (list != null))
                {
                    foreach (LeaveApplication model4 in list)
                    {
                        DateTime time;
                        ApplicationDbContext newentities = new ApplicationDbContext();
                        //   AttendanceEntities entities = new AttendanceEntities();
                        if (model3.LeaveApplicationId == model4.LeaveApplicationId)
                        {
                            continue;
                        }
                        DateTime day = Convert.ToDateTime(model4.FromDate);
                        goto Label_0723;
                    Label_0371:
                        if ((day.Date >= FromDate.Value.Date) && (day.Date <= ToDate.Value.Date))
                        {
                            if (model2 != null)
                            {


                                if (!model2.HolidayInclude || !model2.WeeklyOffInclude)
                                {
                                    //var list2 = (from holidayCalendar in newentities.HolidayCalender
                                    //             where (holidayCalendar.Status == 1) && ((day > holidayCalendar.FromDate) && (day <= holidayCalendar.ToDate))
                                    //             select new
                                    //             {
                                    //                 HolidayCalendarId = holidayCalendar.HolidayCalendarId,
                                    //                 HolidayTypeName = holidayCalendar.HolidayTypeName
                                    //             }).ToList();
                                    var list2 = newentities.HolidayCalender.Where(x => x.Status == 1).Where(x => x.FromDate < day).Where(x => x.ToDate >= day).ToList();

                                    if (!model2.HolidayInclude && model2.WeeklyOffInclude)
                                    {
                                        if (list2.Count <= 0)
                                        {
                                            num4 += model4.IsHalfDay ? Convert.ToDecimal(0.5) : 1M;
                                        }
                                    }
                                    else if (model2.HolidayInclude && !model2.WeeklyOffInclude)
                                    {
                                        if (day.DayOfWeek.ToString() != "Saturday")
                                        {
                                            num4 += model4.IsHalfDay ? Convert.ToDecimal(0.5) : 1M;
                                        }
                                    }
                                    else if ((!model2.HolidayInclude && !model2.WeeklyOffInclude) && ((list2.Count <= 0) && (day.DayOfWeek.ToString() != "Saturday")))
                                    {
                                        num4 += model4.IsHalfDay ? Convert.ToDecimal(0.5) : 1M;
                                    }
                                }
                                else
                                {
                                    num4 += model4.IsHalfDay ? Convert.ToDecimal(0.5) : 1M;
                                }
                            }
                        }
                        day = day.AddDays(1.0);
                    Label_0723:
                        time = day.Date;
                        DateTime? toDate = model4.ToDate;
                        if (toDate.HasValue ? (time <= toDate.GetValueOrDefault()) : false)
                        {
                            goto Label_0371;
                        }
                    }
                }
                num5 = result + num3;
                num6 = num5 - num4;
                if (num6 > 0M)
                {
                    num = num6;
                }
                else
                {
                    num = 0M;
                }
            }
            var type = new
            {
                LeaveBalance = num,
                TotalLeave = num5,
                TakenLeave = num4
            };
            return base.Json(type, 0);
        }




        public JsonResult LoadTotalApplyingLeave(string fromDate = "", string toDate = "", int leaveTypeId = 0, [DecimalConstant(0, 0, 0, 0, (uint)0)] decimal remainingLeave = 0)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            Func<LeaveTypeSetUp, bool> predicate = null;
            FiscalYearSetUp fiscalYear = db.FiscalYearSetUp.Where(x => x.IsActive == true).FirstOrDefault();

            DateTime? time = fiscalYear.FromDate;
            DateTime? time2 = fiscalYear.ToDate;
            decimal d = 0M;
            decimal num2 = 0M;
            decimal num3 = 0M;

            LeaveTypeSetUp provider = new LeaveTypeSetUp();

            LeaveTypeSetUp model = db.LeaveTypeSetUp.Where(x => x.LeaveTypeId == leaveTypeId).FirstOrDefault();

            bool allowLeaveLimitExceed = model.AllowLeaveLimitExceed;
            bool holidayInclude = model.HolidayInclude;
            bool weeklyOffInclude = model.WeeklyOffInclude;
            int num4 = CheckDateInterval(fromDate, toDate, remainingLeave, allowLeaveLimitExceed, holidayInclude, weeklyOffInclude);
            if ((num4 == 1) && (leaveTypeId > 0))
            {
                var list = new List<HolidayCalender>();
                LeaveTypeSetUp provider2 = new LeaveTypeSetUp();
                if (predicate == null)
                {
                    predicate = x => (x.LeaveTypeId == leaveTypeId) && (x.Status == 1);
                }
                LeaveTypeSetUp model2 = db.LeaveTypeSetUp.Where(x => x.LeaveTypeId == leaveTypeId).Where(x => x.Status == 1).FirstOrDefault();

                if ((((fromDate != "") && (toDate != "")) && (fromDate != "____-__-__")) && (toDate != "____-__-__"))
                {
                    DateTime time3 = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(fromDate));
                    DateTime time4 = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(toDate));
                    for (DateTime day = time3.Date; day.Date <= time4.Date; day = day.AddDays(1.0))
                    {

                        list = db.HolidayCalender.Where(x => x.Status == 1).Where(x => x.FromDate < day).Where(x => x.ToDate >= day).ToList();
                        if (list.Count > 0)
                        {
                            num2 = num2++;
                        }
                        if (day.DayOfWeek.ToString() == "Saturday")
                        {
                            num3 = num3++;
                        }
                        if (!model2.HolidayInclude || !model2.WeeklyOffInclude)
                        {
                            if (!model2.HolidayInclude && model2.WeeklyOffInclude)
                            {
                                if (list.Count <= 0)
                                {
                                    d = d++;
                                }
                            }
                            else if (model2.HolidayInclude && !model2.WeeklyOffInclude)
                            {
                                if (day.DayOfWeek.ToString() != "Saturday")
                                {
                                    // d = decimal.op_Increment(d);
                                    d = d++;
                                }
                            }
                            else if ((!model2.HolidayInclude && !model2.WeeklyOffInclude) && ((list.Count <= 0) && (day.DayOfWeek.ToString() != "Saturday")))
                            {
                                d = d++;
                            }
                        }
                        else
                        {
                            d = d++;
                        }
                    }
                }
                else if (((fromDate != "") || (toDate != "")) && ((fromDate != "____-__-__") || (toDate != "____-__-__")))
                {
                    if ((fromDate != "") && (fromDate != "____-__-__"))
                    {
                        DateTime StartDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(fromDate));
                        //list = db.HolidayCalender.Where(x => x.Status == 1).Where(x => x.FromDate < day).Where(x => x.ToDate >= day).ToList();
                        list = db.HolidayCalender.Where(x => x.FromDate >= StartDate).ToList();


                        if (list.Count > 0)
                        {
                            num2 = num2++;
                        }
                        if (StartDate.DayOfWeek.ToString() == "Saturday")
                        {
                            num3 = num3++;
                        }
                        if (!model2.HolidayInclude || !model2.WeeklyOffInclude)
                        {
                            if (!model2.HolidayInclude && model2.WeeklyOffInclude)
                            {
                                if (list.Count <= 0)
                                {
                                    d = d++;
                                }
                            }
                            else if (model2.HolidayInclude && !model2.WeeklyOffInclude)
                            {
                                if (StartDate.DayOfWeek.ToString() != "Saturday")
                                {
                                    d = d++;
                                }
                            }
                            else if ((!model2.HolidayInclude && !model2.WeeklyOffInclude) && ((list.Count <= 0) && (StartDate.DayOfWeek.ToString() != "Saturday")))
                            {
                                d = d++;
                            }
                        }
                        else
                        {
                            d = d++;
                        }
                    }
                    if ((toDate != "") && (toDate != "____-__-__"))
                    {
                        DateTime EndDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(toDate));
                        //list = db.HolidayCalender.Where(x => x.Status == 1).Where(x => x.FromDate < day).Where(x => x.ToDate >= day).ToList();
                        list = db.HolidayCalender.Where(x => x.ToDate <= EndDate).ToList();
                        if (list.Count > 0)
                        {
                            num2 = num2++;
                        }
                        if (EndDate.DayOfWeek.ToString() == "Saturday")
                        {
                            num3 = num3++;
                        }
                        if (!model2.HolidayInclude || !model2.WeeklyOffInclude)
                        {
                            if (!model2.HolidayInclude && model2.WeeklyOffInclude)
                            {
                                if (list.Count <= 0)
                                {
                                    d = d++;
                                }
                            }
                            else if (model2.HolidayInclude && !model2.WeeklyOffInclude)
                            {
                                if (EndDate.DayOfWeek.ToString() != "Saturday")
                                {
                                    d = d++;
                                }
                            }
                            else if ((!model2.HolidayInclude && !model2.WeeklyOffInclude) && ((list.Count <= 0) && (EndDate.DayOfWeek.ToString() != "Saturday")))
                            {
                                d = d++;
                            }
                        }
                        else
                        {
                            d = d++;
                        }
                    }
                    if (allowLeaveLimitExceed)
                    {
                        num4 = 1;
                    }
                    else if (remainingLeave < d)
                    {
                        num4 = 4;
                    }
                }
            }
            var type = new
            {
                TotalLeave = d,
                TotalHolidays = num2,
                TotalSaturdays = num3,
                Flag = num4
            };
            return base.Json(type, 0);
        }



        public int CheckDateInterval(string NFromDate = "", string NToDate = "", [DecimalConstant(0, 0, 0, 0, (uint)0)] decimal RemainingLeave = 0, bool IsLeaveTypeAllowLimitExceed = false, bool IsHolidayInclude = false, bool IsWeekdayInclude = false)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            DateTime time;
            DateTime time2;
            int num = 0;
            if (((NFromDate != "") && (NFromDate != "____-__-__")) || ((NToDate != "") && (NToDate != "____-__-__")))
            {
                FiscalYearSetUp year = db.FiscalYearSetUp.Where(x => x.IsActive == true).FirstOrDefault();
                if ((NFromDate != "") && (NFromDate != "____-__-__"))
                {
                    time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(NFromDate));
                    if ((time < year.FromDate) || (time > year.ToDate))
                    {
                        return (num = 5);
                    }
                }
                if ((NToDate != "") && (NToDate != "____-__-__"))
                {
                    time2 = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(NToDate));
                    if ((time2 < year.FromDate) || (time2 > year.ToDate))
                    {
                        return (num = 5);
                    }
                }
            }
            if ((((NFromDate != "") && (NToDate != "")) && (NFromDate != "____-__-__")) && (NToDate != "____-__-__"))
            {
                time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(NFromDate));
                time2 = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(NToDate));
                if (time2 >= time)
                {
                    if (IsLeaveTypeAllowLimitExceed)
                    {
                        return 1;
                    }
                    if (RemainingLeave > 0M)
                    {

                        int num2 = 0;
                        for (DateTime day = time.Date; day.Date <= time2.Date; day = day.AddDays(1.0))
                        {
                            if ((day.Date >= time.Date) && (day.Date <= time2.Date))
                            {
                                if (!IsHolidayInclude || !IsWeekdayInclude)
                                {

                                    var list = db.HolidayCalender.Where(x => x.Status == 1).Where(x => x.FromDate < day).Where(x => x.ToDate >= day).ToList();
                                    if (!IsHolidayInclude && IsWeekdayInclude)
                                    {
                                        if (list.Count <= 0)
                                        {
                                            num2++;
                                        }
                                    }
                                    else if (IsHolidayInclude && !IsWeekdayInclude)
                                    {
                                        if (day.DayOfWeek.ToString() != "Saturday")
                                        {
                                            num2++;
                                        }
                                    }
                                    else if ((!IsHolidayInclude && !IsWeekdayInclude) && ((list.Count <= 0) && (day.DayOfWeek.ToString() != "Saturday")))
                                    {
                                        num2++;
                                    }
                                }
                                else
                                {
                                    num2++;
                                }
                            }
                        }
                        if (num2 <= RemainingLeave)
                        {
                            num = 1;
                        }
                        else
                        {
                            num = 2;
                        }
                        return num;
                    }
                    return 4;
                }
                return 3;
            }
            return 1;
        }








    }
}