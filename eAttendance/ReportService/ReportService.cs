using eAttendance;
using eAttendance.Models;
using eAttendance.ReportModel;
using eAttendance.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportService
{
    public static class ReportService
    {
        public static List<EmployeeAttendanceList> GetEmpployeeListAccordingToOfficeAndPerDate(int officeId, DateTime logDate, bool GenerateReportStatus = false)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<EmployeeAttendanceList> list = new List<EmployeeAttendanceList>();
            foreach (var m in db.EmployeeOfficeDetail.Where(x => x.OfficeId == officeId))
            {
                ApplicationDbContext dbnew = new ApplicationDbContext();
                if (dbnew.EmployeeInfo.Where(x => x.EmployeeId == m.EmployeeId).FirstOrDefault().Status != 2)
                {
                    EmployeeAttendanceList obj = new EmployeeAttendanceList();
                    obj.OfficeId = (int)m.OfficeId;
                    obj.LevelId = (int)m.LevelId;
                    obj.LevelDisplayOrder = dbnew.LevelSetUp.Where(x => x.LevelId == m.LevelId).FirstOrDefault().DisplayOrder;
                    obj.EmployeeId = (int)m.EmployeeId;
                    obj.BranchId = (int)m.BranchId;
                    obj.ServiceId = (int)m.ServiceId;
                    obj.ServiceDisplayOrder = dbnew.ServiceSetUp.Where(x => x.ServiceId == m.ServiceId).FirstOrDefault().DisplayOrder;
                    obj.DesignationId = (int)m.DesignationId;
                    obj.DesignationDisplayOrder = dbnew.DesignationSetUp.Where(x => x.DesignationId == m.DesignationId).FirstOrDefault().DisplayOrder;
                    list.Add(obj);
                }
            }


            return list.OrderBy(m => m.ServiceDisplayOrder).OrderBy(m => m.LevelDisplayOrder).OrderBy(m => m.DesignationDisplayOrder).ToList();
        }


        public static List<EmployeeAttendanceList> GetEmployeeBy_Year_Month_OfficeIdList(int year, int month, int officeId, bool GenerateReportStatus = false)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<EmployeeAttendanceList> list = new List<EmployeeAttendanceList>();
            foreach (var m in db.EmployeeOfficeDetail.Where(x => x.OfficeId == officeId))
            {
                ApplicationDbContext dbnew = new ApplicationDbContext();
                if (dbnew.EmployeeInfo.Where(x => x.EmployeeId == m.EmployeeId).FirstOrDefault().Status != 2)
                {
                    EmployeeAttendanceList obj = new EmployeeAttendanceList();
                    obj.OfficeId = (int)m.OfficeId;
                    obj.LevelId = (int)m.LevelId;
                    obj.LevelDisplayOrder = dbnew.LevelSetUp.Where(x => x.LevelId == m.LevelId).FirstOrDefault().DisplayOrder;
                    obj.EmployeeId = (int)m.EmployeeId;
                    obj.BranchId = (int)m.BranchId;
                    obj.ServiceId = (int)m.ServiceId;
                    obj.ServiceDisplayOrder = dbnew.ServiceSetUp.Where(x => x.ServiceId == m.ServiceId).FirstOrDefault().DisplayOrder;
                    obj.DesignationId = (int)m.DesignationId;
                    obj.DesignationDisplayOrder = dbnew.DesignationSetUp.Where(x => x.DesignationId == m.DesignationId).FirstOrDefault().DisplayOrder;
                    list.Add(obj);
                }
            }


            return list.OrderBy(m => m.ServiceDisplayOrder).OrderBy(m => m.LevelDisplayOrder).OrderBy(m => m.DesignationDisplayOrder).ToList();
    
        }


        public static List<EmployeeAttendanceList> GetEmployeeBy_FromDate_ToDate_OfficeIdList(DateTime fromDate, DateTime toDate, int officeId, bool p)
        {
           ApplicationDbContext db = new ApplicationDbContext();
            var e = db.EmployeeInfo.Where(x => x.EmployeeId == 1).FirstOrDefault();

            List<EmployeeAttendanceList> list = new List<EmployeeAttendanceList>();
            foreach (var item in db.EmployeeOfficeDetail.Where(x => x.OfficeId == officeId))

            {
                ApplicationDbContext dbnew = new ApplicationDbContext();
                var exist = dbnew.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                int? status = exist.Status;
                if (status != 2)
                {
                    EmployeeAttendanceList obj = new EmployeeAttendanceList();
                    obj.OfficeId = (int)item.OfficeId;
                    obj.LevelId = (int)item.LevelId;
                    obj.LevelDisplayOrder = dbnew.LevelSetUp.Where(x => x.LevelId == item.LevelId).FirstOrDefault().DisplayOrder;
                    obj.EmployeeId = (int)item.EmployeeId;
                    obj.BranchId = (int)item.BranchId;
                    obj.ServiceId = (int)item.ServiceId;
                    obj.ServiceDisplayOrder = dbnew.ServiceSetUp.Where(x => x.ServiceId == item.ServiceId).FirstOrDefault().DisplayOrder;
                    obj.DesignationId = (int)item.DesignationId;
                    obj.DesignationDisplayOrder = dbnew.DesignationSetUp.Where(x => x.DesignationId == item.DesignationId).FirstOrDefault().DisplayOrder;
                    list.Add(obj);
                }
            }


            return list.OrderBy(m => m.ServiceDisplayOrder).OrderBy(m => m.LevelDisplayOrder).OrderBy(m => m.DesignationDisplayOrder).ToList();
    
        }


        public static List<MonthlyAttendanceModel> GetEmployeeBy_FromDate_ToDate_OfficeIdListAnnual(DateTime fromDate, DateTime toDate, int officeId, bool p)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<MonthlyAttendanceModel> list = new List<MonthlyAttendanceModel>();
            foreach (var m in db.EmployeeOfficeDetail.Where(x => x.OfficeId == officeId))
            {
                ApplicationDbContext dbnew = new ApplicationDbContext();

                if (dbnew.EmployeeInfo.Where(x => x.EmployeeId == m.EmployeeId).FirstOrDefault().Status != 2)
                {
                   
                    MonthlyAttendanceModel obj = new MonthlyAttendanceModel();
                    obj.OfficeId = (int)m.OfficeId;
                    obj.LevelId = (int)m.LevelId;
                    obj.LevelDisplayOrder = dbnew.LevelSetUp.Where(x => x.LevelId == m.LevelId).FirstOrDefault().DisplayOrder;
                    obj.EmployeeId = (int)m.EmployeeId;
                    obj.BranchId = (int)m.BranchId;
                    obj.ServiceId = (int)m.ServiceId;
                    obj.ServiceDisplayOrder = dbnew.ServiceSetUp.Where(x => x.ServiceId == m.ServiceId).FirstOrDefault().DisplayOrder;
                    obj.DesignationId = (int)m.DesignationId;
                    obj.DesignationDisplayOrder = dbnew.DesignationSetUp.Where(x => x.DesignationId == m.DesignationId).FirstOrDefault().DisplayOrder;
                    list.Add(obj);
                }
            }


            return list.OrderBy(m => m.ServiceDisplayOrder).OrderBy(m => m.LevelDisplayOrder).OrderBy(m => m.DesignationDisplayOrder).ToList();
    
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