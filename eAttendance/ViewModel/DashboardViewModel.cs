using eAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ViewModel
{
    public class DashboardViewModel
    {
        public string NepaliDate { get; set; }

        public List<DailyAttendanceForDashboardModel> DailyAttendanceList { get; set; }

        public List<MonthlyAttendanceModel> EmployeeAttendanceList { get; set; }

       //public EmployeeAttendanceSummaryForDashboardModel EmployeeAttendanceSummaryForDashboard { get; set; }

        public List<LeaveApplicationModel> LeaveApplicationList { get; set; }

        public List<VisitApplicationModel> VisitApplicationList { get; set; }

        public List<DailyAttendanceForAdministratorDashboardModel> DailyAttendanceForAdministratorDashboardList { get; set; }

        public AttendanceCountModel TodayAttendanceCount { get; set; }

        public List<LeaveApplication> EmployeeOnLeave { get; set; }

        public List<VisitApplication> EmployeeOnVisit { get; set; }
    }

    public class EmployeeCountDashBoardModl
    {
        public DateTime? InDateTime { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeNo { get; set; }
        public string EmployeeNameNp { get; set; }

        public int? DesignationId { get; set; }

        public string Remarks { get; set; }

        public int TotalEmployee{ get; set; }

        public int ActiveEmployee { get; set; }

        public int InactiveEmployee { get; set; }

        public int Present { get; set; }

        public int OnLeave { get; set; }

        public int OnVisit { get; set; }

        public string OfficeName { get; set; }

        public int Status { get; set; }
        public int OfficeId { get; set; }
    }
}