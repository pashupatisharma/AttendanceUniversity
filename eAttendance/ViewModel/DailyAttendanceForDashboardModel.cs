using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ViewModel
{
    public class DailyAttendanceForDashboardModel
    {
        public int EmployeeId { get; set; }

        public int EmployeeNo { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeNameNp { get; set; }

        public string DepartmentName { get; set; }

        public DateTime InDateTime { get; set; }

        public DateTime OutOutTime { get; set; }

        public int AttendanceType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}