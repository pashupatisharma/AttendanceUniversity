using eAttendance.ReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ViewModel
{
    public class MonthlyAttendanceModel
    {
        // Properties
        public int EmployeeId { get; set; }

        public int EmployeeNo { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeNameNp { get; set; }

        public string DesignationName { get; set; }

        public string LevelName { get; set; }

        public int OrderNo { get; set; }

        public DateTime LogDate { get; set; }

        public int DayType { get; set; }

        public int IsHoliday { get; set; }

        public string HolidayType { get; set; }

        public int IsOnVisit { get; set; }

        public int IsOnLeave { get; set; }

        public TimeSpan CheckIn { get; set; }

        public TimeSpan CheckOut { get; set; }

        public string AttendanceStatus { get; set; }



        public List<MonthlyAttendanceModel> MonthlyAttendanceModelList { get; set; }
        public List<MonthlyAttendanceModel> FilteredEmployeeList { get; set; }
        public List<EmployeeLeaveSummaryList> EmployeeLeaveSummaryList { get; set; }


        public int YearId { get; set; }


        public int BranchId { get; set; }

        public int ServiceId { get; set; }

        public int LevelId { get; set; }

        public int DesignationId { get; set; }

        public object LeaveTypeName { get; set; }

        public object LastYear { get; set; }

        public object ThisYear { get; set; }

        public object Total { get; set; }

        public object TakenLeaveYearly { get; set; }

        public object RemainingLeave { get; set; }

        public int OfficeId { get; set; }

        public int LevelDisplayOrder { get; set; }

        public int ServiceDisplayOrder { get; set; }

        public int DesignationDisplayOrder { get; set; }
    }
}