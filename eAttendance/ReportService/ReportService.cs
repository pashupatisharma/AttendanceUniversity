using eAttendance;
using eAttendance.Models;
using eAttendance.ReportModel;
using eAttendance.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace ReportService
{
    public static class ReportService
    {
        private static readonly string[] FullAccessRoles =
       {
        "Admin",
        "SuperAdmin",
        "Administrator"
    };

        private static IQueryable<EmployeeOfficeDetail> GetEmployeeOfficeQuery(
            ApplicationDbContext db,
            int officeId,
            IPrincipal user)
        {
            var query = db.EmployeeOfficeDetail
                .Where(x => x.OfficeId == officeId);

            // Admin users can see all employees
            bool isFullAccess =
                user.IsInRole("Admin") ||
                user.IsInRole("SuperAdmin") ||
                user.IsInRole("Administrator");

            if (!isFullAccess)
            {
                // Get logged-in user's EmployeeId
                string userId = user.Identity.GetUserId();

                // Change "UserId" below if your EmployeeInfo
                // uses another field to connect with ASP.NET Identity.
                int? employeeId = db.EmployeeInfo
                    .Where(x => x.UserId == userId)
                    .Select(x => (int?)x.EmployeeId)
                    .FirstOrDefault();

                if (!employeeId.HasValue)
                {
                    // No employee associated with this user
                    return query.Where(x => false);
                }

                // Employee can see only his/her own record
                query = query.Where(x => x.EmployeeId == employeeId.Value);
            }

            return query;
        }

        private static List<EmployeeAttendanceList> GetEmployeeList(
    ApplicationDbContext db,
    int officeId,
    IPrincipal user)
        {
            var query = GetEmployeeOfficeQuery(db, officeId, user);

            var result =
                from office in query

                join employee in db.EmployeeInfo
                    on office.EmployeeId equals employee.EmployeeId

                join level in db.LevelSetUp
                    on office.LevelId equals level.LevelId into levels
                from level in levels.DefaultIfEmpty()

                join service in db.ServiceSetUp
                    on office.ServiceId equals service.ServiceId into services
                from service in services.DefaultIfEmpty()

                join designation in db.DesignationSetUp
                    on office.DesignationId equals designation.DesignationId into designations
                from designation in designations.DefaultIfEmpty()

                join shift in db.EmployeeShiftTime
                    on office.EmployeeId equals shift.EmployeeId into shifts
                from shift in shifts.DefaultIfEmpty()

                where employee.Status != 2

                select new EmployeeAttendanceList
                {
                    OfficeId = office.OfficeId ?? 0,
                    LevelId = office.LevelId ?? 0,
                    DisplayOrder=employee.DisplayOrder,
                    LevelDisplayOrder = level != null
                        ? level.DisplayOrder
                        : 0,

                    EmployeeId = office.EmployeeId ?? 0,

                    ShiftTypeId = shift != null
                        ? shift.ShiftTypeId
                        : 0,

                    BranchId = office.BranchId ?? 0,
                    ServiceId = office.ServiceId ?? 0,

                    ServiceDisplayOrder = service != null
                        ? service.DisplayOrder
                        : 0,

                    DesignationId = office.DesignationId ?? 0,

                    DesignationDisplayOrder = designation != null
                        ? designation.DisplayOrder
                        : 0
                };

            return result
                     .OrderBy(x => x.DisplayOrder)

                     //.ThenBy(x => x.DesignationDisplayOrder)
                     .ToList();
        }

        public static List<EmployeeAttendanceList>
    GetEmpployeeListAccordingToOfficeAndPerDate(
        int officeId,
        DateTime logDate,
        IPrincipal user,
        bool GenerateReportStatus = false)
        {
            using (var db = new ApplicationDbContext())
            {
                return GetEmployeeList(db, officeId, user);
            }
        }

        public static List<EmployeeAttendanceList>
    GetEmployeeBy_Year_Month_OfficeIdList(
        int year,
        int month,
        int officeId,
        IPrincipal user,
        bool GenerateReportStatus = false)
        {
            using (var db = new ApplicationDbContext())
            {
                return GetEmployeeList(db, officeId, user);
            }
        }

        public static List<EmployeeAttendanceList>
    GetEmployeeBy_FromDate_ToDate_OfficeIdList(
        DateTime fromDate,
        DateTime toDate,
        int officeId,
        IPrincipal user,
        bool p)
        {
            using (var db = new ApplicationDbContext())
            {
                return GetEmployeeList(db, officeId, user);
            }
        }

        public static List<MonthlyAttendanceModel>
    GetEmployeeBy_FromDate_ToDate_OfficeIdListAnnual(
        DateTime fromDate,
        DateTime toDate,
        int officeId,
        IPrincipal user,
        bool p)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = GetEmployeeOfficeQuery(db, officeId, user);

                var result =
                    from office in query

                    join employee in db.EmployeeInfo
                        on office.EmployeeId equals employee.EmployeeId

                    join level in db.LevelSetUp
                        on office.LevelId equals level.LevelId into levels
                    from level in levels.DefaultIfEmpty()

                    join service in db.ServiceSetUp
                        on office.ServiceId equals service.ServiceId into services
                    from service in services.DefaultIfEmpty()

                    join designation in db.DesignationSetUp
                        on office.DesignationId equals designation.DesignationId into designations
                    from designation in designations.DefaultIfEmpty()

                    join shift in db.EmployeeShiftTime
                        on office.EmployeeId equals shift.EmployeeId into shifts
                    from shift in shifts.DefaultIfEmpty()

                    where employee.Status != 2

                    select new MonthlyAttendanceModel
                    {
                        OfficeId = office.OfficeId ?? 0,
                        LevelId = office.LevelId ?? 0,
                        DisplayOrder = employee.DisplayOrder,

                        LevelDisplayOrder = level != null
                            ? level.DisplayOrder
                            : 0,

                        EmployeeId = office.EmployeeId ?? 0,

                        ShiftTypeId = shift != null
                            ? shift.ShiftTypeId
                            : 0,

                        BranchId = office.BranchId ?? 0,

                        ServiceId = office.ServiceId ?? 0,

                        ServiceDisplayOrder = service != null
                            ? service.DisplayOrder
                            : 0,

                        DesignationId = office.DesignationId ?? 0,

                        DesignationDisplayOrder = designation != null
                            ? designation.DisplayOrder
                            : 0
                    };

                return result
                    .OrderBy(x => x.DisplayOrder)
                  
                   // .ThenBy(x => x.DesignationDisplayOrder)
                    .ToList();
            }
        }


        public static List<EmployeeAttendanceList> GetEmployeeAttandaneByOfficeWithInDateRange(int? empId, int officeId, DateTime fromDate, DateTime toDate)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var datefrom = "null";
            if (!string.IsNullOrEmpty(fromDate.ToString()))
            {
                datefrom = "'" + Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd") + "'";
            }

            var dateto = "null";
            if (!string.IsNullOrEmpty(toDate.ToString()))
            {
                dateto = "'" + Convert.ToDateTime(toDate).ToString("yyyy/MM/dd") + "'";
            }

            string s = "SpGetEmployeeAttandaneByOfficeWithInDateRange" + " " + empId + "," + officeId + "," + datefrom + "," + dateto;




            ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 180;

            var list = db.Database.SqlQuery<EmployeeAttendanceList>(s).ToList();

            return list;
        }




        public static List<EmployeeAttendanceList> GetEmployeeVisitSummaryByemployeeIdFromDateToDate(int? empId, DateTime frmDate, DateTime toDate)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {


                string s = "SpEmployeeVisitSummary" + " " + empId + "," + "'" + frmDate + "'" + "," + "'" + toDate + "'";




                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 180;

                var list = db.Database.SqlQuery<EmployeeAttendanceList>(s).ToList();

                return list;

            }
        }







        public static List<EmployeeLeaveSummaryList> GetEmployeeAttendanceLeaveSummary(int empId, DateTime monthFromDate, DateTime monthToDate)
        {
            ApplicationDbContext entities = new ApplicationDbContext();
            var datfrom = monthFromDate.Date.AddDays(-1);
            var datto = monthToDate.Date.AddDays(1);
            //FiscalYearSetUp fiscalYear = entities.FiscalYearSetUp
            //    .Where(x => x.FromDate >= datfrom && x.ToDate <= datto && x.Status == 1).FirstOrDefault();

            List<EmployeeLeaveSummaryList> list = new List<EmployeeLeaveSummaryList>();
            //if (fiscalYear.FiscalYearId > 0)
            //{
            List<EmployeeLeaveSummaryList> list2 = new List<EmployeeLeaveSummaryList>();
            list2 = (from ael in entities.AssignEmployeeLeave
                     join slt in entities.LeaveTypeSetUp on ael.LeaveTypeId equals slt.LeaveTypeId into slt
                     where (((ael.EmployeeId == empId) && (ael.CreatedDate >= datfrom && ael.CreatedDate <= datto)) && (ael.Status == 1))
                     select new EmployeeLeaveSummaryList
                     {
                         EmployeeId = ael.EmployeeId,
                         LeaveTypeId = ael.LeaveTypeId,
                         OpenningBalance = ael.OpenningBalance,
                         NoOfLeave = ael.NoOfLeave
                     }).ToList<EmployeeLeaveSummaryList>();
            List<EmployeeLeaveSummaryList> source = new List<EmployeeLeaveSummaryList>();
            source = (from la in entities.LeaveApplication
                      join slt in entities.LeaveTypeSetUp on la.LeaveTypeId equals slt.LeaveTypeId into slt
                      where ((((la.EmployeeId == empId) && (la.ApprovedStatus == 2)) && (la.Status == 1)) && ((la.FromDate > datfrom) || (la.ToDate <= datto)))
                      select new EmployeeLeaveSummaryList
                      {
                          LeaveApplicationId = la.LeaveApplicationId,
                          LeaveTypeId = la.LeaveTypeId,
                          FromDate = la.FromDate,
                          ToDate = la.ToDate
                      }).ToList<EmployeeLeaveSummaryList>();
            List<EmployeeLeaveSummaryList> list4 = (from la in entities.LeaveApplication
                                                    join slt in entities.LeaveTypeSetUp on la.LeaveTypeId equals slt.LeaveTypeId into slt
                                                    where ((((la.EmployeeId == empId) && (la.ApprovedStatus == 2)) && ((la.CreatedDate >= datfrom) && la.CreatedDate <= datto)))
                                                    select new EmployeeLeaveSummaryList
                                                    {
                                                        LeaveApplicationId = la.LeaveApplicationId,
                                                        LeaveTypeId = la.LeaveTypeId,
                                                        FromDate = la.FromDate,
                                                        ToDate = la.ToDate
                                                    }).ToList<EmployeeLeaveSummaryList>();
            foreach (EmployeeLeaveSummaryList item in list2)
            {
                DateTime time;
                DateTime time2;
                var list5 = new List<EmployeeLeaveSummaryList>();
                Func<LeaveTypeSetUp, bool> predicate = null;
                Func<LeaveApplication, bool> func2 = null;
                Func<LeaveApplication, bool> func3 = null;
                decimal num = 0M;
                decimal num2 = 0M;

                if (predicate == null)
                {
                    predicate = x => x.LeaveTypeId == item.LeaveTypeId;
                }

                var LeaveTypelist = entities.LeaveTypeSetUp.ToList();
                LeaveTypeSetUp model = LeaveTypelist.Where<LeaveTypeSetUp>(predicate).FirstOrDefault<LeaveTypeSetUp>();

                source = source.Where(x => x.LeaveTypeId == item.LeaveTypeId).ToList();

                foreach (EmployeeLeaveSummaryList model2 in source)
                {
                    ApplicationDbContext entitiesnew = new ApplicationDbContext();
                    time = Convert.ToDateTime(model2.FromDate);
                    time2 = Convert.ToDateTime(model2.ToDate);
                    for (DateTime day = time.Date; day.Date <= time2.Date; day = day.AddDays(1.0))
                    {
                        if ((day.Date >= monthFromDate.Date) && (day.Date <= monthToDate.Date))
                        {
                            list5 = (from holidayCalendar in entitiesnew.HolidayCalender
                                     where (holidayCalendar.Status == 1) && ((day > holidayCalendar.FromDate) && (day <= holidayCalendar.ToDate))
                                     select new EmployeeLeaveSummaryList
                                     {

                                         HolidayCalendarId = holidayCalendar.HolidayCalendarId,
                                         HolidayTypeName = holidayCalendar.HolidayTypeName
                                     }).ToList();
                            if (!model.HolidayInclude && model.WeeklyOffInclude)
                            {
                                if (list5.Count <= 0)
                                {
                                    num += model2.IsHalfDay ? Convert.ToDecimal((double)0.5) : 1M;
                                }
                            }
                            else if (model.HolidayInclude && !model.WeeklyOffInclude)
                            {
                                if (day.DayOfWeek.ToString() != "Saturday")
                                {
                                    num += model2.IsHalfDay ? Convert.ToDecimal((double)0.5) : 1M;
                                }
                            }
                            else if (!model.HolidayInclude && !model.WeeklyOffInclude)
                            {
                                if ((list5.Count <= 0) && (day.DayOfWeek.ToString() != "Saturday"))
                                {
                                    num += model2.IsHalfDay ? Convert.ToDecimal((double)0.5) : 1M;
                                }
                            }
                            else
                            {
                                num += model2.IsHalfDay ? Convert.ToDecimal((double)0.5) : 1M;
                            }
                        }
                    }
                }
                if (func3 == null)
                {


                    list4 = list4.Where(x => x.LeaveTypeId == item.LeaveTypeId).ToList();
                }
                foreach (EmployeeLeaveSummaryList model3 in list4)
                {
                    ApplicationDbContext entitiesnewnew = new ApplicationDbContext();
                    time = Convert.ToDateTime(model3.FromDate);
                    time2 = Convert.ToDateTime(model3.ToDate);
                    for (DateTime time3 = time.Date; time3.Date <= time2.Date; time3 = time3.AddDays(1.0))
                    {
                        if ((time3.Date >= datfrom) && (time3.Date <= datto))
                        {
                            list5 = (from holidayCalendar in entitiesnewnew.HolidayCalender
                                     where (holidayCalendar.Status == 1) && ((time3 > holidayCalendar.FromDate) && (time3 <= holidayCalendar.ToDate))
                                     select new EmployeeLeaveSummaryList
                                     {
                                         HolidayCalendarId = holidayCalendar.HolidayCalendarId,
                                         HolidayTypeName = holidayCalendar.HolidayTypeName
                                     }).ToList();
                            if (!model.HolidayInclude || !model.WeeklyOffInclude)
                            {
                                if (!model.HolidayInclude && model.WeeklyOffInclude)
                                {
                                    if (list5.Count <= 0)
                                    {
                                        num2 += model3.IsHalfDay ? Convert.ToDecimal((double)0.5) : 1M;
                                    }
                                }
                                else if (model.HolidayInclude && !model.WeeklyOffInclude)
                                {
                                    if (time3.DayOfWeek.ToString() != "Saturday")
                                    {
                                        num2 += model3.IsHalfDay ? Convert.ToDecimal((double)0.5) : 1M;
                                    }
                                }
                                else if ((!model.HolidayInclude && !model.WeeklyOffInclude) && ((list5.Count <= 0) && (time3.DayOfWeek.ToString() != "Saturday")))
                                {
                                    num2 += model3.IsHalfDay ? Convert.ToDecimal((double)0.5) : 1M;
                                }
                            }
                            else
                            {
                                num2 += model3.IsHalfDay ? Convert.ToDecimal((double)0.5) : 1M;
                            }
                        }
                    }
                }
                decimal num3 = item.OpenningBalance + item.NoOfLeave;
                decimal num4 = num3 - num2;
                EmployeeLeaveSummaryList list6 = new EmployeeLeaveSummaryList();
                decimal noOfLeave = 0M;
                if (Convert.ToDecimal(model.MaximumLimit) <= 0M)
                {
                    noOfLeave = item.NoOfLeave;
                }
                else
                {
                    noOfLeave = (item.NoOfLeave > Convert.ToDecimal(model.MaximumLimit)) ? Convert.ToDecimal(model.MaximumLimit) : item.NoOfLeave;
                }
                list6.LeaveTypeId = item.LeaveTypeId;
                list6.LeaveTypeName = model.LeaveTypeName;
                list6.LastYear = item.OpenningBalance;
                list6.ThisYear = noOfLeave;
                list6.Total = num3;
                list6.TakenLeave = num;
                list6.TakenLeaveYearly = num2;
                list6.RemainingLeave = (num4 > 0M) ? num4 : 0M;
                list.Add(list6);
            }
            //}
            return list;
        }













        public static List<EmployeeAttendanceList> GetEmployeeLeaveSummaryByEmployeeIdAndFiscalYearId(int? EmployeeId, string fromDate, string toDate)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            //FiscalYearSetUp year = (from x in _context.FiscalYearSetUp
            //                        where x.FiscalYearId == yearId
            //                        select x).FirstOrDefault<FiscalYearSetUp>();
            //DateTime? fromDate = year.FromDate;
            //DateTime? toDate = year.ToDate;

            string[] strArray = fromDate.Split(new char[] { '-' });
            string[] strArray2 = toDate.Split(new char[] { '-' });
            DateTime fDate = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2])));
            DateTime tDate = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(int.Parse(strArray2[0]), int.Parse(strArray2[1]), int.Parse(strArray2[2])));


            List<EmployeeAttendanceList> list = new List<EmployeeAttendanceList>();
            list = (from ael in _context.AssignEmployeeLeave
                    join slt in _context.LeaveTypeSetUp on ael.LeaveTypeId equals slt.LeaveTypeId into slt
                    where (ael.EmployeeId == EmployeeId) && (ael.Status == 1)
                    select new EmployeeAttendanceList
                    {
                        EmployeeId = ael.EmployeeId,
                        LeaveTypeId = ael.LeaveTypeId,
                        OpenningBalance = ael.OpenningBalance,
                        NoOfLeave = ael.NoOfLeave
                    }).ToList<EmployeeAttendanceList>();
            List<EmployeeAttendanceList> sourceOrginal = (from la in _context.LeaveApplication
                                                          join slt in _context.LeaveTypeSetUp on la.LeaveTypeId equals slt.LeaveTypeId into slt
                                                          where (((la.EmployeeId == EmployeeId) && (la.ApprovedStatus == 2)) && (la.Status == 1)) && ((la.FromDate > fDate) || (la.ToDate <= tDate))
                                                          select new EmployeeAttendanceList
                                                          {
                                                              LeaveApplicationId = la.LeaveApplicationId,
                                                              LeaveTypeId = la.LeaveTypeId,
                                                              FromDate = la.FromDate,
                                                              ToDate = la.ToDate
                                                          }).ToList<EmployeeAttendanceList>();



            List<EmployeeAttendanceList> source = new List<EmployeeAttendanceList>();



            List<EmployeeAttendanceList> list3 = new List<EmployeeAttendanceList>();
            foreach (EmployeeAttendanceList item in list)
            {
                int num = 0;
                LeaveTypeSetUp model = _context.LeaveTypeSetUp.Where(x => x.LeaveTypeId == item.LeaveTypeId)
                    .FirstOrDefault();
                if (item.LeaveTypeId != null)
                {
                    source = sourceOrginal.Where(x => x.LeaveTypeId == item.LeaveTypeId).ToList();
                }


                foreach (EmployeeAttendanceList model2 in source)
                {
                    DateTime time = Convert.ToDateTime(model2.FromDate);
                    DateTime time2 = Convert.ToDateTime(model2.ToDate);
                    for (DateTime day = time.Date; day.Date <= time2.Date; day = day.AddDays(1.0))
                    {
                        if ((day.Date >= fDate) && (day.Date <= tDate))
                        {
                            var list4 = (from holidayCalendar in _context.HolidayCalender
                                         where (holidayCalendar.Status == 1) && ((day > holidayCalendar.FromDate) && (day <= holidayCalendar.ToDate))
                                         select new
                                         {
                                             HolidayCalendarId = holidayCalendar.HolidayCalendarId,
                                             HolidayTypeName = holidayCalendar.HolidayTypeName
                                         }).ToList();
                            if (!model.HolidayInclude || !model.WeeklyOffInclude)
                            {
                                if (!model.HolidayInclude && model.WeeklyOffInclude)
                                {
                                    if (list4.Count <= 0)
                                    {
                                        num++;
                                    }
                                }
                                else if (model.HolidayInclude && !model.WeeklyOffInclude)
                                {
                                    if (day.DayOfWeek.ToString() != "Saturday")
                                    {
                                        num++;
                                    }
                                }
                                else if ((!model.HolidayInclude && !model.WeeklyOffInclude) && ((list4.Count <= 0) && (day.DayOfWeek.ToString() != "Saturday")))
                                {
                                    num++;
                                }
                            }
                            else
                            {
                                num++;
                            }
                        }
                    }
                }
                int num2 = Convert.ToInt32((decimal)(item.OpenningBalance + item.NoOfLeave));
                int num3 = num2 - num;
                EmployeeAttendanceList list5 = new EmployeeAttendanceList
                {
                    LeaveTypeId = item.LeaveTypeId,
                    LeaveTypeName = model.LeaveTypeName,
                    LastYear = item.OpenningBalance,
                    ThisYear = item.NoOfLeave,
                    Total = num2,
                    TakenLeave = num,
                    RemainingLeave = num3
                };
                list3.Add(list5);
            }
            return list3;
        }


    }
}