using System;

namespace eAttendance.ReportModel
{
    public class DailyPresentAttendanceReport
    {
        // Properties
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int EmployeeNo { get; set; }

        public string DesignationName { get; set; }

        public int ShiftTime { get; set; }

        public DateTime? InTime { get; set; }

        public DateTime? OutTime { get; set; }
    }
}

 

 
